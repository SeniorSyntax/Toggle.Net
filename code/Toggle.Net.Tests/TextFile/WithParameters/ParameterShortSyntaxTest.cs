using NUnit.Framework;
using SharpTestsEx;
using Toggle.Net.Configuration;
using Toggle.Net.Providers.TextFile;
using Toggle.Net.Tests.Stubs;

namespace Toggle.Net.Tests.TextFile.WithParameters;

public class ParameterShortSyntaxTest
{
	[Test]
	public void ShouldBeAbleToRunSingleParameterSpecificationUsingOneLine()
	{
		var content = new[]
		{
			"someflag.ParameterSpecification." + SpecificationWithParameter.ParameterName + "=true"
		};
		var mappings = new DefaultSpecificationMappings();
		mappings.AddMapping("parameterspecification", new SpecificationWithParameter());
		var fileProvider = new FileParser(new FileReaderStub(content), mappings);
		var toggleChecker = new ToggleConfiguration(fileProvider).Create();

		toggleChecker.IsEnabled("someflag")
			.Should().Be.True();
	}

	[Test]
	public void ShouldContainValidSpecificationUsingShortSyntax()
	{
		var content = new[] { "someflag.nope.nope=true" };
		Assert.Throws<IncorrectTextFileException>(() =>
				new ToggleConfiguration(new FileParser(new FileReaderStub(content), new DefaultSpecificationMappings())).Create()
			).ToString()
			.Should().Contain(string.Format(FileParser.MustHaveValidSpecification, "nope", 1));
	}
}