using System.ComponentModel.DataAnnotations;

namespace Api.Common.Account.Dto.Request;

/// <summary>
/// Запрос на создание нового пользователя
/// </summary>
public record RegisterRequest
{
    /// <summary>
    /// Фамилия Имя Отчество
    /// </summary>
    [Required]
    [MaxLength(250)]
    public string FIO { get; init; }
    
    /// <summary>
    /// Номер телефона
    /// </summary>
    [Required]
    [RegularExpression(@"^7\d{10}$", ErrorMessage = "Not a valid phone number")]
    public string Phone { get; init; }
    
    /// <summary>
    /// Email пользователя
    /// </summary>
    [Required]
    [EmailAddress]
    [MaxLength(150)]
    public string Email { get; init; }

    /// <summary>
    /// Пароль
    /// </summary>
    [Required] 
    [MaxLength(20)]
    public string Password { get; init; }
    
    /// <summary>
    /// Подтверждение пароля
    /// </summary>
    [Required] 
    [MaxLength(20)]
    [Compare("Password", ErrorMessage = "The password must match the confirmation password")]
    public string PasswordConfirm { get; init; }
}