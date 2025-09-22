using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Builder;
using NUnit.Framework;
using SharpTestsEx;

namespace Toggle.Net.Autofac.Test;

public class ToggleState
{
	private readonly Dictionary<string, bool> _toggleStates = new();
	
	public bool IsEnabled(string toggleName) => _toggleStates[toggleName];

	public void Set(string theToggle, bool toggleState) => 
		_toggleStates[theToggle] = toggleState;
}

[TestFixture(true, true)]
[TestFixture(true, false)]
[TestFixture(false, false)]
[TestFixture(false, true)]
public class RegisterToggledComponentTest(bool toggleState, bool resolveOnce)
{
	private readonly string _theToggle = "theToggle";
	private IMyService _myService;
	private IContainer _container;
	private ToggleState _toggleState;

	private class enableToggledRegistrations(IComponentContext componentContext) : IToggledRegistrationPicker
	{
		public object PickService<TOn, TOff>(string toggleName)
		{
			if(componentContext.Resolve<ToggleState>().IsEnabled(toggleName))
				return componentContext.Resolve<MyServiceOn>();
			return componentContext.Resolve<MyServiceOff>();
		}
	}
	
	[SetUp]
	public void BeforeTest()
	{
		var builder = new ContainerBuilder();
		builder.RegisterType<MyServiceOn>();
		builder.RegisterType<MyServiceOff>();
		builder.RegisterToggledComponent<MyServiceOn, MyServiceOff, IMyService>(_theToggle, resolveOnce);
		builder.EnableToggledRegistrations<enableToggledRegistrations>();
		builder.RegisterType<ToggleState>().SingleInstance();
		_container = builder.Build();
		_myService = _container.Resolve<IMyService>();
		_toggleState = _container.Resolve<ToggleState>();
		_toggleState.Set(_theToggle, toggleState);
	}

	[TearDown]
	public void AfterTest()
	{
		_container.Dispose();
	}

	[Test]
	public void ShouldHaveProxyAsSingleInstance()
	{
		using (var newScope = _container.BeginLifetimeScope())
		{
			var myService2 = newScope.Resolve<IMyService>();
			_myService.Should().Be.SameInstanceAs(myService2);
		}
	}

	[Test]
	public void ShouldBubbleUpRealException()
	{
		Assert.Throws<ArgumentException>(() => { _myService.MethodThatThrowsArgumentException(); });
	}

	[Test]
	public void ShouldNotLooseStackTrace()
	{
		try
		{
			_myService.MethodThatThrowsArgumentException();
		}
		catch (Exception e)
		{
			var stackTraceLines = e.StackTrace.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
			stackTraceLines[0].Should().Contain("MethodThatThrowsArgumentException");
		}
	}

	[Test]
	public void ShouldReturnCorrectType()
	{
		var value = _myService.Value;
		value.Should().Be.EqualTo(toggleState);
	}

	[Test]
	public void ShouldBeAbleToCallGenericMethod()
	{
		_container.Resolve<IMyService>().Returns100<string>()
			.Should().Be.EqualTo(100);
	}

	[Test]
	public void ShouldChangeReturnedTypeOnTheFly_FirstAccessAfterOverride()
	{
		_toggleState.Set(_theToggle, !_toggleState.IsEnabled(_theToggle));
		var value = _myService.Value;
		value.Should().Be.EqualTo(!toggleState);
	}

	[Test]
	public void ShouldChangeReturnedTypeOnTheFly_FirstAccessBeforeOverride()
	{
		_myService.Value.ToString(); //trigger a first call
		_toggleState.Set(_theToggle, !_toggleState.IsEnabled(_theToggle));
		var value = _myService.Value;
		if (resolveOnce)
		{
			value.Should().Be.EqualTo(toggleState);
		}
		else
		{
			value.Should().Be.EqualTo(!toggleState);
		}
	}
	
	[Test]
	public void ClassProxiesNotSupportedAtTheMoment()
	{
		//got really hairy to work properly so let's just throw if trying to register this way for now...
		//feel free to fix but remember to make sure that toggled component services should work as well
		//(=if components are registered internally)
		//That works, but then all methods/props needs to be virtual on component type... Gave up.
		var builder = new ContainerBuilder();
		Assert.Throws<ArgumentException>(() =>
		{
			builder.RegisterToggledComponent<MyServiceOn, MyServiceOff, object>(_theToggle, resolveOnce);
		});
	}

	public class MyServiceOn : IMyService
	{
		public int Returns100<T>() => 100;

		public bool Value => true;

		public void MethodThatThrowsArgumentException() => 
			throw new ArgumentException();
	}

	public class MyServiceOff : IMyService
	{
		public void MethodThatThrowsArgumentException() => 
			throw new ArgumentException();

		public int Returns100<T>() => 100;

		public bool Value => false;
	}

	public interface IMyService
	{
		int Returns100<T>();
		bool Value { get; }
		void MethodThatThrowsArgumentException();
	}
}