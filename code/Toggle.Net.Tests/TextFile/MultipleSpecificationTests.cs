using NUnit.Framework;
using SharpTestsEx;
using Toggle.Net.Configuration;
using Toggle.Net.Providers.TextFile;
using Toggle.Net.Tests.Stubs;

namespace Toggle.Net.Tests.TextFile;

public class MultipleSpecificationTests
{
	[Test]
	public void AllMustBeEnabledIfAllowingMultipleFeatureDeclarations()
	{
		var content = new[]
		{
			"someflag=false",
			"someflag=true"
		};
		var toggleChecker = new ToggleConfiguration(new FileParser(new FileReaderStub(content), new DefaultSpecificationMappings()).AllowMultipleFeatureDeclarations()).Create();
		toggleChecker.IsEnabled("someflag")
			.Should().Be.False();
	}

	[Test]
	public void ThrowByDefaultIfMultiple()
	{
		var content = new[]
		{
			"someflag=false",
			"someflag=true"
		};
		Assert.Throws<IncorrectTextFileException>(() =>
				new ToggleConfiguration(new FileParser(new FileReaderStub(content), new DefaultSpecificationMappings())).Create()
			).ToString()
			.Should().Contain(string.Format(FileParser.MustOnlyBeDeclaredOnce, "someflag", 2));
	}
}