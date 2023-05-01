using GhibliUniverse.Interfaces;

namespace GhibliUniverse;

public class ConsoleWriter : IWriter
{
    public void Write(string output)
    {
        Console.Write(output);
    }

    public void WriteLine(string output)
    {
        Console.WriteLine(output);
    }
}