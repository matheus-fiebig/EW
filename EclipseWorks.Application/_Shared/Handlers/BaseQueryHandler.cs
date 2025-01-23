using MediatR;

namespace EclipseWorks.Application._Shared.Handlers
{
    public abstract class BaseQueryHandler<TEntity, TResponse> : IRequestHandler<TEntity, TResponse> where TEntity : IRequest<TResponse>
    {
        public async Task<TResponse> Handle(TEntity request, CancellationToken cancellationToken)
        {
            try
            {
                return await TryHandle(request, cancellationToken);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected abstract Task<TResponse> TryHandle(TEntity request, CancellationToken cancellationToken);
    }
}