using EclipseWorks.Application._Shared.Models;
using EclipseWorks.Domain._Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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

                return response!.StatusCode switch
                {
                    HttpStatusCode.OK => Ok(response),
                    HttpStatusCode.NoContent => NoContent(),
                    HttpStatusCode.BadRequest => BadRequest(response),
                    HttpStatusCode.NotFound => NotFound(),
                    _ => StatusCode((int)response.StatusCode,response),
                };
            }
            catch (Exception ex)
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
