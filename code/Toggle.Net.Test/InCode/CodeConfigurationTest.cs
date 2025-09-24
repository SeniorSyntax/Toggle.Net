using System.Collections.Generic;
using NUnit.Framework;
using SharpTestsEx;
using Toggle.Net.Configuration;
using Toggle.Net.Providers.InCode;
using Toggle.Net.Specifications;

namespace Toggle.Net.Test.InCode;

public class CodeConfigurationTest
{
    [Test]
    public void ShouldEnableToggle()
    {
        var toggleValues = new Dictionary<string, IToggleSpecification> { { "toggle", new BoolSpecification(true) } };
        new ToggleConfiguration(new CodeConfiguration(toggleValues))
            .Create().IsEnabled("toggle")
            .Should().Be.True();
    }
    
    [Test]
    public void ShouldDisableToggle()
    {
        var toggleValues = new Dictionary<string, IToggleSpecification> { { "toggle", new BoolSpecification(false) } };
        new ToggleConfiguration(new CodeConfiguration(toggleValues))
            .Create().IsEnabled("toggle")
            .Should().Be.False();
    }

    [TestCase(ToggleMode.Development, ExpectedResult = true)]
    [TestCase(ToggleMode.Rc, ExpectedResult = false)]
    public bool ShouldDefaultToDev(ToggleMode toggleMode)
    {
        return new ToggleConfiguration(new CodeConfiguration(new Dictionary<string, IToggleSpecification>()))
            .SetToggleMode(toggleMode)
            .Create().IsEnabled("toggle");
    }
}