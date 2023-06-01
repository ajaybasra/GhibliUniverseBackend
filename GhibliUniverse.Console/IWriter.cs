namespace GhibliUniverse.Console;

public interface IWriter
{
    void Write<t>(t output);
    void WriteLine<t>(t output);
}