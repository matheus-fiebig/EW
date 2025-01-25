using EclipseWorks.Application._Shared.Models;
using EclipseWorks.Domain._Shared.Interfaces.UOW;
using MediatR;

namespace EclipseWorks.Application._Shared.Handlers
{
    public abstract class BaseCommandHandler<TEntity, TResponse> : IRequestHandler<TEntity, Response> where TEntity : IRequest<Response>
    {
        protected readonly IUnitOfWork unitOfWork;

        public BaseCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Response> Handle(TEntity request, CancellationToken cancellationToken)
        {
            try
            {
                await unitOfWork.BeginTrasactionAsync();
                Response response = await TryHandle(request, cancellationToken);
                
                if(response.Errors != null && response.Errors.Any())
                {
                    await unitOfWork.RollbackTrasactionAsync();
                    return response;
                }

                await unitOfWork.CommitTransactionAsync();
                return response;
            }
            catch (Exception)
            {
                await unitOfWork.RollbackTrasactionAsync();
                throw;
            }
        }

        protected abstract Task<Response> TryHandle(TEntity request, CancellationToken cancellationToken);
    }
}
