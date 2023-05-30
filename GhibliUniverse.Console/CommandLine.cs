namespace GhibliUniverse.Console;

public class CommandLine : ICommandLine
{
    public string[] GetCommandLineArgs()
    {
        return Environment.GetCommandLineArgs();
    }
}