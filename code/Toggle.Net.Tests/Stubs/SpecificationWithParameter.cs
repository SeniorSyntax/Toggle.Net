using System.Collections.Generic;
using Toggle.Net.Configuration;
using Toggle.Net.Specifications;

namespace Toggle.Net.Tests.Stubs;

public class SpecificationWithParameter : IToggleSpecification
{
	public const string ParameterName = "TheParameterName";

	public bool IsEnabled(ToggleMode toggleMode, string currentUser, IDictionary<string, string> parameters)
	{
		return bool.Parse(parameters[ParameterName]);
	}
}