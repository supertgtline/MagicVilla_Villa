using MagicVilla_VillaAPI.Models.Dto;

namespace Magic_Web.Services.IServices;

public interface ITokenProvider
{
    void SetToken(TokenDTO tokenDTO);
    TokenDTO? GetToken();
    void ClearToken();
}