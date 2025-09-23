using System;
using System.Reflection;
using System.Runtime.ExceptionServices;
using LinFu.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;

namespace Toggle.Net.Ioc;

public static class ServiceCollectionExtensions
{
    private static readonly ProxyFactory proxyFactory = new();

    public static void RegisterToggledComponent<TToggleOn, TToggleOff, TInterface>
    (
        this IServiceCollection serviceCollection,
        string toggleName,
        bool resolveOnce
    )
        where TToggleOn : TInterface
        where TToggleOff : TInterface
        where TInterface : class
    {
        if (!typeof(TInterface).IsInterface)
            throw new ArgumentException(
                "TInterface type must be an interface. Toggled class proxies not supported ATM.");

        serviceCollection.AddSingleton(c =>
        {
            if (c.GetService<IToggledRegistrationPicker>() == null)
                throw new ToggledRegistrationIsNotEnabledException();
            return proxyFactory.CreateProxy<TInterface>(
                new toggledTypeInterceptor<TToggleOn, TToggleOff>(c, toggleName, resolveOnce));
        });
    }

    private class toggledTypeInterceptor<TToggleOn, TToggleOff> : IInterceptor
    {
        private readonly IToggledRegistrationPicker _toggledRegistrationPicker;
        private readonly string _toggleName;
        private readonly Lazy<object> _componentToUse;

        public toggledTypeInterceptor(IServiceProvider serviceProvider, string toggleName, bool resolveOnce)
        {
            _toggleName = toggleName;
            _toggledRegistrationPicker = serviceProvider.GetService<IToggledRegistrationPicker>();
            if(resolveOnce) 
                _componentToUse = new Lazy<object>(chooseService);
        }

        private static MethodInfo createGenericMethodInfoIfNeeded(MethodInfo orgMethodInfo, Type[] typeArguments) =>
            orgMethodInfo.ContainsGenericParameters ?
                orgMethodInfo.MakeGenericMethod(typeArguments) :
                orgMethodInfo;
        

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
    
    
    public static void EnableToggledRegistrations<T>(this IServiceCollection serviceCollection) where T : class, IToggledRegistrationPicker => 
        serviceCollection.AddSingleton<IToggledRegistrationPicker, T>();
}