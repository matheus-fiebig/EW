﻿using EclipseWorks.Application._Shared.Models;
using EclipseWorks.Application.Tasks.Models;
using MediatR;

namespace EclipseWorks.Application.Tasks.Commands
{
    public sealed record CreateTaskCommand(CreateTaskRequest Body): IRequest<Response>;
}
