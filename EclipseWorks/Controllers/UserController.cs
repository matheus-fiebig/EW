using EclipseWorks.Application.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EclipseWorks.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController : BaseController
    {
        private readonly ISender mediatr;

        public UserController(ISender mediatr)
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
    }
}
