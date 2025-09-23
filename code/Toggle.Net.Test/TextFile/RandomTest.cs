using NUnit.Framework;
using SharpTestsEx;
using Toggle.Net.Configuration;
using Toggle.Net.Providers.TextFile;
using Toggle.Net.Specifications;
using Toggle.Net.Test.Stubs;

namespace Toggle.Net.Test.TextFile;

public class RandomTest
{
	[NUnit.Framework.Test]
	public void ShouldAlwaysBeEnabledIf100Percent()
	{
		var content = new[]
		{
			"someflag=random",
			"someflag.random.percent=100"
		};
		var toggleChecker = new ToggleConfiguration(new FileParser(new FileReaderStub(content), new DefaultSpecificationMappings()))
			.SetUserProvider(new UserProviderStub("something"))
			.Create();

		toggleChecker.IsEnabled("someflag")
			.Should().Be.True();
	}

	[NUnit.Framework.Test]
	public void ShouldAlwaysBeDisabledIf0Percent()
	{
		var content = new[]
		{
			"someflag=random",
			"someflag.random.percent=0"
		};
		var toggleChecker = new ToggleConfiguration(new FileParser(new FileReaderStub(content), new DefaultSpecificationMappings()))
			.SetUserProvider(new UserProviderStub("something"))
			.Create();

		toggleChecker.IsEnabled("someflag")
			.Should().Be.False();
	}

	[NUnit.Framework.Test]
	public void ShouldRandomize()
	{
		var content = new[]
		{
			"someflag=random",
			"someflag.random.percent=50"
		};
		var toggleChecker = new ToggleConfiguration(new FileParser(new FileReaderStub(content), new DefaultSpecificationMappings()))
			.SetUserProvider(new UserProviderRandom())
			.Create();

		const int numberOfQueries = 10000;
		var numberOfEnabled = 0;

		for (var x = 0; x < numberOfQueries; x++)
		{
			if (toggleChecker.IsEnabled("someflag"))
				numberOfEnabled++;
		}

		numberOfEnabled.Should().Be.IncludedIn(3000, 7000);
	}

	[NUnit.Framework.Test]
	public void ShouldReturnSameValueForOneSpecificUser()
	{
		var content = new[]
		{
			"someflag=random",
			"someflag.random.percent=50"
		};
		var toggleChecker = new ToggleConfiguration(new FileParser(new FileReaderStub(content), new DefaultSpecificationMappings()))
			.SetUserProvider(new UserProviderStub("something"))
			.Create();

		var firstResult = toggleChecker.IsEnabled("someflag");

		toggleChecker.IsEnabled("someflag").Should().Be.EqualTo(firstResult);
		toggleChecker.IsEnabled("someflag").Should().Be.EqualTo(firstResult);
		toggleChecker.IsEnabled("someflag").Should().Be.EqualTo(firstResult);
		toggleChecker.IsEnabled("someflag").Should().Be.EqualTo(firstResult);
	}

	[NUnit.Framework.Test]
	public void ShouldOnlyAcceptInts()
	{
		var content = new[]
		{
			"someflag=random",
			"someflag.random.percent=50%"
		};

		Assert.Throws<IncorrectTextFileException>(() =>
				new ToggleConfiguration(new FileParser(new FileReaderStub(content), new DefaultSpecificationMappings())).Create()
			).ToString()
			.Should().Contain(string.Format(RandomSpecification.MustDeclaredPercentAsInt, "someflag"));
	}

	[NUnit.Framework.Test]
	public void ShouldThrowIfMissingPercent()
	{
		var content = new[]
		{
			"someflag=random"
		};

		Assert.Throws<IncorrectTextFileException>(() =>
				new ToggleConfiguration(new FileParser(new FileReaderStub(content), new DefaultSpecificationMappings())).Create()
			).ToString()
			.Should().Contain(string.Format(RandomSpecification.MustHaveDeclaredPercent, "someflag"));
	}


	[NUnit.Framework.Test]
	public void ShouldThrowIfOutOfRange([Values("-1000", "-1", "101", "1000")] string percent)
	{
		var content = new[]
		{
			"someflag=random",
			"someflag.random.percent=" + percent
		};

		Assert.Throws<IncorrectTextFileException>(() =>
				new ToggleConfiguration(new FileParser(new FileReaderStub(content), new DefaultSpecificationMappings())).Create()
			).ToString()
			.Should().Contain(string.Format(RandomSpecification.MustBeBetween0And100, "someflag"));
	}
}