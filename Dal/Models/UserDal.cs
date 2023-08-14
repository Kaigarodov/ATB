using Dal.Helpers.Attributes;

namespace Dal.Models;

[CustomTableName("users")]
public class UserDal
{
    public int Id { get; set; }

    public string FIO { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime LastLogin { get; set; }

}