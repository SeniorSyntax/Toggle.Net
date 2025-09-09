using System.Collections.Generic;

namespace Toggle.Net.Providers;

public class FeatureProvider(IDictionary<string, Feature> features)
{
	public Feature Get(string toggleName)
	{
		return features.TryGetValue(toggleName, out var feature) ?
			feature :
			null;
	} 
}