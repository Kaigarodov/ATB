namespace Logic.Account.Models;

public record AuthUserModel
{
    public string Phone { get; init; }
    public string Password { get; init; }

}