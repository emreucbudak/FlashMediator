using System.ComponentModel;

namespace FlashMediator;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IBaseRequest
    {
    }
    public interface IRequest<TResponse> : IBaseRequest
    {
    }
    public interface IRequest : IBaseRequest
    {
    }

