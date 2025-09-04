using System;
using System.Collections.Generic;
using Toggle.Net.Specifications;

namespace Toggle.Net.Providers.TextFile;

public class DefaultSpecificationMappings : ISpecificationMappings
{
	private readonly IDictionary<string, IToggleSpecification> _mappings = 
		new Dictionary<string, IToggleSpecification>(StringComparer.OrdinalIgnoreCase)
	{
		["true"] = new BoolSpecification(true),
		["false"] = new BoolSpecification(false),
		["user"] = new UserSpecification(),
		["random"] = new RandomSpecification()
	};

	public void AddMapping(string specificationName, IToggleSpecification specification) => 
		_mappings.Add(specificationName, specification);

	public IToggleSpecification GetSpecification(string name) =>
		_mappings.TryGetValue(name, out var specification) ? 
			specification : 
			null;
}