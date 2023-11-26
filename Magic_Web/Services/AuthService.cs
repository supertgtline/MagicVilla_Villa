using Magic_Web.Models;
using Magic_Web.Models.Dto;
using Magic_Web.Services.IServices;
using MagicVilla_Utility;

namespace Magic_Web.Services;

public class AuthService : IAuthService
{
    private readonly IHttpClientFactory _clientFactory;
    private string villaUrl;
    private readonly IBaseService _baseService;

    public AuthService(IHttpClientFactory clientFactory, IConfiguration configuration, IBaseService baseService)
    {        _clientFactory = clientFactory;
        villaUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");
        _baseService = baseService;
    }

    public async Task<T> LoginAsync<T>(LoginRequestDTO obj)
    {
        return await _baseService.SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.POST,
            Data = obj,
            Url = villaUrl + $"/api/{SD.CurrentAPIVersion}/UsersAuth/login"
        }, withBearer:false);
    }

    public async Task<T> RegisterAsync<T>(RegistrationRequestDTO obj)
    {
        return await _baseService.SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.POST,
            Data = obj,
            Url = villaUrl + "/api/v1/UsersAuth/register"
        }, withBearer: false);
    }
}