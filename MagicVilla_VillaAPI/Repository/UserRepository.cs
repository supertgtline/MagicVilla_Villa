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
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IMapper _mapper;
    private string secretKey;

    public UserRepository(ApplicationDbContext db,
        IConfiguration configuration,
        UserManager<ApplicationUser> userManager,
        IMapper mapper,
        RoleManager<IdentityRole> roleManager)
    {
        _db = db;
        _userManager = userManager;
        secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        _mapper = mapper;
        _roleManager = roleManager;
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

    public async Task<TokenDTO> Login(LoginRequestDTO loginRequestDto)
    {
        var user = _db.ApplicationUsers.FirstOrDefault
            (u => u.UserName.ToLower() == loginRequestDto.UserName.ToLower());
        bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
        if (user == null || isValid == false)
        {
            return new TokenDTO()
            {
                AccessToken = ""
            };
        }

        var accessToken = await GetAccessToken(user);
        
        TokenDTO tokenDto = new TokenDTO()
        {
            AccessToken = accessToken,
                
        };
        return tokenDto;
    }

    public async Task<UserDTO> Register(RegisterationRequestDTO registerationRequestDTO)
    {
        ApplicationUser user = new()
        {
            UserName = registerationRequestDTO.UserName,
            Email = registerationRequestDTO.UserName,
            NormalizedEmail = registerationRequestDTO.UserName.ToUpper(),
            Name = registerationRequestDTO.Name
        };
        try
        {
            var result = await _userManager.CreateAsync(user, registerationRequestDTO.Password);
            if (result.Succeeded)
            {
                if (!_roleManager.RoleExistsAsync(registerationRequestDTO.Role).GetAwaiter().GetResult()){
                    await _roleManager.CreateAsync(new IdentityRole(registerationRequestDTO.Role)); }
                await _userManager.AddToRoleAsync(user, registerationRequestDTO.Role);
                var userToReturn = _db.ApplicationUsers
                    .FirstOrDefault(u => u.UserName == registerationRequestDTO.UserName);
                return _mapper.Map<UserDTO>(userToReturn);
            }
        }
        catch(Exception e)
        {

        }

        return new UserDTO();

    }
    private async Task<string> GetAccessToken(ApplicationUser user)
    {
        //if user was found generate JWT Token
        var roles = await _userManager.GetRolesAsync(user);
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secretKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.UserName.ToString()),
                new Claim(ClaimTypes.Role, roles.FirstOrDefault())
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenStr = tokenHandler.WriteToken(token);
        return tokenStr;
    }
}