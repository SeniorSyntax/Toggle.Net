using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Toggle.Net.Specifications;

public class RegExSpecification(Regex regex) : IToggleSpecification, IToggleSpecificationValidator
{
	private const string regExParameter = "pattern";

	public const string MustDeclareRegexPattern = "Missing parameter '" + regExParameter + "' for Feature '{0}'.";

	public bool IsEnabled(string currentUser, IDictionary<string, string> parameters)
	{
		return regex.IsMatch(parameters[regExParameter]);
	}

	public void Validate(string toggleName, IDictionary<string, string> parameters)
	{
		if (!parameters.ContainsKey(regExParameter))
			throw new InvalidSpecificationParameterException(string.Format(MustDeclareRegexPattern, toggleName));
	}
}