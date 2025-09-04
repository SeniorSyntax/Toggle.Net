using System;
using System.Collections.Generic;

namespace Toggle.Net.Specifications;

public class RandomSpecification : IToggleSpecification, IToggleSpecificationValidator
{
	private const string percentParameter = "percent";
		
	public const string MustHaveDeclaredPercent = "Missing RandomSpecification parameter '" + percentParameter + "' for feature '{0}'.";
	public const string MustDeclaredPercentAsInt = "RandomSpecification parameter '" + percentParameter + "' for feature '{0}' must be declared as an int.";
	public const string MustBeBetween0And100 = "RandomSpecification parameter '" + percentParameter + "' for feature '{0}' must be between 0 and 100.";

	public bool IsEnabled(string currentUser, IDictionary<string, string> parameters)
	{
		var percent = Convert.ToInt32(parameters[percentParameter]);
		var userHash = currentUser.GetHashCode();
		var userHash1To100 = Math.Abs(userHash%100);

		return percent > userHash1To100;
	}

	public void Validate(string toggleName, IDictionary<string, string> parameters)
	{
		if (!parameters.TryGetValue(percentParameter, out var parameterValue))
		{
			throw new InvalidSpecificationParameterException(string.Format(MustHaveDeclaredPercent, toggleName));
		}
		if (!int.TryParse(parameterValue, out var percent))
		{
			throw new InvalidSpecificationParameterException(string.Format(MustDeclaredPercentAsInt, toggleName));
		}
		if (percent < 0 || percent > 100)
		{
			throw new InvalidSpecificationParameterException(string.Format(MustBeBetween0And100, toggleName));
		}
	}
}