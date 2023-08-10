namespace Api.Areas.Cabinet.Models;

public class CabinetViewModel
{
    public string FIO { get; init; }
    public string Phone { get; init; }
    public string Email { get; init; }
    public string LastLogin { get; init; }

    public CabinetViewModel()
    {
        FIO = "Кайгародов Андрей Иванович";
        Phone = "89920141674";
        Email = "kaigarodov2012@yandex.ru";
        LastLogin = DateTime.Now.ToString();
    }
}