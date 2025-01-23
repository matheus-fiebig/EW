using EclipseWorks.Application._Shared.Models;
using EclipseWorks.Domain._Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace EclipseWorks.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected async Task<IActionResult> HandleResponse(Func<Task<Response>> func)
        {
            try
            {
                var response = await func();
                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception)
            {
                return Problem(
                    "Error",
                    "Ocorreu um erro inesperado no servidor.",
                    StatusCodes.Status500InternalServerError
                );
            }
        }
    }
}
