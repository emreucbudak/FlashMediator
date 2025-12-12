using FlashMediator.src.Entity;
using FlashMediator.src.FlashMediator.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FlashMediator.src.FlashMediator.Wrappers
{
    internal abstract class RequestHandlerBase
    {
    }

    internal abstract class RequestHandlerWrapper : RequestHandlerBase
    {
        public abstract Task Handle(object request, IServiceProvider serviceProvider, CancellationToken cancellationToken);
    }

    internal abstract class RequestHandlerWrapper<TResponse> : RequestHandlerBase
    {
        public abstract Task<TResponse> Handle(object request, IServiceProvider serviceProvider, CancellationToken cancellationToken);
    }

    internal class VoidRequestHandlerWrapperImpl<TRequest> : RequestHandlerWrapper
        where TRequest : IRequest
    {
        public override Task Handle(object request, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            async Task<Unit> Handler()
            {
                await serviceProvider.GetRequiredService<IRequestHandler<TRequest>>()
                    .Handle((TRequest)request, cancellationToken);
                return Unit.Value;
            }

            return serviceProvider
                .GetServices<IPipelineBehavior<TRequest, Unit>>()
                .Reverse()
                .Aggregate((RequestHandlerDelegate<Unit>)Handler,
                    (next, pipeline) => () => pipeline.Handle((TRequest)request, next, cancellationToken))();
        }
    }

    internal class GenericRequestHandlerWrapperImpl<TRequest, TResponse> : RequestHandlerWrapper<TResponse>
        where TRequest : IRequest<TResponse>
    {
        public override Task<TResponse> Handle(object request, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            Task<TResponse> Handler()
            {
                return serviceProvider.GetRequiredService<IRequestHandler<TRequest, TResponse>>()
                    .Handle((TRequest)request, cancellationToken);
            }

            return serviceProvider
                .GetServices<IPipelineBehavior<TRequest, TResponse>>()
                .Reverse()
                .Aggregate((RequestHandlerDelegate<TResponse>)Handler,
                    (next, pipeline) => () => pipeline.Handle((TRequest)request, next, cancellationToken))();
        }
    }
}