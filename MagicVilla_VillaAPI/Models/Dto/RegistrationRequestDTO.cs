namespace MagicVilla_VillaAPI.Models.Dto;

public class RegistrationRequestDTO
{
    public string UserName { get; set; }
    public string Name { get; }
    public string Password { get; }
    public string Role { get; }
}