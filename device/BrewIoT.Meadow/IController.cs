using System.Collections.Generic;

namespace BrewIot.Meadow;

public interface IController
{
    void Initialize(IReadOnlyDictionary<string, string> settings);
    void Run();
}