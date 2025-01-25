using System.Net;
using EclipseWorks.Domain._Shared.Models;

namespace EclipseWorks.Application._Shared.Models
{
    public class Response
    {
        public object Data { get; init; }

        public List<Issue> Errors { get; init; }

        public HttpStatusCode StatusCode { get; init; }

        private Response()
        {

        }
        public static Response Empty()
        {
            return new Response { StatusCode = HttpStatusCode.NoContent };
        }

        public static Response FromData(object data)
        {
            return new Response { Data = data, StatusCode = HttpStatusCode.OK };
        }

        public static Response FromError(Issue error)
        {
            return new Response { Errors = [error], StatusCode = HttpStatusCode.BadRequest };
        }

        public static Response FromErrors(List<Issue> errors)
        {
            return new Response { Errors = errors, StatusCode = HttpStatusCode.BadRequest };
        }

        public static Response FromException(Exception ex)
        {
            return new Response { Errors = [Issue.CreateNew(ex.HResult.ToString(), "Something bad happens")], StatusCode = HttpStatusCode.InternalServerError };
        }

        public static implicit operator Response(Issue issue)
        {
            return FromError(issue);
        }

        public static implicit operator Response(Exception ex)
        {
            return FromException(ex);
        }
    }
}
