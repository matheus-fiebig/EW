using EclipseWorks.Application.Tasks.Commands;
using EclipseWorks.Application.Tasks.Models;
using EclipseWorks.Application.Tasks.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EclipseWorks.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TaskController : BaseController
    {
        private readonly ISender mediatr;

        public TaskController(ISender mediatr)
        {
            this.mediatr = mediatr;
        }

        /// <summary>
        /// Obtém todas as tarefas de um determinado projeto
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllTasksByProject([FromQuery] Guid? projectId)
        {
            return await HandleResponse(async () => await mediatr.Send(new GetAllTasksByProjectQuery(projectId)));
        }

        /// <summary>
        /// Cria uma tarefa em determinado projeto
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskRequest request)
        {
            return await HandleResponse(async () => await mediatr.Send(new CreateTaskCommand(request)));
        }

        /// <summary>
        /// Cria uma tarefa em determinado projeto
        /// </summary>
        [HttpPost("add-commentary")]
        public async Task<IActionResult> AddCommentary([FromBody] CreateTaskRequest request)
        {
            return await HandleResponse(async () => await mediatr.Send(new CreateTaskCommand(request)));
        }

        /// <summary>
        /// Atualiza uma tarefa 
        /// </summary>
        [HttpPut("{taskId}")]
        public async Task<IActionResult> Update(Guid taskId, [FromBody] UpdateTaskRequest request)
        {
            return await HandleResponse(async () => await mediatr.Send(new UpdateTaskCommand(taskId, request)));
        }

        /// <summary>
        /// Deleta uma tarefa
        /// </summary>
        [HttpDelete("{taskId}")]
        public async Task<IActionResult> Delete(Guid taskId)
        {
            return await HandleResponse(async () => await mediatr.Send(new DeleteTaskCommand(taskId)));
        }
    }
}
