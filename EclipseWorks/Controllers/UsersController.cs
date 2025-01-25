using EclipseWorks.Application.Projects.Queries;
using EclipseWorks.Application.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EclipseWorks.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UsersController : BaseController
    {
        private readonly ISender mediatr;

        public UsersController(ISender mediatr)
        {
            this.mediatr = mediatr;
        }

        /// <summary>
        /// Rota auxiliar para buscar todos os usuarios cadastrados
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            return await HandleResponse(async () => await mediatr.Send(new GetAllUsersQuery()));
        }

        /// <summary>
        /// Obtém todos os projetos filtrados
        /// </summary>
        [HttpGet("{userId}/projects")]
        public async Task<IActionResult> GetAllProjects(Guid userId)
        {
            return await HandleResponse(async () => await mediatr.Send(new GetAllProjectsByUserQuery(userId)));
        }
    }
}
