using EclipseWorks.Application._Shared.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipseWorks.Application.Tasks.Commands
{
   public sealed record DeleteTaskCommand(Guid TaskId) : IRequest<Response>;
}
