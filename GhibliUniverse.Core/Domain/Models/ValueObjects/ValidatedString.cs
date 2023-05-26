using ValueOf;

namespace GhibliUniverse.ValueObjects;

public class ValidatedString : ValueOf<string, ValidatedString>
{
    protected override void Validate()
    {
        if (string.IsNullOrWhiteSpace(Value))
        {
            throw new ArgumentException("Value cannot be null or empty");
        }
    }
    
}