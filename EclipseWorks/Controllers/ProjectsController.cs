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
    public class ProjectsController : BaseController
    {
        private readonly ISender mediatr;

        public ProjectsController(ISender mediatr)
        {
            this.mediatr = mediatr;
        }

        /// <summary>
        /// Obtém todas as tarefas de um determinado projeto
        /// </summary>
        [HttpGet("{projectId}/tasks")]
        public async Task<IActionResult> GetAllTasksByProject(Guid projectId)
        {
            return await HandleResponse(async () => await mediatr.Send(new GetAllTasksByProjectQuery(projectId)));
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
        /// Deleta um projeto
        /// </summary>
        [HttpDelete("{projectId}")]
        public async Task<IActionResult> Delete(Guid projectId)
        {
            return await HandleResponse(async () => await mediatr.Send(new DeleteProjectCommand(projectId)));
        }
    }
}
