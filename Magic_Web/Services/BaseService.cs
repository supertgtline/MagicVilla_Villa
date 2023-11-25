using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Magic_Web.Models;
using Magic_Web.Services.IServices;
using MagicVilla_Utility;
using Newtonsoft.Json;

namespace Magic_Web.Services;

public class BaseService : IBaseService
{
    public APIResponse responseModel { get; set; }
    public IHttpClientFactory HttpClientFactory { get; set; }

    public BaseService(IHttpClientFactory httpClientFactory)
    {
        this.responseModel = new();
        this.HttpClientFactory = httpClientFactory;
    }

    public async Task<T> SendAsync<T>(APIRequest apiRequest)
    {
        try
        {
            var client = HttpClientFactory.CreateClient("MagicVilla_VillaAPI");
            HttpRequestMessage message = new HttpRequestMessage();
            if (apiRequest.ContentType == SD.ContentType.MultipartFormData)
            {
                message.Headers.Add("Accept","*.*");
            }
            else
            {
                message.Headers.Add("Accept", "application/json");
            }
            message.RequestUri = new Uri(apiRequest.Url);
            if (apiRequest.ContentType == SD.ContentType.MultipartFormData)
            {
                var content = new MultipartFormDataContent();
                foreach (var prop in apiRequest.Data.GetType().GetProperties())
                {
                    var value = prop.GetValue(apiRequest.Data);
                    if (value is FormFile)
                    {
                        var file = (FormFile)value;
                        if (file != null)
                        {
                            content.Add(new StreamContent(file.OpenReadStream()), prop.Name);
                        }
                    }
                    else
                    {
                        content.Add(new StringContent(value == null ? "" : value.ToString()), prop.Name);
                    }
                }

                message.Content = content;
            }
            else
            {
                if (apiRequest.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data),
                        Encoding.UTF8, "application/json");
                } 
            }
            
            switch (apiRequest.ApiType)
            {
                case SD.ApiType.POST:
                    message.Method = HttpMethod.Post;
                    break;
                case SD.ApiType.PUT:
                    message.Method = HttpMethod.Put;
                    break;
                case SD.ApiType.DELETE:
                    message.Method = HttpMethod.Delete;
                    break;
                default:
                    message.Method = HttpMethod.Get;
                    break;
            }

            HttpResponseMessage apiResponse = null;
            if (!string.IsNullOrEmpty(apiRequest.Token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiRequest.Token);
            }

            apiResponse = await client.SendAsync(message);
            var apiContent = await apiResponse.Content.ReadAsStringAsync();
            try
            {
                APIResponse ApiResponse = JsonConvert.DeserializeObject<APIResponse>(apiContent);
                if( ApiResponse!=null &&( apiResponse.StatusCode==System.Net.HttpStatusCode.BadRequest 
                                          || apiResponse.StatusCode == System.Net.HttpStatusCode.NotFound))

                {
                    ApiResponse.StatusCode = HttpStatusCode.BadRequest;
                    ApiResponse.IsSuccess = false;
                    var res = JsonConvert.SerializeObject(ApiResponse);
                    var returnObj = JsonConvert.DeserializeObject<T>(res);
                    return returnObj;
                }
            }
            catch (Exception e)
            {
                var exceptionResponse = JsonConvert.DeserializeObject<T>(apiContent);
                return exceptionResponse;
            }

            var APIResponse = JsonConvert.DeserializeObject<T>(apiContent);
            return APIResponse;
        }
        catch (Exception e)
        {
            var dto = new APIResponse
            {
                ErrorMessages = new List<string> { Convert.ToString(e.Message) },
                IsSuccess = false
            };
            var res = JsonConvert.SerializeObject(dto);
            var APIResponse = JsonConvert.DeserializeObject<T>(res);
            return APIResponse;
        }
    }
}