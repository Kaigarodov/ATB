using System.ComponentModel.DataAnnotations;

namespace Api.Areas.Rest.Controllers.Account.Dto.Request;

public record RegisterRequest
{
    [Required]
    [MaxLength(250)]
    public string FIO { get; init; }
    [Required]
    [RegularExpression(@"^7\d{10}$", ErrorMessage = "Not a valid phone number")]
    public string Phone { get; init; }
    [Required]
    [EmailAddress]
    [MaxLength(150)]
    public string Email { get; init; }
    [Required] 
    [MaxLength(20)]
    public string Password { get; init; }
    [Required] 
    [MaxLength(20)]
    [Compare("Password", ErrorMessage = "The password must match the confirmation password")]
    public string PasswordConfirm { get; init; }
}