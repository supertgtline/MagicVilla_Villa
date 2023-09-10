using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepository;

namespace MagicVilla_VillaAPI.Repository;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _db;
    private string secretKey;

    public UserRepository(ApplicationDbContext db,
        IConfiguration configuration)
    {
        _db = db;
        secretKey = configuration.GetValue<string>("ApiSettings:Secret");
    }
    
    public bool IsUniqueUser(string username)
    {
        var user = _db.LocalUsers.FirstOrDefault(x => x.UserName == username);
        if (user == null)
        {
            return true;
        }

        return false;
    }

    public Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDto)
    {
        var user = _db.LocalUsers.FirstOrDefault
        (u => u.UserName.ToLower() == loginRequestDto.UserName.ToLower()
              && u.Password == loginRequestDto.Passwrod);
        if (user == null)
        {
            return null;
        }
        // if user was found generate JWT Token
        
    }

    public async Task<LocalUser> Register(RegistrationRequestDTO registrationRequestDto)
    {
        LocalUser user = new LocalUser()
        {
            UserName = registrationRequestDto.UserName,
            Password = registrationRequestDto.Password,
            Name = registrationRequestDto.Name,
            Role = registrationRequestDto.Role
        };
        _db.LocalUsers.Add(user);
        await _db.SaveChangesAsync();
        user.Password = "";
        return user;
    }
}