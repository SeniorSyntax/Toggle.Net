using NUnit.Framework;
using SharpTestsEx;
using Toggle.Net.Configuration;
using Toggle.Net.Providers.TextFile;
using Toggle.Net.Tests.Stubs;

namespace Toggle.Net.Tests.TextFile;

public class MultipleSpecificationTests
{
	[Test]
	public void AllMustBeEnabledByDefault()
	{
		var content = new[]
		{
			"someflag=false",
			"someflag=true"
		};
		var toggleChecker = new ToggleConfiguration(new FileParser(new FileReaderStub(content), new DefaultSpecificationMappings())).Create();
		toggleChecker.IsEnabled("someflag")
			.Should().Be.False();
	}

	[Test]
	public void ThrowIfMultiple()
	{
		var content = new[]
		{
			"someflag=false",
			"someflag=true"
		};
		Assert.Throws<IncorrectTextFileException>(() =>
				new ToggleConfiguration(new FileParser(new FileReaderStub(content), new DefaultSpecificationMappings()) { ThrowIfFeatureIsDeclaredTwice = true }).Create()
			).ToString()
			.Should().Contain(string.Format(FileParser.MustOnlyBeDeclaredOnce, "someflag", 2));
	}
}