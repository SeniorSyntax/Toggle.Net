using System.Collections.Generic;
using Toggle.Net.Internal;

namespace Toggle.Net.Providers;

public class StaticFeatureProvider : IFeatureProvider
{
	private readonly IDictionary<string, Feature> _features;

	public StaticFeatureProvider(IDictionary<string, Feature> features)
	{
		_features = features;
	}

	public Feature Get(string toggleName)
	{
		return _features.TryGetValue(toggleName, out var feature) ?
			feature :
			null;
	} 
}