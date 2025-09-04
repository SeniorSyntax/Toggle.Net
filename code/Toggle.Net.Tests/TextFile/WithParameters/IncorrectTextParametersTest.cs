using NUnit.Framework;
using SharpTestsEx;
using Toggle.Net.Configuration;
using Toggle.Net.Providers.TextFile;
using Toggle.Net.Tests.Stubs;

namespace Toggle.Net.Tests.TextFile.WithParameters;

public class IncorrectTextParametersTest
{
	[Test]
	public void ShouldThrowIfParameterIsUsedAndOnlyOneDotExist()
	{
		var content = new[]
		{
			"myfeature=ParameterSpecification",
			"myfeature.ParameterSpecification=true"
		};
		var mappings = new DefaultSpecificationMappings();
		mappings.AddMapping("parameterSpecification", new SpecificationWithParameter());
		var fileProvider = new FileParser(new FileReaderStub(content), mappings);

		Assert.Throws<IncorrectTextFileException>(() =>
				new ToggleConfiguration(fileProvider).Create()
			).ToString()
			.Should().Contain(string.Format(FileParser.MustHaveTwoDotsIfParameterUse, 2));
	}

	[Test]
	public void ShouldThrowIfParameterIsUsedAndMoreThanTwoDotsExist()
	{
		var content = new[]
		{
			"myfeature=ParameterSpecification",
			"myfeature.ParameterSpecification.three.four=true"
		};
		var mappings = new DefaultSpecificationMappings();
		mappings.AddMapping("parameterSpecification", new SpecificationWithParameter());
		var fileProvider = new FileParser(new FileReaderStub(content), mappings);

		Assert.Throws<IncorrectTextFileException>(() =>
				new ToggleConfiguration(fileProvider).Create()
			).ToString()
			.Should().Contain(string.Format(FileParser.MustHaveTwoDotsIfParameterUse, 2));
	}

	[Test]
	public void ShouldThrowIfParameterIsDeclaredMoreThanOnce()
	{
		var content = new[]
		{
			"someflag=ParameterSpecification",
			"someflag.ParameterSpecification." + SpecificationWithParameter.ParameterName + "=true",
			"someflag.ParameterSpecification." + SpecificationWithParameter.ParameterName + "=true"
		};
		var mappings = new DefaultSpecificationMappings();
		mappings.AddMapping("parameterSpecification", new SpecificationWithParameter());
		var fileProvider = new FileParser(new FileReaderStub(content), mappings);

		Assert.Throws<IncorrectTextFileException>(() =>
				new ToggleConfiguration(fileProvider).Create()
			).ToString()
			.Should().Contain(string.Format(FileParser.MustOnlyContainSameParameterOnce, SpecificationWithParameter.ParameterName, 3));
	}
}