using System;
using System.Reflection;
using System.Runtime.ExceptionServices;
using Autofac;
using Autofac.Builder;
using LinFu.DynamicProxy;

namespace Toggle.Net.Autofac;

public static class ContainerBuilderExtensions
{
	private static readonly ProxyFactory proxyFactory = new();
	
	public static IRegistrationBuilder<TInterface, SimpleActivatorData, SingleRegistrationStyle> RegisterToggledComponent<TToggleOn, TToggleOff, TInterface>
		(
			this ContainerBuilder builder, 
			string toggleName, 
			bool resolveOnce
		)
		where TToggleOn : TInterface
		where TToggleOff : TInterface
		where TInterface : class
	{
		if (!typeof(TInterface).IsInterface)
			throw new ArgumentException("TInterface type must be an interface. Toggled class proxies not supported ATM.");
		
		return builder.Register(c =>
		{
			if (!c.IsRegistered<IToggledRegistrationPicker>())
				throw new ToggledRegistrationIsNotEnabledException();
			return proxyFactory.CreateProxy<TInterface>(
				new toggledTypeInterceptor<TToggleOn, TToggleOff>(c, toggleName, resolveOnce));
		});
	}

	private static MethodInfo createGenericMethodInfoIfNeeded(MethodInfo orgMethodInfo, Type[] typeArguments) =>
		orgMethodInfo.ContainsGenericParameters ?
			orgMethodInfo.MakeGenericMethod(typeArguments) :
			orgMethodInfo;

	private class toggledTypeInterceptor<TToggleOn, TToggleOff> : IInterceptor
	{
		private readonly IToggledRegistrationPicker _toggledRegistrationPicker;
		private readonly string _toggleName;
		private readonly Lazy<object> _componentToUse;

		public toggledTypeInterceptor(IComponentContext componentContext, string toggleName, bool resolveOnce)
		{
			_toggleName = toggleName;
			_toggledRegistrationPicker = componentContext.Resolve<IToggledRegistrationPicker>();
			if(resolveOnce) 
				_componentToUse = new Lazy<object>(chooseService);
		}


		public object Intercept(InvocationInfo info)
		{
			var svc = _componentToUse == null ? 
				chooseService() : 
				_componentToUse.Value;
			try
			{
				var realTargetMethod = createGenericMethodInfoIfNeeded(info.TargetMethod, info.TypeArguments);
				return realTargetMethod.Invoke(svc, info.Arguments);
			}
			catch (TargetInvocationException e)
			{
				ExceptionDispatchInfo.Capture(e.InnerException).Throw();
				throw;
			}
		}

		private object chooseService() => 
			_toggledRegistrationPicker.PickService<TToggleOn, TToggleOff>(_toggleName);
	}
    
    public static void EnableToggledRegistrations<T>(this ContainerBuilder builder) where T : IToggledRegistrationPicker => 
        builder.RegisterType<T>().As<IToggledRegistrationPicker>().SingleInstance();
}