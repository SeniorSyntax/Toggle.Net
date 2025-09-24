using NUnit.Framework;
using SharpTestsEx;
using Toggle.Net.Configuration;
using Toggle.Net.Providers.TextFile;
using Toggle.Net.Test.Stubs;

namespace Toggle.Net.Test.TextFile;


public class CustomerTest : ToggleModeTest
{
	protected override bool UndefinedFeatureShouldBe => false;
	protected override bool RcFeatureShouldBe => false;
	protected override ToggleMode? Mode => ToggleMode.Customer;
}

public class DeveloperTest : ToggleModeTest
{
	protected override bool UndefinedFeatureShouldBe => true;
	protected override bool RcFeatureShouldBe => true;
	protected override ToggleMode? Mode => ToggleMode.Development;
}

public class RcTest : ToggleModeTest
{
	protected override bool UndefinedFeatureShouldBe => false;
	protected override bool RcFeatureShouldBe => true;
	protected override ToggleMode? Mode => ToggleMode.Rc;
}

public class EmptyToggleModeTest : CustomerTest
{
	protected override ToggleMode? Mode => null;
}

public abstract class ToggleModeTest
{
    [Test]
	public void DisabledFeatureInFile()
	{
		var content = new[] { "someflag=false" };
		var toggleChecker = createToggleChecker(content);
		toggleChecker.IsEnabled("someflag")
			.Should().Be.False();
	}

	[Test]
	public void EnabledFeatureInFile()
	{
		var content = new[] { "someflag=true" };
		var toggleChecker = createToggleChecker(content);
		toggleChecker.IsEnabled("someflag")
			.Should().Be.True();
	}

	[Test]
	public void UndefinedFeatureInFile()
	{
		string[] content = [];
		var toggleChecker = createToggleChecker(content);
		toggleChecker.IsEnabled("someflag")
			.Should().Be.EqualTo(UndefinedFeatureShouldBe);
	}

	[Test]
	public void RcFeatureInFile()
	{
		var content = new[] { "someflag=rc" };
		var toggleChecker = createToggleChecker(content);
		toggleChecker.IsEnabled("someflag")
			.Should().Be.EqualTo(RcFeatureShouldBe);
	}

	[Test]
	public void DevFeatureInFile()
	{
		var content = new[] { "someflag=dev" };
		var toggleChecker = createToggleChecker(content);
		toggleChecker.IsEnabled("someflag")
			.Should().Be.EqualTo(devFeatureShouldBe);
	}

	private IToggleChecker createToggleChecker(string[] content)
	{
		var config = new ToggleConfiguration(new FileParser(new FileReaderStub(content), new DefaultSpecificationMappings()));
		if (Mode.HasValue)
			config.SetToggleMode(Mode.Value);
		return config.Create();
	}

	protected abstract bool UndefinedFeatureShouldBe { get; }
	protected abstract bool RcFeatureShouldBe { get; }

	private bool devFeatureShouldBe => UndefinedFeatureShouldBe;

	protected abstract ToggleMode? Mode { get; }
}