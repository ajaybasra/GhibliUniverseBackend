using System.Collections;

namespace GhibliUniverse.Console.Tests;

public class FakeWriter : IWriter
{
    private readonly ArrayList _outputList = new();
    public void Write<t>(t output)
    {
        _outputList.Add(output);
    }

    public void WriteLine<t>(t output)
    {
        _outputList.Add(output);
    }

    public ArrayList GetOutput()
    {
        return _outputList;
    }
}