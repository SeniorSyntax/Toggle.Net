using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Toggle.Net.Ioc.Test;

public class RegisterToggledComponentWhenNotEnabled
{
    [Test]
    public void ShouldThrowIfNotToggleRegistrationIsEnabled([Values(false, true)] bool resolveOnce)
    {
        var builder = new ServiceCollection();
        builder.RegisterToggledComponent<Foo1, Foo2, IFoo>("_", resolveOnce);
        builder.AddSingleton<Foo1>();
        builder.AddSingleton<Foo2>();
        using var container = builder.BuildServiceProvider();
        Assert.Throws<ToggledRegistrationIsNotEnabledException>(() =>
        {
            container.GetService<IFoo>();
        });
    }
	
    public interface IFoo;
    public class Foo1 : IFoo;
    public class Foo2 : IFoo;
}