using System;
using System.Threading;
using System.Threading.Tasks;
using Meadow;
using Meadow.Foundation;
using Meadow.Hardware;
using Meadow.Peripherals.Sensors;
using Meadow.Units;

namespace BrewIoT.Device.Meadow.Sensors;

public class Max31865TemperatureSensor : SamplingSensorBase<Temperature>, ITemperatureSensor
{
    private readonly ISpiBus spiBus;
    private readonly IDigitalOutputPort spiCommsChipSelect;
    private readonly ISpiCommunications spiComms;
    private readonly KnownSensorType knownSensorType;
    private readonly IRtdConverter rtdConverter;

    public Temperature? Temperature { get; private set; }

    public Max31865TemperatureSensor(ISpiBus spiBus, IDigitalOutputPort spiCommsChipSelect, Frequency frequency, KnownSensorType knownSensorType, IRtdConverter rtdConverter)
    {
        this.spiBus = spiBus;
        this.spiCommsChipSelect = spiCommsChipSelect;
        this.spiComms = new SpiCommunications(spiBus, spiCommsChipSelect, frequency, SpiClockConfiguration.Mode.Mode1);

        this.knownSensorType = knownSensorType;
        this.rtdConverter = rtdConverter;
    }

    public Max31865TemperatureSensor(ISpiBus spiBus, IDigitalOutputPort spiCommsChipSelect, KnownSensorType knownSensorType)
        : this(spiBus, spiCommsChipSelect, new(5_000_000, Frequency.UnitType.Hertz), knownSensorType, new DirectMathematicalMethod())
    {
    }

    public byte ReadFault()
    {
        return this.spiComms.ReadRegister(Registers.FaultStat);
    }

    private async Task<ushort> ReadInternal()
    {
        ClearFault();
        Enable(Configuration.Bias);
        await Task.Delay(10);

        Enable(Configuration.OneShot);
        await Task.Delay(65);

        var rtd = this.spiComms.ReadRegisterAsUShort(Registers.RtdMsb, ByteOrder.LittleEndian);

        // Disable bias current again to reduce self-heating.
        Disable(Configuration.Bias);

        // // remove the fault bit
        rtd >>= 1;

        return rtd;
    }

    public void ClearFault()
    {
        byte value = this.spiComms.ReadRegister(Registers.ConfigurationRead);

        // Page 14 (Fault Status Clear (D1)) of the technical documentation
        value = (byte)(value & ~0x2C);
        value |= Configuration.FaultStat;

        this.spiComms.WriteRegister(Registers.ConfigurationWrite, value);
    }

    private void Enable(byte configuration)
    {
        byte value = this.spiComms.ReadRegister(Registers.ConfigurationRead);

        value |= configuration;

        this.spiComms.WriteRegister(Registers.ConfigurationWrite, value);
    }

    private void Disable(byte configuration)
    {
        byte value = this.spiComms.ReadRegister(Registers.ConfigurationRead);

        value &= (byte)~configuration;

        this.spiComms.WriteRegister(Registers.ConfigurationWrite, value);
    }

    public void Initialize()
    {
        byte value = Configuration.TwoFourWire | Configuration.Filter60Hz;

        Resolver.Log.Info($"Initialize: {value}");

        this.spiComms.WriteRegister(Registers.ConfigurationWrite, value);

        // SetWires(Wires.TwoWire);
        // Disable(Configuration.Bias);
        // Disable(Configuration.ModeAuto);
        // // SetThresholds(0, 0xFFFF);
        // ClearFault();
    }

    private void SetThresholds(ushort lower, ushort upper)
    {
        this.spiComms.WriteRegister(Registers.LFaultLsb, (byte)(lower & 0xFF));
        this.spiComms.WriteRegister(Registers.LFaultMsb, (byte)(lower >> 8));
        this.spiComms.WriteRegister(Registers.HFaultLsb, (byte)(upper & 0xFF));
        this.spiComms.WriteRegister(Registers.HFaultMsb, (byte)(upper >> 8));
    }

    private void SetWires(Wires wires)
    {
        byte t = this.spiComms.ReadRegister(Registers.ConfigurationRead);

        if (wires == Wires.ThreeWire)
        {
            t |= Configuration.ThreeWire;
        }
        else
        {
            // 2 or 4 wire
            t = (byte)(t & ~Configuration.ThreeWire);
        }

        this.spiComms.WriteRegister(Registers.ConfigurationWrite, t);
    }

    protected override async Task<Temperature> ReadSensor()
    {
        var rtd = await ReadInternal();

        return new Temperature(this.rtdConverter.Convert(rtd, (float)this.knownSensorType, 430));
    }

