using Magic_Web.Models;
using Magic_Web.Models.Dto;
using Magic_Web.Services.IServices;
using MagicVilla_Utility;

namespace Magic_Web.Services;

public class VillaService : BaseService,IVillaService
{
    private readonly IHttpClientFactory _clientFactory;
    private string villaUrl;
    private IVillaService _villaServiceImplementation;
    private IVillaService _villaServiceImplementation1;

    public VillaService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpClientFactory)
    {
        _clientFactory = httpClientFactory;
        villaUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");
    }

    public Task<T> GetAllAsync<T>(string token)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.GET,
            Url = villaUrl + "/api/v1/villaAPI",
            Token = token
        });
    }

    public Task<T> GetAsync<T>(int id,string token)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.GET,
            Url = villaUrl + "/api/v1/villaAPI/"+id,
            Token = token
        });
    }

    public Task<T> CreateAsync<T>(VillaCreateDTO dto,string token)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.POST,
            Data = dto,
            Url = villaUrl + "/api/v1/villaAPI",
            Token = token
        });
    }

    public Task<T> UpdateAsync<T>(VillaUpdateDTO dto, string token)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.PUT,
            Data = dto,
            Url = villaUrl + "/api/v1/villaAPI/"+dto.Id,
            Token = token
        });
    }

    public Task<T> DeleteAsync<T>(int id, string token)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.DELETE,
            Url = villaUrl + "/api/v1/villaAPI/"+id,
            Token = token
        });
    }
}