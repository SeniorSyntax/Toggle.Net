using NUnit.Framework;
using SharpTestsEx;
using Toggle.Net.Configuration;
using Toggle.Net.Providers.TextFile;
using Toggle.Net.Tests.Stubs;

namespace Toggle.Net.Tests.TextFile.WithParameters;

public class MultipleParametersTest
{
	[Test]
	public void ShouldBeEnabled()
	{
		var content = new[]
		{
			"someflag=ParametersSpecification",
			"someflag.ParametersSpecification." + SpecificationWithMultipleParameters.ParameterName1 + "=true",
			"someflag.ParametersSpecification." + SpecificationWithMultipleParameters.ParameterName2 + "=true",
			"someflag.ParametersSpecification." + SpecificationWithMultipleParameters.ParameterName3 + "=true"
		};
		var mappings = new DefaultSpecificationMappings();
		mappings.AddMapping("parametersSpecification", new SpecificationWithMultipleParameters());
		var fileProvider = new FileParser(new FileReaderStub(content), mappings);
		var toggleChecker = new ToggleConfiguration(fileProvider).Create();
		toggleChecker.IsEnabled("someflag")
			.Should().Be.True();
	}

	[Test]
	public void ShouldBeDisabled()
	{
		var content = new[]
		{
			"someflag=ParametersSpecification",
			"someflag.ParametersSpecification." + SpecificationWithMultipleParameters.ParameterName1 + "=true",
			"someflag.ParametersSpecification." + SpecificationWithMultipleParameters.ParameterName2 + "=false",
			"someflag.ParametersSpecification." + SpecificationWithMultipleParameters.ParameterName3 + "=true"
		};
		var mappings = new DefaultSpecificationMappings();
		mappings.AddMapping("parametersSpecification", new SpecificationWithMultipleParameters());
		var fileProvider = new FileParser(new FileReaderStub(content), mappings);
		var toggleChecker = new ToggleConfiguration(fileProvider).Create();
		toggleChecker.IsEnabled("someflag")
			.Should().Be.False();
	}  
}