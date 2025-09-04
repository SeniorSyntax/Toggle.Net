using NUnit.Framework;
using SharpTestsEx;
using Toggle.Net.Configuration;
using Toggle.Net.Providers.TextFile;
using Toggle.Net.Specifications;
using Toggle.Net.Tests.Stubs;

namespace Toggle.Net.Tests.TextFile;

public class UserTest
{
	[Test]
	public void ShouldBeEnabled()
	{
		var content = new[]
		{
			"someflag=user",
			"someflag.user.ids=10"
		};
		var toggleChecker = new ToggleConfiguration(new FileParser(new FileReaderStub(content), new DefaultSpecificationMappings()))
			.SetUserProvider(new UserProviderStub("10"))
			.Create();

		toggleChecker.IsEnabled("someflag")
			.Should().Be.True();
	}

	[Test]
	public void ShouldBeDisabled()
	{
		var content = new[]
		{
			"someflag=user",
			"someflag.user.ids=nope"
		};
		var toggleChecker = new ToggleConfiguration(new FileParser(new FileReaderStub(content), new DefaultSpecificationMappings()))
			.SetUserProvider(new UserProviderStub("10"))
			.Create();

		toggleChecker.IsEnabled("someflag")
			.Should().Be.False();
	}

	[Test]
	public void ShouldBeEnabledIfExistsInParameterList()
	{
		var content = new[]
		{
			"someflag=user",
			"someflag.user.ids=1,2,3,4"
		};
		var toggleChecker = new ToggleConfiguration(new FileParser(new FileReaderStub(content), new DefaultSpecificationMappings()))
			.SetUserProvider(new UserProviderStub("2"))
			.Create();

		toggleChecker.IsEnabled("someflag")
			.Should().Be.True();
	}


	[Test]
	public void ShouldBeEnabledIfExistsInParameterListWithSpaces()
	{
		var content = new[]
		{
			"someflag=user",
			"someflag.user.ids =   1, 2	,  3 ,4  "
		};
		var toggleChecker = new ToggleConfiguration(new FileParser(new FileReaderStub(content), new DefaultSpecificationMappings()))
			.SetUserProvider(new UserProviderStub("2"))
			.Create();

		toggleChecker.IsEnabled("someflag")
			.Should().Be.True();
	}

	[Test]
	public void ShouldTreatIdsAsOneIfCurrentUserContainsComma_Disabled()
	{
		var content = new[]
		{
			"someflag=user",
			"someflag.user.ids=1,2,3,4"
		};
		var toggleChecker = new ToggleConfiguration(new FileParser(new FileReaderStub(content), new DefaultSpecificationMappings()))
			.SetUserProvider(new UserProviderStub("2,"))
			.Create();

		toggleChecker.IsEnabled("someflag")
			.Should().Be.False();
	}


	[Test]
	public void ShouldTreatIdsAsOneIfCurrentUserContainsComma_Enabled()
	{
		var content = new[]
		{
			"someflag=user",
			"someflag.user.ids=  1,2,3,4  "
		};
		var toggleChecker = new ToggleConfiguration(new FileParser(new FileReaderStub(content), new DefaultSpecificationMappings()))
			.SetUserProvider(new UserProviderStub("1,2,3,4"))
			.Create();

		toggleChecker.IsEnabled("someflag")
			.Should().Be.True();
	}

	[Test]
	public void ShouldNotBeCaseSensitive()
	{
		var content = new[]
		{
			"someflag=user",
			"someflag.user.ids=roger"
		};
		var toggleChecker = new ToggleConfiguration(new FileParser(new FileReaderStub(content), new DefaultSpecificationMappings()))
			.SetUserProvider(new UserProviderStub("ROGER"))
			.Create();

		toggleChecker.IsEnabled("someflag")
			.Should().Be.True();
	}

	[Test]
	public void ShouldNotBeCaseSensitiveWhenCurrentUserContainsComma()
	{
		var content = new[]
		{
			"someflag=user",
			"someflag.user.ids=roger,"
		};
		var toggleChecker = new ToggleConfiguration(new FileParser(new FileReaderStub(content), new DefaultSpecificationMappings()))
			.SetUserProvider(new UserProviderStub("ROGER,"))
			.Create();

		toggleChecker.IsEnabled("someflag")
			.Should().Be.True();
	}

	[Test]
	public void ShouldThrowIfMissingIdsValue()
	{
		var content = new[]
		{
			"someflag=user"
		};

		Assert.Throws<IncorrectTextFileException>(() =>
				new ToggleConfiguration(new FileParser(new FileReaderStub(content), new DefaultSpecificationMappings())).Create()
			).ToString()
			.Should().Contain(string.Format(UserSpecification.MustHaveDeclaredIds, "someflag"));
	}
}