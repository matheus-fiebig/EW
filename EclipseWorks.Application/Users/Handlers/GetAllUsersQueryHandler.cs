using EclipseWorks.Application._Shared.Handlers;
using EclipseWorks.Application._Shared.Models;
using EclipseWorks.Application.Users.Models;
using EclipseWorks.Application.Users.Queries;
using EclipseWorks.Domain.Users.Entities;
using EclipseWorks.Domain.Users.Interfaces;

namespace EclipseWorks.Application.Users.Handlers
{
    public class GetAllUsersQueryHandler : BaseQueryHandler<GetAllUsersQuery, Response>
    {
        private readonly IQueryUserRepository queryUserRepository;

        public GetAllUsersQueryHandler(IQueryUserRepository queryUserRepository)
        {
            this.queryUserRepository = queryUserRepository;
        }

        protected override async Task<Response> TryHandle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<UserEntity> users = await queryUserRepository.GetAllAsync(cancellationToken: cancellationToken);
            return Response.FromData(UsersQueryResponse.ToModel(users));
        }
    }
}
