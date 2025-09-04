using NUnit.Framework;
using SharpTestsEx;
using Toggle.Net.Configuration;
using Toggle.Net.Providers.TextFile;
using Toggle.Net.Tests.Stubs;

namespace Toggle.Net.Tests.TextFile.WithParameters;

public class MultipleFeaturesUsesSameSpecificationTypeTest
{
	[Test]
	public void ShouldBeEnabled()
	{
		var content = new[]
		{
			"trueFlag=ParameterSpecification",
			"trueFlag.ParameterSpecification." + SpecificationWithParameter.ParameterName + "=true",
			"falseFlag=ParameterSpecification",
			"falseFlag.ParameterSpecification." + SpecificationWithParameter.ParameterName + "=false"
		};
		var mappings = new DefaultSpecificationMappings();
		mappings.AddMapping("parameterspecification", new SpecificationWithParameter());
		var fileProvider = new FileParser(new FileReaderStub(content), mappings);
		var toggleChecker = new ToggleConfiguration(fileProvider).Create();
		toggleChecker.IsEnabled("trueFlag")
			.Should().Be.True();
	}

	[Test]
	public void ShouldBeDisabled()
	{
		var content = new[]
		{
			"trueFlag=ParameterSpecification",
			"trueFlag.ParameterSpecification." + SpecificationWithParameter.ParameterName + "=true",
			"falseFlag=ParameterSpecification",
			"falseFlag.ParameterSpecification." + SpecificationWithParameter.ParameterName + "=false"
		};
		var mappings = new DefaultSpecificationMappings();
		mappings.AddMapping("parameterspecification", new SpecificationWithParameter());
		var fileProvider = new FileParser(new FileReaderStub(content), mappings);
		var toggleChecker = new ToggleConfiguration(fileProvider).Create();
		toggleChecker.IsEnabled("falseFlag")
			.Should().Be.False();
	} 
}