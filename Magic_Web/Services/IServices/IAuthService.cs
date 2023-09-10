using Magic_Web.Models.Dto;

namespace Magic_Web.Services.IServices;

public interface IAuthService
{
    Task<T> LoginAsync<T>(LoginRequestDTO loginRequestDto);
    Task<T> RegisterAsync<T>(UserDTO objToCreeate);
}