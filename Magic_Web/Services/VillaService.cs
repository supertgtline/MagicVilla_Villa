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

    public Task<T> GetAllAsync<T>()
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.GET,
            Url = villaUrl + $"/api/{SD.CurrentAPIVersion}/villaAPI"
        });
    }

    public Task<T> GetAsync<T>(int id)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.GET,
            Url = villaUrl + $"/api/{SD.CurrentAPIVersion}/villaAPI/"+id
        });
    }

    public Task<T> CreateAsync<T>(VillaCreateDTO dto)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.POST,
            Data = dto,
            Url = villaUrl + $"/api/{SD.CurrentAPIVersion}/villaAPI",
            ContentType = SD.ContentType.MultipartFormData
        });
    }

    public Task<T> UpdateAsync<T>(VillaUpdateDTO dto)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.PUT,
            Data = dto,
            Url = villaUrl + $"/api/{SD.CurrentAPIVersion}/villaAPI/"+dto.Id,
            ContentType = SD.ContentType.MultipartFormData
        });
    }

    public Task<T> DeleteAsync<T>(int id)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.DELETE,
            Url = villaUrl + $"/api/{SD.CurrentAPIVersion}/villaAPI/"+id
        });
    }
}