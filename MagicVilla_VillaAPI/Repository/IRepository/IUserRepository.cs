using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;

namespace MagicVilla_VillaAPI.Repository.IRepository;

public interface IUserRepository
{
    bool IsUniqueUser(string username);
    Task<TokenDTO> Login(LoginRequestDTO loginRequestDto);
    Task<UserDTO> Register(RegisterationRequestDTO registerationRequestDto);
}