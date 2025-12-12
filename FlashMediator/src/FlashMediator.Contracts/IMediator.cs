using System;
using System.Collections.Generic;
using System.Text;

namespace FlashMediator.src.FlashMediator.Contracts
{
    public interface IMediator
    {
        Task Send(IRequest request, CancellationToken cancellationToken = default);

 
        Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
    }
}
