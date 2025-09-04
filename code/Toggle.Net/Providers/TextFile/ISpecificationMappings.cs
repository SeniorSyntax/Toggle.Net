using Toggle.Net.Specifications;

namespace Toggle.Net.Providers.TextFile;

public interface ISpecificationMappings
{
	IToggleSpecification GetSpecification(string name);
}