using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using TRMediator.Core;
using TRMediator.Core.Abstractions;

namespace TRMediator.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddMediator(this IServiceCollection services, params Assembly[] assemblies)
        {
            var registers = assemblies.SelectMany(GetPendingRegisters).ToList();
            foreach (var register in registers)
            {
                services.AddScoped(register.InterfaceType, register.ConcreteType);
            }

            services.AddScoped<IMediator, Mediator>();

            return services;
        }

        private static IEnumerable<(Type InterfaceType, Type ConcreteType)> GetPendingRegisters(Assembly assembly)
        {
            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                if (!typeof(IHandler).IsAssignableFrom(type)) continue;
                
                var interfaceTypes = type.GetInterfaces();
                foreach (var interfaceType in interfaceTypes)
                {
                    if (typeof(IHandler).IsAssignableFrom(interfaceType))
                    {
                        yield return (interfaceType, type);
                    }
                }
            }
        }
    }
}
