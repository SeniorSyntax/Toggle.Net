using NUnit.Framework;
using SharpTestsEx;
using Toggle.Net.Configuration;
using Toggle.Net.Providers.TextFile;
using Toggle.Net.Test.Stubs;

namespace Toggle.Net.Test.TextFile.WithParameters;

public class EdgeCasesTest
{
	[NUnit.Framework.Test]
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

	[NUnit.Framework.Test]
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

	[NUnit.Framework.Test]
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