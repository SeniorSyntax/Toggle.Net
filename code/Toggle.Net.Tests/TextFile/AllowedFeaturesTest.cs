using NUnit.Framework;
using SharpTestsEx;
using Toggle.Net.Configuration;
using Toggle.Net.Providers.TextFile;
using Toggle.Net.Tests.Stubs;

namespace Toggle.Net.Tests.TextFile;

public class AllowedFeaturesTest
{
    [Test]
    public void ThrowIfUnknownFeature()
    {
        var content = new[]
        {
            "someflag=true"
        };
        Assert.Throws<IncorrectTextFileException>(() =>
                new ToggleConfiguration(new FileParser(new FileReaderStub(content), new DefaultSpecificationMappings())
                {
                    AllowedFeatures = new[]{"someflag2"}
                }).Create()
            ).ToString()
            .Should().Contain(string.Format(FileParser.NotAllowedFeature, "someflag"));
    }

    [Test]
    public void ShouldAllowFeatureIfExistInCollection()
    {
        var content = new[]
        {
            "someflag1=false"
        };
        new ToggleConfiguration(new FileParser(new FileReaderStub(content), new DefaultSpecificationMappings())
            {
                AllowedFeatures = new[]{"someflag1", "someflag2"}
            }).Create().IsEnabled("someflag1")
            .Should().Be.False();
    }
        
    [Test]
    public void ShouldNotCareAboutCasing()
    {
        var content = new[]
        {
            "someflag=true"
        };
        new ToggleConfiguration(new FileParser(new FileReaderStub(content), new DefaultSpecificationMappings())
            {
                AllowedFeatures = new[]{"SoMeFLag"}
            }).Create().IsEnabled("someflag")
            .Should().Be.True();
    }

    [Test]
    public void ShouldNotCareAboutSpaces()
    {
        var content = new[]
        {
            "someflag=true"
        };
        new ToggleConfiguration(new FileParser(new FileReaderStub(content), new DefaultSpecificationMappings())
            {
                AllowedFeatures = new[]{"                someflag          "}
            }).Create().IsEnabled("someflag")
            .Should().Be.True();
    }
}