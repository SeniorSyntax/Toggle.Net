using System.Collections.Generic;
using Toggle.Net.Configuration;
using Toggle.Net.Providers;
using Toggle.Net.Specifications;

namespace Toggle.Net.Internal;

public class ToggleChecker : IToggleChecker
{
	private readonly IEnumerable<IFeatureProvider> _featureProviders;
	private readonly IToggleSpecification _defaultToggleSpecification;
	private readonly IUserProvider _userProvider;

	internal ToggleChecker(IEnumerable<IFeatureProvider> featureProviders, IToggleSpecification defaultToggleSpecification, IUserProvider userProvider)
	{
		_featureProviders = featureProviders;
		_defaultToggleSpecification = defaultToggleSpecification;
		_userProvider = userProvider;
	}

	public bool IsEnabled(string toggleName)
	{
		var currentUser = _userProvider.CurrentUser();
		foreach (var featureProvider in _featureProviders)
		{
			var feature = featureProvider.Get(toggleName);
			if (feature != null)
			{
				return feature.IsEnabled(currentUser);
			}
		}
		return _defaultToggleSpecification.IsEnabled(currentUser, new Dictionary<string, string>());
	}
}