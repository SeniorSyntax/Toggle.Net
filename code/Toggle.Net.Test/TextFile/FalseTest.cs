using NUnit.Framework;
using SharpTestsEx;
using Toggle.Net.Configuration;
using Toggle.Net.Providers.TextFile;
using Toggle.Net.Test.Stubs;

namespace Toggle.Net.Test.TextFile;

public class FalseTest
{
	[Test]
	public void ShouldBeDisabled()
	{
		var content = new[] { "someflag=false" };
		var toggleChecker = new ToggleConfiguration(new FileParser(new FileReaderStub(content), new DefaultSpecificationMappings())).Create();
		toggleChecker.IsEnabled("someflag")
			.Should().Be.False();
	}
}