    /// <summary>
    /// Starts continuously sampling the sensor.
    ///
    /// This method also starts raising `Changed` events and IObservable
    /// subscribers getting notified. Use the `readIntervalDuration` parameter
    /// to specify how often events and notifications are raised/sent.
    /// </summary>
    /// <param name="updateInterval">A `TimeSpan` that specifies how long to
    /// wait between readings. This value influences how often `*Updated`
    /// events are raised and `IObservable` consumers are notified.
    ///</param>
    public override void StartUpdating(TimeSpan? updateInterval = null)
    {
        lock (samplingLock)
        {
            if (IsSampling) { return; }
            IsSampling = true;

            if (updateInterval.HasValue)
            {
                TimeSpan valueOrDefault = updateInterval.GetValueOrDefault();
                base.UpdateInterval = valueOrDefault;
            }

            SamplingTokenSource = new CancellationTokenSource();
            CancellationToken ct = SamplingTokenSource.Token;
            Task.Run(async delegate
            {
                while (!ct.IsCancellationRequested)
                {
                    float rtd = await ReadInternal();
                    float temperature = this.rtdConverter.Convert(rtd, (int)this.knownSensorType, 430);
                    Temperature = new Temperature(temperature);
                    // ChangeResult<Voltage> changeResult = new ChangeResult<Voltage>(voltage, PreviousVoltageReading);
                    // RaiseChangedAndNotify(changeResult);
                    // PreviousVoltageReading = voltage;
                    await Task.Delay(base.UpdateInterval);
                }

                // base.Observers.ForEach(delegate (IObserver<IChangeResult<Voltage>> x)
                // {
                //     x.OnCompleted();
                // });
            }, SamplingTokenSource.Token).RethrowUnhandledExceptions(SamplingTokenSource.Token);
        }
    }

    /// <summary>
    /// Stops sampling the temperature
    /// </summary>
    public override void StopUpdating()
    {
        lock (samplingLock)
        {
            if (!IsSampling) { return; }
            IsSampling = false;
            //AnalogInputPort.StopUpdating();
        }
    }

    private static class Configuration
    {
        public const byte Bias = 0b_1000_0000;
        public const byte ModeAuto = 0b_0100_0000;
        public const byte ModeOff = 0x00;
        public const byte OneShot = 0b_0010_0000;
        public const byte ThreeWire = 0b_0001_0000;
        public const byte TwoFourWire = 0b_0000_0000;
        public const byte FaultStat = 0b_0000_0010;
        public const byte Filter50Hz = 0b_0000_0001;
        public const byte Filter60Hz = 0b_0000_0000;
    }

    private static class Registers
    {
        public const byte ConfigurationRead = 0x00;
        public const byte RtdMsb = 0x01;
        public const byte RtdLsb = 0x02;
        public const byte HFaultMsb = 0x03;
        public const byte HFaultLsb = 0x04;
        public const byte LFaultMsb = 0x05;
        public const byte LFaultLsb = 0x06;
        public const byte FaultStat = 0x07;
        public const byte ConfigurationWrite = 0x80;
    }

    public static class Fault
    {
        public const byte HighThresh = 0x80;
        public const byte LowThresh = 0x40;
        public const byte RefInLow = 0x20;
        public const byte RefInHigh = 0x10;
        public const byte RtdInLow = 0x08;
        public const byte OvUv = 0x04;
    }

    public enum Wires : byte
    {
        TwoWire = 0,
        ThreeWire = 1,
        FourWire = 0
    }

    public enum KnownSensorType : short
    {
        PT100 = 100,
        PT1000 = 1000
    }
}

public interface IRtdConverter
{
    float Convert(float raw, float nominal, float referenceResistor);
}

public class DirectMathematicalMethod : IRtdConverter
{
    private const float RTD_A = 3.9083e-3f;
    private const float RTD_B = -5.775e-7f;

    public float Convert(float rtdRaw, float rtdNominal, float refResistor)
    {
        float Z1, Z2, Z3, Z4, resistance, temp;
        
        // This math originates from:
        // http://www.analog.com/media/en/technical-documentation/application-notes/AN709_0.pdf
        resistance = rtdRaw;
        Resolver.Log.Info($"rtdRaw: {rtdRaw}");
        resistance /= 32768;
        resistance *= refResistor;

        Resolver.Log.Info($"Rt: {resistance}");

        Z1 = -RTD_A;
        Z2 = RTD_A * RTD_A - (4 * RTD_B);
        Z3 = (4 * RTD_B) / rtdNominal;
        Z4 = 2 * RTD_B;

        temp = Z2 + (Z3 * resistance);
        temp = (MathF.Sqrt(temp) + Z1) / Z4;

        if (temp >= 0)
            return temp;

        // ugh.
        resistance /= rtdNominal;
        resistance *= 100; // normalize to 100 ohm

        float rpoly = resistance;

        temp = -242.02f;
        temp += 2.2228f * rpoly;
        rpoly *= resistance; // square
        temp += 2.5859e-3f * rpoly;
        rpoly *= resistance; // ^3
        temp -= 4.8260e-6f * rpoly;
        rpoly *= resistance; // ^4
        temp -= 2.8183e-8f * rpoly;
        rpoly *= resistance; // ^5
        temp += 1.5243e-10f * rpoly;

        return temp;
    }
}