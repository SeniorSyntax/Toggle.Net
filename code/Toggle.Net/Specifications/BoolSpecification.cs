using System.Collections.Generic;

namespace Toggle.Net.Specifications;

public class BoolSpecification : IToggleSpecification
{
    private readonly bool _value;

    public BoolSpecification(bool value)
    {
        _value = value;
    }
        
    public bool IsEnabled(string currentUser, IDictionary<string, string> parameters)
    {
        return _value;
    }
}