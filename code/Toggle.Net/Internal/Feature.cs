using System;
using System.Collections.Generic;
using System.Linq;
using Toggle.Net.Specifications;

namespace Toggle.Net.Internal;

public class Feature
{
	private readonly IDictionary<IToggleSpecification, IDictionary<string, string>> _specificationData;

	public Feature(IToggleSpecification specification)
	{
		_specificationData = new Dictionary<IToggleSpecification, IDictionary<string, string>>();
		AddSpecification(specification);
	}

	public bool IsEnabled(string currentUser)
	{
		return _specificationData.Keys.All(specification =>
			specification.IsEnabled(currentUser, _specificationData[specification]));
	}

	public void AddSpecification(IToggleSpecification specification)
	{
		if(specification==null)
			throw new ArgumentNullException(nameof(specification));
		_specificationData.Add(specification, new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));
	}

	public void AddParameter(IToggleSpecification specification, string parameterName, string parameterValue)
	{
		_specificationData[specification].Add(parameterName, parameterValue);
	}

	public void Validate(string toggleName)
	{
		foreach (var specification in _specificationData)
		{
			if(specification.Key is IToggleSpecificationValidator specValidator)
			{
				specValidator.Validate(toggleName, specification.Value);
			}
		}
	}
}