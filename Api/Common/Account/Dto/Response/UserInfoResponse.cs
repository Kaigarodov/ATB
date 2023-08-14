namespace Api.Common.Account.Dto.Response;

public record UserInfoResponse
{
    public string FIO { get; init; }
    public string Phone { get; init; }
    public string Email { get; init; }
    public string LastLogin { get; init; }
}