using System.Collections.Generic;
using System.Linq;
using Toggle.Net.Specifications;

namespace Toggle.Net.Providers.InCode;

public class CodeConfiguration : IFeatureProvider
{
    private readonly IDictionary<string, Feature> _toggles;
    private bool _throwIfMissing;

    public CodeConfiguration(IDictionary<string, IToggleSpecification> toggleValues)
    {
        _toggles = toggleValues.ToDictionary(x => x.Key, x =>  new Feature(x.Value));
    }
    
    public CodeConfiguration ThrowIfMissing()
    {
        _throwIfMissing = true;
        return this;
    }
    
    public void Init()
    {
    }
    
    public Feature TryGet(string toggleName)
    {
        if (_toggles.TryGetValue(toggleName, out var feature))
            return feature;
        
        return _throwIfMissing ? 
            throw new NotExistingToggleException(toggleName) : 
            null;
    }
}