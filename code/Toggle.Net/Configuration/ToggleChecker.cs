using System.Collections.Generic;
using Toggle.Net.Providers;
using Toggle.Net.Specifications;

namespace Toggle.Net.Configuration;

internal class ToggleChecker : IToggleChecker
{
	private readonly IEnumerable<IFeatureProvider> _featureProviders;
	private readonly IToggleSpecification _defaultToggleSpecification;
	private readonly IUserProvider _userProvider;
	private readonly ToggleMode _toggleMode;

	internal ToggleChecker(IEnumerable<IFeatureProvider> featureProviders, 
		IToggleSpecification defaultToggleSpecification, 
		IUserProvider userProvider,
		ToggleMode toggleMode)
	{
		_featureProviders = featureProviders;
		_defaultToggleSpecification = defaultToggleSpecification;
		_userProvider = userProvider;
		_toggleMode = toggleMode;
	}

	public bool IsEnabled(string toggleName)
	{
		var currentUser = _userProvider.CurrentUser();
		foreach (var featureProvider in _featureProviders)
		{
			var feature = featureProvider.TryGet(toggleName);
			if (feature != null)
			{
				return feature.IsEnabled(_toggleMode, currentUser);
			}
		}
		return _defaultToggleSpecification.IsEnabled(_toggleMode, currentUser, new Dictionary<string, string>());
	}
}