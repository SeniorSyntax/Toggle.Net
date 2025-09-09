using System.Collections.Generic;
using Toggle.Net.Configuration;

namespace Toggle.Net.Specifications;

public class RcSpecification : IToggleSpecification
{
    public bool IsEnabled(ToggleMode toggleMode, string currentUser, IDictionary<string, string> parameters) => 
        toggleMode != ToggleMode.Customer;
}