namespace Logic.Account.Models;

public record AuthorizationModel
{
    public string Phone { get; init; }
    public string Password { get; init; }

}