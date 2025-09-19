using System;
using Autofac;
using NUnit.Framework;

namespace Toggle.Net.Autofac.Test;

public class RegisterToggledComponentWhenNotEnabled
{
    [Test]
    public void ShouldThrowIfNotEnabled([Values(false, true)] bool resolveOnce)
    {
        var builder = new ContainerBuilder();
        builder.RegisterToggledComponent<Foo1, Foo2, IFoo>("_", resolveOnce);
        builder.RegisterType<Foo1>();
        builder.RegisterType<Foo2>();
        using var container = builder.Build();
        var ex = Assert.Catch<Exception>(() =>
        {
            container.Resolve<IFoo>();
        });
        Assert.That(ex.InnerException, Is.TypeOf<ToggledRegistrationIsNotEnabledException>());
    }
	
    public interface IFoo;
    public class Foo1 : IFoo;
    public class Foo2 : IFoo;
}