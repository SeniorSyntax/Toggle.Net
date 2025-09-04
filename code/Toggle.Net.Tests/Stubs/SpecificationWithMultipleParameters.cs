using System.Collections.Generic;
using Toggle.Net.Specifications;

namespace Toggle.Net.Tests.Stubs;

public class SpecificationWithMultipleParameters : IToggleSpecification
{
	public const string ParameterName1 = "TheParameterName1";
	public const string ParameterName2 = "TheParameterName2";
	public const string ParameterName3 = "TheParameterName3";
	
	public bool IsEnabled(string currentUser, IDictionary<string, string> parameters)
	{
		return bool.Parse(parameters[ParameterName1]) &&
		       bool.Parse(parameters[ParameterName2]) &&
		       bool.Parse(parameters[ParameterName3]);
	}
}