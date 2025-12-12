using System.Threading;       
using System.Threading.Tasks;
namespace FlashMediator.src.FlashMediator.Contracts
{
    public interface IPipelineBehavior<in TRequest, TResponse>
    {
        Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken);
    }
    public delegate Task<TResponse> RequestHandlerDelegate<TResponse>();
}
