using System.Collections.Generic;
using System.Threading.Tasks;

namespace BrewIoT.Device.Meadow;

public interface IController
{
    void Initialize(IReadOnlyDictionary<string, string> settings);

    Task Read();
    void Write();
}