using System.Collections.Generic;
using System.Linq;
using Toggle.Net.Internal;
using Toggle.Net.Providers;
using Toggle.Net.Specifications;

namespace Toggle.Net.Configuration;

public class ToggleConfiguration
{
	private readonly IList<IFeatureProviderFactory> _featureProviderFactories;
	private IUserProvider _userProvider;
	private IToggleSpecification _defaultToggleSpecification;

	public ToggleConfiguration(IFeatureProviderFactory featureProviderFactory)
	{
		_featureProviderFactories = new List<IFeatureProviderFactory> {featureProviderFactory};
	}
		
	public ToggleConfiguration AddFeatureProviderFactoryWithHigherPriority(IFeatureProviderFactory featureProviderFactory)
	{
		_featureProviderFactories.Insert(0, featureProviderFactory);
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

	public IToggleChecker Create()
	{
		if (_userProvider == null)
			_userProvider = new NullUserProvider();
		if(_defaultToggleSpecification==null)
			_defaultToggleSpecification = new BoolSpecification(false);
			
		var featureProviders = _featureProviderFactories.Select(factory => factory.Create()).ToArray();
		return new ToggleChecker(featureProviders, _defaultToggleSpecification, _userProvider);
	}
}