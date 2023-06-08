namespace GhibliUniverse.Console;

public class ConsoleWriter : IWriter
{
    public void Write<t>(t output)
    {
        System.Console.Write(output);
    }

    public void WriteLine<t>(t output)
    {
        System.Console.WriteLine(output);
    }
}