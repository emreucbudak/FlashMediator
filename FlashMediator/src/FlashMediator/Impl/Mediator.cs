using System.Collections.Concurrent;

namespace FlashMediator;

    public class Mediator : IMediator
    {
        private readonly IServiceProvider _serviceProvider;
        private static readonly ConcurrentDictionary<Type, RequestHandlerBase> _requestHandlers = new();

        public Mediator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task Send(IRequest request, CancellationToken cancellationToken = default)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            var requestType = request.GetType();

            var handler = (RequestHandlerWrapper)_requestHandlers.GetOrAdd(requestType, type =>
            {
                var wrapperType = typeof(VoidRequestHandlerWrapperImpl<>).MakeGenericType(type);
                return (RequestHandlerBase)Activator.CreateInstance(wrapperType)!;
            });

            return handler.Handle(request, _serviceProvider, cancellationToken);
        }

        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            var requestType = request.GetType();

            var handler = (RequestHandlerWrapper<TResponse>)_requestHandlers.GetOrAdd(requestType, type =>
            {
                var wrapperType = typeof(GenericRequestHandlerWrapperImpl<,>).MakeGenericType(type, typeof(TResponse));
                return (RequestHandlerBase)Activator.CreateInstance(wrapperType)!;
            });

            return handler.Handle(request, _serviceProvider, cancellationToken);
        }
    }

