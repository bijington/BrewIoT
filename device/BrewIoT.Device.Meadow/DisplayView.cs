using Meadow.Foundation.Displays.Lcd;

namespace BrewIoT.Device.Meadow;

public class DisplayView
{
    CharacterDisplay display;
        
    public DisplayView()
    {
        Initialize();
    }

    void Initialize()
    {
        display = new CharacterDisplay
        (
            pinRS:  MeadowApp.Device.Pins.D10,
            pinE:   MeadowApp.Device.Pins.D09,
            pinD4:  MeadowApp.Device.Pins.D08,
            pinD5:  MeadowApp.Device.Pins.D07,
            pinD6:  MeadowApp.Device.Pins.D06,
            pinD7:  MeadowApp.Device.Pins.D05,
            rows: 4, columns: 20
        );
    }

    public void WriteLine(string text, byte lineNumber) 
    {
        display.WriteLine($"{text}", lineNumber);
    }
}
