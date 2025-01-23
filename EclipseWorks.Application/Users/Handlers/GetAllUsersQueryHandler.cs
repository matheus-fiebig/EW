using EclipseWorks.Application._Shared.Models;
using EclipseWorks.Application.Users.Models;
using EclipseWorks.Application.Users.Queries;
using EclipseWorks.Domain.Users.Entities;
using EclipseWorks.Domain.Users.Interfaces;
using MediatR;

namespace EclipseWorks.Application.Users.Handlers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Response>
    {
        private readonly IQueryUserRepository queryUserRepository;

        public GetAllUsersQueryHandler(IQueryUserRepository queryUserRepository)
        {
            this.queryUserRepository = queryUserRepository;
        }

        public async Task<Response> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<UserEntity> users = await queryUserRepository.GetAllAsync(cancellationToken: cancellationToken);
            return Response.FromData(UsersQueryResponse.ToModel(users));
        }
    }
}
