using EclipseWorks.Application._Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace EclipseWorks.Helpers
{
    public interface IBaseController
    {
        Task<IActionResult> HandleResponse(Func<Task<Response>> func);
    }
}
