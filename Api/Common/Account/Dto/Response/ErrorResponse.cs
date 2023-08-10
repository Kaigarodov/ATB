using System.Net;

namespace Api.Areas.Rest.Controllers.Account.Dto.Response;

public record ErrorResponse
{
    public HttpStatusCode Code { get; init; }
    public string Message { get; init; }
}