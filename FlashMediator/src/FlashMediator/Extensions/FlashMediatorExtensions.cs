using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FlashMediator;

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

