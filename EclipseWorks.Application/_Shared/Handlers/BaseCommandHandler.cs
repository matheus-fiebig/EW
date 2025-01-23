using EclipseWorks.Domain._Shared.Interfaces.UOW;
using MediatR;

namespace EclipseWorks.Application._Shared.Handlers
{
    public abstract class BaseCommandHandler<TEntity, TResponse> : IRequestHandler<TEntity, TResponse> where TEntity : IRequest<TResponse>
    {
        protected readonly IUnitOfWork unitOfWork;

        public BaseCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<TResponse> Handle(TEntity request, CancellationToken cancellationToken)
        {
            try
            {
                await unitOfWork.BeginTrasactionAsync();
                return await TryHandle(request, cancellationToken);
            }
            catch (Exception)
            {
                await unitOfWork.RollbackTrasactionAsync();
                throw;
            }
        }

        protected abstract Task<TResponse> TryHandle(TEntity request, CancellationToken cancellationToken);
    }
}
