using System.Collections.Generic;
using Toggle.Net.Specifications;

namespace Toggle.Net.Providers.TextFile;

public interface ISpecificationMappings
{
	IDictionary<string, IToggleSpecification> NameSpecificationMappings();
}