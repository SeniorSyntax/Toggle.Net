using NUnit.Framework;
using SharpTestsEx;
using Toggle.Net.Configuration;
using Toggle.Net.Providers.TextFile;
using Toggle.Net.Tests.Stubs;

namespace Toggle.Net.Tests.TextFile;

public class IncorrectTextTest
{
	[Test]
	public void ShouldContainEqualSign()
	{
		var content = new[] { "someflag2" };
		Assert.Throws<IncorrectTextFileException>(() =>
				new ToggleConfiguration(new FileParser(new FileReaderStub(content), new DefaultSpecificationMappings())).Create()
			).ToString()
			.Should().Contain(string.Format(FileParser.MustContainEqualSign, 1));
	}

	[Test]
	public void ShouldNotContainMoreThanOneEqualSign()
	{
		var content = new[] { "someflag=true=true" };
		Assert.Throws<IncorrectTextFileException>(() =>
				new ToggleConfiguration(new FileParser(new FileReaderStub(content), new DefaultSpecificationMappings())).Create()
			).ToString()
			.Should().Contain(string.Format(FileParser.MustOnlyContainOneEqualSign, 1));
	}

	[Test]
	public void ShouldReturnAllExceptions()
	{
		var content = new[]
		{
			"missingEqual",
			"multipleEqual=false=true"
		};
		var ex = Assert.Throws<IncorrectTextFileException>(() =>
			new ToggleConfiguration(new FileParser(new FileReaderStub(content), new DefaultSpecificationMappings())).Create()
		).ToString();
		ex.Should().Contain(string.Format(FileParser.MustContainEqualSign, 1));
		ex.Should().Contain(string.Format(FileParser.MustOnlyContainOneEqualSign, 2));
	}

	[Test]
	public void ShouldContainValidSpecification()
	{
		var content = new[] { "someflag=maybe" };
		Assert.Throws<IncorrectTextFileException>(() =>
				new ToggleConfiguration(new FileParser(new FileReaderStub(content), new DefaultSpecificationMappings())).Create()
			).ToString()
			.Should().Contain(string.Format(FileParser.MustHaveValidSpecification, "maybe", 1));
	}
}