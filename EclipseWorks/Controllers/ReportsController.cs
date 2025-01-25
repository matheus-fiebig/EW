using EclipseWorks.Application.Reports.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EclipseWorks.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ReportsControlller : BaseController
    {
        private readonly ISender mediatr;
        public ReportsControlller(ISender mediatr)
        {
            this.mediatr = mediatr;
        }

        /// <summary>
        /// Obtém dados do relatório
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetGenericReportData([FromQuery] Guid loggedUserId, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            return await HandleResponse(async () => await mediatr.Send(new GetGenericReportQuery(loggedUserId, from, to)));
        }
    }
}




    
