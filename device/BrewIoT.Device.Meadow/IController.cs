using System.Collections.Generic;

namespace BrewIoT.Device.Meadow;

public interface IController
{
    void Initialize(IReadOnlyDictionary<string, string> settings);
    void Run();
}