using NUnit.Framework;
using SharpTestsEx;
using Toggle.Net.Configuration;
using Toggle.Net.Providers.TextFile;
using Toggle.Net.Specifications;
using Toggle.Net.Tests.Stubs;

namespace Toggle.Net.Tests.TextFile;

public class NonExistingFeatureTest
{
	[Test]
	public void ShouldDefaultToFalse()
	{
		var toggleChecker = new ToggleConfiguration(new FileParser(new FileReaderStub(new string[0]), new DefaultSpecificationMappings())).Create();
		toggleChecker.IsEnabled("sometoggle")
			.Should().Be.False();
	}

	[Test]
	public void ShouldBeAbleToChangeDefaultSpecification()
	{
		var toggleChecker = new ToggleConfiguration(new FileParser(new FileReaderStub(new string[0]), new DefaultSpecificationMappings()))
			.SetDefaultSpecification(new BoolSpecification(true))
			.Create();
		toggleChecker.IsEnabled("sometoggle")
			.Should().Be.True();
	}
}