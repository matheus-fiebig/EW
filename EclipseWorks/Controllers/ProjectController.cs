using EclipseWorks.Application.Projects.Commands;
using EclipseWorks.Application.Projects.Models;
using EclipseWorks.Application.Projects.Queries;
using EclipseWorks.Application.Tasks.Commands;
using EclipseWorks.Application.Tasks.Models;
using EclipseWorks.Application.Tasks.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EclipseWorks.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProjectController : BaseController
    {
        private readonly ISender mediatr;

        public ProjectController(ISender mediatr)
        {
            this.mediatr = mediatr;
        }

        /// <summary>
        /// Obtém todos os projetos filtrados
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllProjectsByUser([FromQuery] Guid? userId)
        {
            return await HandleResponse(async () => await mediatr.Send(new GetAllProjectsByUserQuery(userId)));
        }

        /// <summary>
        /// Cria um projeto
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProjectRequest request)
        {
            return await HandleResponse(async () => await mediatr.Send(new CreateProjectCommand(request)));
        }

        /// <summary>
        /// Deleta uma tarefa
        /// </summary>
        [HttpDelete("{projectId}")]
        public async Task<IActionResult> Delete(Guid projectId)
        {
            return await HandleResponse(async () => await mediatr.Send(new DeleteProjectCommand(projectId)));
        }
    }
}
