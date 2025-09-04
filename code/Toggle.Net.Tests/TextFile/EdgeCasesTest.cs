using NUnit.Framework;
using SharpTestsEx;
using Toggle.Net.Configuration;
using Toggle.Net.Providers.TextFile;
using Toggle.Net.Tests.Stubs;

namespace Toggle.Net.Tests.TextFile;

public class EdgeCasesTest
{
	[Test]
	public void ShouldFindSpecificationWithWrongCasing()
	{
		var content = new[] { "someflag=TrUE" };
		var toggleChecker = new ToggleConfiguration(new FileParser(new FileReaderStub(content), new DefaultSpecificationMappings())).Create();
		toggleChecker.IsEnabled("someflag")
			.Should().Be.True();
	}

	[Test]
	public void ShouldFindToggleWithWrongCasing()
	{
		var content = new[] { "SOMEfLAg=true" };
		var toggleChecker = new ToggleConfiguration(new FileParser(new FileReaderStub(content), new DefaultSpecificationMappings())).Create();
		toggleChecker.IsEnabled("someflag")
			.Should().Be.True();
	}

	[Test]
	public void ShouldFindUntrimmedToggle()
	{
		var content = new[] { "   someflag					  =true" };
		var toggleChecker = new ToggleConfiguration(new FileParser(new FileReaderStub(content), new DefaultSpecificationMappings())).Create();
		toggleChecker.IsEnabled("someflag")
			.Should().Be.True();
	}

	[Test]
	public void ShouldFindUntrimmedSpecification()
	{
		var content = new[] { "someflag=         true		  " };
		var toggleChecker = new ToggleConfiguration(new FileParser(new FileReaderStub(content), new DefaultSpecificationMappings())).Create();
		toggleChecker.IsEnabled("someflag")
			.Should().Be.True();
	}
}