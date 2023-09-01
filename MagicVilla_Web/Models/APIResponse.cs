using System.Net;

namespace MagicVilla_VillaAPI.Models
{

    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<String> ErrorMessages { get; set; }
        public Object Result { get; set; }
    }
}