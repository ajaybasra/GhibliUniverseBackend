using GhibliUniverse.Interfaces;

namespace GhibliUniverse;

public class CommandLine : ICommandLine
{
    public string[] GetCommandLineArgs()
    {
        return Environment.GetCommandLineArgs();
    }
}