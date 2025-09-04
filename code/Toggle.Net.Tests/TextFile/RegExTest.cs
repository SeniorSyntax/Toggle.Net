using System.Text.RegularExpressions;
using NUnit.Framework;
using SharpTestsEx;
using Toggle.Net.Configuration;
using Toggle.Net.Providers.TextFile;
using Toggle.Net.Specifications;
using Toggle.Net.Tests.Stubs;

namespace Toggle.Net.Tests.TextFile;

public class RegExTest
{
	[Test]
	public void ShouldHandleMatch()
	{
		const string wordToMatch = "the toggle";

		var content = new[]
		{
			"sometoggle=myRegex",
			"sometoggle.myRegex.pattern=" + wordToMatch
		};
		var mappings = new DefaultSpecificationMappings();
		mappings.AddMapping("myRegex", new RegExSpecification(new Regex("^" + wordToMatch + "$")));

		var toggleChecker = new ToggleConfiguration(new FileParser(new FileReaderStub(content), mappings))
			.Create();

		toggleChecker.IsEnabled("sometoggle")
			.Should().Be.True();
	}

	[Test]
	public void ShouldHandleMiss()
	{
		const string wordToMatch = "the toggle";

		var content = new[]
		{
			"sometoggle=myRegex",
			"sometoggle.myRegex.pattern=" + wordToMatch
		};
		var mappings = new DefaultSpecificationMappings();
		mappings.AddMapping("myRegex", new RegExSpecification(new Regex("^somethingelse$")));

		var toggleChecker = new ToggleConfiguration(new FileParser(new FileReaderStub(content), mappings))
			.Create();

		toggleChecker.IsEnabled("sometoggle")
			.Should().Be.False();
	}

	[Test]
	public void ShouldThrowIfRegExIsMissing()
	{
		const string wordToMatch = "the toggle";

		var content = new[]
		{
			"sometoggle=myRegex"
		};
		var mappings = new DefaultSpecificationMappings();
		mappings.AddMapping("myRegex", new RegExSpecification(new Regex("^" + wordToMatch + "$")));

		Assert.Throws<IncorrectTextFileException>(() => 
				new ToggleConfiguration(new FileParser(new FileReaderStub(content), mappings))
					.Create()).ToString()
			.Should().Contain(string.Format(RegExSpecification.MustDeclareRegexPattern, "sometoggle"));
	}
}