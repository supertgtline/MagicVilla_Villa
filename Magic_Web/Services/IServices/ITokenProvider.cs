using Magic_Web.Models.Dto;

namespace Magic_Web.Services.IServices;

public interface ITokenProvider
{
    void SetToken(TokenDTO tokenDTO);
    TokenDTO? GetToken();
    void ClearToken();
}