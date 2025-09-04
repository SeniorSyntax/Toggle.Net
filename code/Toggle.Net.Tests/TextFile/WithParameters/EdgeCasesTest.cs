using NUnit.Framework;
using SharpTestsEx;
using Toggle.Net.Configuration;
using Toggle.Net.Providers.TextFile;
using Toggle.Net.Tests.Stubs;

namespace Toggle.Net.Tests.TextFile.WithParameters;

public class EdgeCasesTest
{
	[Test]
	public void ShouldFindParameterWithWrongCasing()
	{
		var content = new[]
		{
			"someflag=ParameterSpecification",
			"someflag.ParameterSpecification." + SpecificationWithParameter.ParameterName.ToUpper() + "=true"
		};
		var mappings = new DefaultSpecificationMappings();
		mappings.AddMapping("parameterspecification", new SpecificationWithParameter());
		var fileProvider = new FileParser(new FileReaderStub(content), mappings);
		var toggleChecker = new ToggleConfiguration(fileProvider).Create();

		toggleChecker.IsEnabled("someflag")
			.Should().Be.True();
	}

	[Test]
	public void ShouldTrimAfterParameter()
	{
		var content = new[]
		{
			"someflag=ParameterSpecification",
			"someflag.ParameterSpecification.		" + SpecificationWithParameter.ParameterName.ToUpper() + "=true"
		};
		var mappings = new DefaultSpecificationMappings();
		mappings.AddMapping("parameterspecification", new SpecificationWithParameter());
		var fileProvider = new FileParser(new FileReaderStub(content), mappings);
		var toggleChecker = new ToggleConfiguration(fileProvider).Create();

		toggleChecker.IsEnabled("someflag")
			.Should().Be.True();
	}

	[Test]
	public void ShouldTrimBeforeFeatureName()
	{
		var content = new[]
		{
			"				someflag=ParameterSpecification",
			"			someflag.ParameterSpecification." + SpecificationWithParameter.ParameterName.ToUpper() + "=true"
		};
		var mappings = new DefaultSpecificationMappings();
		mappings.AddMapping("parameterspecification", new SpecificationWithParameter());
		var fileProvider = new FileParser(new FileReaderStub(content), mappings);
		var toggleChecker = new ToggleConfiguration(fileProvider).Create();

		toggleChecker.IsEnabled("someflag")
			.Should().Be.True();
	}
}