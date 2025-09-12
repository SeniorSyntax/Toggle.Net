using System.Collections.Generic;
using Toggle.Net.Providers;
using Toggle.Net.Specifications;

namespace Toggle.Net.Configuration;

public class ToggleConfiguration(IFeatureProvider featureProvider)
{
	private readonly IList<IFeatureProvider> _featureProviders = new List<IFeatureProvider> {featureProvider};
	private IUserProvider _userProvider;
	private IToggleSpecification _defaultToggleSpecification;
	private ToggleMode _toggleMode = ToggleMode.Customer;

	public ToggleConfiguration AddFeatureProviderFactoryWithHigherPriority(IFeatureProvider featureProvider)
	{
		_featureProviders.Insert(0, featureProvider);
		return this;
	}

	public ToggleConfiguration SetUserProvider(IUserProvider userProvider)
	{
		_userProvider = userProvider;
		return this;
	}

	public ToggleConfiguration SetDefaultSpecification(IToggleSpecification toggleSpecification)
	{
		_defaultToggleSpecification = toggleSpecification;
		return this;
	}

	public ToggleConfiguration SetToggleMode(ToggleMode toggleMode)
	{
		_toggleMode = toggleMode;
		return this;
	}

	public IToggleChecker Create()
	{
		_userProvider ??= new nullUserProvider();
		_defaultToggleSpecification ??= new DevSpecification();

		foreach (var provider in _featureProviders)
		{
			provider.Init();
		}
		return new ToggleChecker(_featureProviders, _defaultToggleSpecification, _userProvider, _toggleMode);
	}
	
	private class nullUserProvider : IUserProvider
	{
		public string CurrentUser() => 
			string.Empty;
	}
}