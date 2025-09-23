using NUnit.Framework;
using SharpTestsEx;
using Toggle.Net.Configuration;
using Toggle.Net.Providers.TextFile;
using Toggle.Net.Test.Stubs;

namespace Toggle.Net.Test.TextFile;

public class AllowedFeaturesTest
{
    [NUnit.Framework.Test]
    public void ThrowIfUnknownFeature()
    {
        var content = new[]
        {
            "someflag=true"
        };
        Assert.Throws<IncorrectTextFileException>(() =>
                new ToggleConfiguration(new FileParser(new FileReaderStub(content), new DefaultSpecificationMappings())
                    .SetAllowedFeatures(["someflag2"]))
                    .Create()
            ).ToString()
            .Should().Contain(string.Format(FileParser.NotAllowedFeature, "someflag"));
    }

    [NUnit.Framework.Test]
    public void ShouldAllowFeatureIfExistInCollection()
    {
        var content = new[]
        {
            "someflag1=false"
        };
        new ToggleConfiguration(new FileParser(new FileReaderStub(content), new DefaultSpecificationMappings())
            .SetAllowedFeatures(["someflag1", "someflag2"]))
            .Create().IsEnabled("someflag1")
            .Should().Be.False();
    }
        
    [NUnit.Framework.Test]
    public void ShouldNotCareAboutCasing()
    {
        var content = new[]
        {
            "someflag=true"
        };
        new ToggleConfiguration(new FileParser(new FileReaderStub(content), new DefaultSpecificationMappings())
            .SetAllowedFeatures(["SoMeFLag"]))
            .Create().IsEnabled("someflag")
            .Should().Be.True();
    }

    [NUnit.Framework.Test]
    public void ShouldNotCareAboutSpaces()
    {
        var content = new[]
        {
            "someflag=true"
        };
        new ToggleConfiguration(new FileParser(new FileReaderStub(content), new DefaultSpecificationMappings())
            .SetAllowedFeatures(["                someflag          "]))
            .Create().IsEnabled("someflag")
            .Should().Be.True();
    }
}