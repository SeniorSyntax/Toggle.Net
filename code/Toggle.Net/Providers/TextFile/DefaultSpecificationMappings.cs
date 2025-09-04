using System;
using System.Collections.Generic;
using System.Linq;
using Toggle.Net.Specifications;

namespace Toggle.Net.Providers.TextFile;

public class DefaultSpecificationMappings : ISpecificationMappings
{
	private readonly IDictionary<string, IToggleSpecification> _mappings;

	public DefaultSpecificationMappings()
	{
		_mappings = new Dictionary<string, IToggleSpecification>(StringComparer.OrdinalIgnoreCase)
		{
			["true"] = new BoolSpecification(true),
			["false"] = new BoolSpecification(false),
			["user"] = new UserSpecification(),
			["random"] = new RandomSpecification()
		};
	}

	public IDictionary<string, IToggleSpecification> NameSpecificationMappings()
	{
		return _mappings.ToDictionary(x => x.Key, x => x.Value, StringComparer.OrdinalIgnoreCase);
	}

	public void AddMapping(string specificationName, IToggleSpecification specification)
	{
		_mappings.Add(specificationName, specification);
	}
}