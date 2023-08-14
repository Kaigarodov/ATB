using System.Net;
namespace Api.Common.Account.Dto.Response;
public record ErrorResponse
{
    public HttpStatusCode Code { get; init; } = HttpStatusCode.BadRequest;
    public string Message { get; init; } = "Неизвестная ошибка";
}