using System.Collections.Generic;

namespace Toggle.Net.Providers;

public class StaticFeatureProvider(IDictionary<string, Feature> features) : IFeatureProvider
{
	public Feature Get(string toggleName)
	{
		return features.TryGetValue(toggleName, out var feature) ?
			feature :
			null;
	} 
}