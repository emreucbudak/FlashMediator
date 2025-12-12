using System;
using System.Collections.Generic;
using System.Text;

namespace FlashMediator.src.FlashMediator.Contracts
{
    public interface IBaseRequest
    {
    }
    public interface IRequest<TResponse> : IBaseRequest
    {
    }
    public interface IRequest : IBaseRequest
    {
    }
}
