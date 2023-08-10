using Dal.Helpers.Attributes;

namespace Dal.Models;

[CustomTableName("users")]
public class UserDal
{
    public int Id { get; set; } = default!;

    public string FIO { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public DateTime LastLogin { get; set; } = default!;

}