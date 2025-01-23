using EclipseWorks.Application.Commentaries.Commands;
using EclipseWorks.Application.Commentaries.Models;
using EclipseWorks.Application.Tasks.Commands;
using EclipseWorks.Application.Tasks.Models;
using EclipseWorks.Application.Tasks.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EclipseWorks.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TasksController : BaseController
    {
        private readonly ISender mediatr;

        public TasksController(ISender mediatr)
        {
            this.mediatr = mediatr;
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
        [HttpPost("{taskId}/add-commentary")]
        public async Task<IActionResult> AddCommentary(Guid taskId, [FromBody] AddCommentaryRequest request)
        {
            return await HandleResponse(async () => await mediatr.Send(new AddCommentaryCommand(taskId, request)));
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
