using System.Collections.Generic;

namespace Toggle.Net.Specifications;

public class BoolSpecification(bool value) : IToggleSpecification
{
    public bool IsEnabled(string currentUser, IDictionary<string, string> parameters)
    {
        return value;
    }
}