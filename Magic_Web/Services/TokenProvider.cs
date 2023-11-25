using Magic_Web.Services.IServices;
using MagicVilla_Utility;
using MagicVilla_VillaAPI.Models.Dto;

namespace Magic_Web.Services;

public class TokenProvider : ITokenProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public TokenProvider(IHttpContextAccessor contextAccessor)
    {
        _httpContextAccessor = contextAccessor;
    }
    public void SetToken(TokenDTO tokenDTO)
    {
        var cookieOptions = new CookieOptions { Expires = DateTime.UtcNow.AddDays(60) };
        _httpContextAccessor.HttpContext?.Response.Cookies.Append(SD.AccessToken,tokenDTO.Token, cookieOptions);
    }

    public TokenDTO GetToken()
    {
        try
        {
            bool hasAccessToken =
                _httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(SD.AccessToken, out string accessToken);
            TokenDTO tokenDTO = new()
            {
                Token = accessToken
            };
            return hasAccessToken ? tokenDTO : null;
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public void ClearToken()
    {
        _httpContextAccessor.HttpContext?.Response.Cookies.Delete(SD.AccessToken);
    }
}