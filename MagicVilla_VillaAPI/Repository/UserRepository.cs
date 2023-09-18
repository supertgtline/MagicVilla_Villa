using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace MagicVilla_VillaAPI.Repository;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;
    private string secretKey;

    public UserRepository(ApplicationDbContext db,
        IConfiguration configuration,
        UserManager<ApplicationUser> userManager,
        IMapper mapper)
    {
        _db = db;
        _userManager = userManager;
        secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        mapper = mapper;
    }
    
    public bool IsUniqueUser(string username)
    {
        var user = _db.ApplicationUsers.FirstOrDefault(x => x.UserName == username);
        if (user == null)
        {
            return true;
        }

        return false;
    }

    public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDto)
    {
        var user = _db.ApplicationUsers.FirstOrDefault
        (u => u.UserName.ToLower() == loginRequestDto.UserName.ToLower());
        bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
        if (user == null || isValid == false)
        {
            return new LoginResponseDTO()
            {
                Token = "",
                User = null
            };
        }
        // if user was found generate JWT Token
        var roles = await _userManager.GetRolesAsync(user);
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secretKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new(ClaimTypes.Name, user.Id.ToString()),
                new(ClaimTypes.Role, roles.FirstOrDefault() ?? throw new InvalidOperationException())
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        LoginResponseDTO loginResponseDto = new LoginResponseDTO()
        {
            Token = tokenHandler.WriteToken(token),
            User = _mapper.Map<UserDTO>(user),
            Role = roles.FirstOrDefault()
        };
        return loginResponseDto;
    }

    public async Task<LocalUser> Register(RegisterationRequestDTO registerationRequestDto)
    {
        LocalUser user = new LocalUser()
        {
            UserName = registerationRequestDto.UserName,
            Password = registerationRequestDto.Password,
            Name = registerationRequestDto.Name,
            Role = registerationRequestDto.Role
        };
        _db.LocalUsers.Add(user);
        await _db.SaveChangesAsync();
        user.Password = "";
        return user;
    }
}