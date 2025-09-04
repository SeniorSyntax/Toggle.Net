using System;
using System.Collections.Generic;
using System.Linq;

namespace Toggle.Net.Specifications;

/// <summary>
/// Is enabled for certain user(s).
/// If parameter value contains comma(s),
/// it is treated as a list.
/// If not, a single string as currentUser is expected.
/// </summary>
public class UserSpecification : IToggleSpecification, IToggleSpecificationValidator
{
	public const string MustHaveDeclaredIds = "Missing UserSpecification parameter '" + idsParameter + "' for feature '{0}'.";

	private const string idsParameter = "ids";
	private const char delimiter = ',';

	public bool IsEnabled(string currentUser, IDictionary<string, string> parameters)
	{
		var currentUserContainsDelimiter = currentUser.Contains(delimiter);
		var parameterValues = parameters[idsParameter];

		if (currentUserContainsDelimiter)
		{
			return parameterValues.Equals(currentUser, StringComparison.OrdinalIgnoreCase);
		}

		var values = parameterValues.Split(delimiter);
		return values.Any(value => value.Trim().Equals(currentUser, StringComparison.OrdinalIgnoreCase));
	}

	public void Validate(string toggleName, IDictionary<string, string> parameters)
	{
		if (!parameters.Keys.Contains(idsParameter))
		{
			throw new InvalidSpecificationParameterException(string.Format(MustHaveDeclaredIds, toggleName));
		}
	}
}