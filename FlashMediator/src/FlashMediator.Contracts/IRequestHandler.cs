using System;
using System.Collections.Generic;
using System.Text;

namespace FlashMediator.src.FlashMediator.Contracts
{
    public interface IRequestHandler <in TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        Task<TResponse> Handle(TRequest request,CancellationToken cancellationToken);
    }
    public interface IRequestHandler <in TRequest>
        where TRequest : IRequest
    {
        Task Handle(TRequest request,CancellationToken cancellationToken);
    }
}
