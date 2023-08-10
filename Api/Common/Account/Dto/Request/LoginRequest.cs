using System.ComponentModel.DataAnnotations;

namespace Api.Areas.Rest.Controllers.Account.Dto.Request;

public record LoginRequest
{
    [Required]
    [RegularExpression(@"^7\d{10}$", ErrorMessage = "Not a valid phone number")]
    public string Phone { get; init; }
    
    [Required]
    [StringLength(20)]
    public string Password { get; init; }
}