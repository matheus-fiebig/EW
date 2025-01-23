using EclipseWorks.Application._Shared.Models;
using EclipseWorks.Domain._Shared.Enums;
using MediatR;

namespace EclipseWorks.Application.Tasks.Commands
{
    public sealed record UpdateTaskRequest(string Title, string Description, DateTime DueDate, EProgress Progress);
}
