using FlashMediator.src.FlashMediator.Contracts;
using FlashMediator.src.FlashMediator.Impl;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace FlashMediator.src.FlashMediator.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFlashMediator(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddTransient<IMediator, Mediator>();

            var types = assemblies.SelectMany(a => a.GetTypes())
                                  .Where(t => t.IsClass && !t.IsAbstract && !t.IsInterface);

            foreach (var type in types)
            {
                var interfaces = type.GetInterfaces();

                foreach (var @interface in interfaces)
                {
                    if (!@interface.IsGenericType) continue;

                    var genericTypeDef = @interface.GetGenericTypeDefinition();

                    if (genericTypeDef == typeof(IRequestHandler<>) ||
                        genericTypeDef == typeof(IRequestHandler<,>))
                    {
                        services.AddTransient(@interface, type);
                    }
                }
            }

            return services;
        }

        public static IServiceCollection AddPipelineBehavior(this IServiceCollection services, Type behaviorType)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), behaviorType);
            return services;
        }
    }
}
