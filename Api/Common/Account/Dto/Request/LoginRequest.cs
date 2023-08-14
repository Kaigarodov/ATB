using System.ComponentModel.DataAnnotations;

namespace Api.Common.Account.Dto.Request;

/// <summary>
/// Запрос на авторизацию пользователя
/// </summary>
public record LoginRequest
{
    /// <summary>
    /// Номер телефона
    /// </summary>
    [Required]
    [RegularExpression(@"^7\d{10}$", ErrorMessage = "Not a valid phone number")]
    public string Phone { get; init; }
    
    /// <summary>
    /// Пароль
    /// </summary>
    [Required]
    [StringLength(20)]
    public string Password { get; init; }
}