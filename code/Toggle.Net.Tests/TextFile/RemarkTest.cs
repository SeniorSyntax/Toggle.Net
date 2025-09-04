using NUnit.Framework;
using SharpTestsEx;
using Toggle.Net.Configuration;
using Toggle.Net.Providers.TextFile;
using Toggle.Net.Tests.Stubs;

namespace Toggle.Net.Tests.TextFile;

public class RemarkTest
{
	[Test]
	public void ShouldBeAbleToWriteRemarks()
	{
		var content = new[]
		{
			"#this should not throw",
			"someflag=true",
			" # and neither should this"
		};
		var toggleChecker = new ToggleConfiguration(new FileParser(new FileReaderStub(content), new DefaultSpecificationMappings())).Create();
		toggleChecker.IsEnabled("someflag")
			.Should().Be.True();
	}

	[Test]
	public void ShouldAllowBlankLines()
	{
		var content = new[]
		{
			"				 ",
			"someflag=true",
			"",
			string.Empty
		};
		var toggleChecker = new ToggleConfiguration(new FileParser(new FileReaderStub(content), new DefaultSpecificationMappings())).Create();
		toggleChecker.IsEnabled("someflag")
			.Should().Be.True();
	}

	[Test]
	public void ShouldAllowCommentingInsideARow()
	{
		var content = new[]
		{
			"someflag=true #this should be possible"
		};
		var toggleChecker = new ToggleConfiguration(new FileParser(new FileReaderStub(content), new DefaultSpecificationMappings())).Create();
		toggleChecker.IsEnabled("someflag")
			.Should().Be.True();
	}
}