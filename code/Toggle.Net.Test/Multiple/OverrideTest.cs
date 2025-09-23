using NUnit.Framework;
using SharpTestsEx;
using Toggle.Net.Configuration;
using Toggle.Net.Providers.TextFile;
using Toggle.Net.Specifications;
using Toggle.Net.Test.Stubs;

namespace Toggle.Net.Test.Multiple;

public class OverrideTest
{
    [NUnit.Framework.Test]
    public void ShouldUseHighestPrio()
    {
        var contentLowPrio = new[]
        {
            "someflag=false"
        };
        var contentHighPrio = new[]
        {
            "someflag=true"
        };
        new ToggleConfiguration(new FileParser(new FileReaderStub(contentLowPrio), new DefaultSpecificationMappings()))
            .AddFeatureProviderWithHigherPriority(new FileParser(new FileReaderStub(contentHighPrio), new DefaultSpecificationMappings()))
            .Create().IsEnabled("someflag")
            .Should().Be.True();
    }
        
    [NUnit.Framework.Test]
    public void ShouldUseLowPrio()
    {
        var contentLowPrio = new[]
        {
            "someflag=true"
        };
        var contentHighPrio = new[]
        {
            "someflag2=false"
        };
        new ToggleConfiguration(new FileParser(new FileReaderStub(contentLowPrio), new DefaultSpecificationMappings()))
            .AddFeatureProviderWithHigherPriority(new FileParser(new FileReaderStub(contentHighPrio), new DefaultSpecificationMappings()))
            .Create().IsEnabled("someflag")
            .Should().Be.True();
    }
        
    [NUnit.Framework.Test]
    public void ShouldUseDefaultValue()
    {
        var contentLowPrio = new[]
        {
            "someflag2=true"
        };
        var contentHighPrio = new[]
        {
            "someflag2=false"
        };
        new ToggleConfiguration(new FileParser(new FileReaderStub(contentLowPrio), new DefaultSpecificationMappings()))
            .AddFeatureProviderWithHigherPriority(new FileParser(new FileReaderStub(contentHighPrio), new DefaultSpecificationMappings()))
            .SetDefaultSpecification(new BoolSpecification(true))
            .Create().IsEnabled("someflag")
            .Should().Be.True();
    }
}