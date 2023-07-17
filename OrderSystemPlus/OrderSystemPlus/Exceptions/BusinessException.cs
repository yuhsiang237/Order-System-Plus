using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace OrderSystemPlus
{
    public class BusinessException : Exception
    {
        public int StatusCode { get; }

        public BusinessException(string message, int statusCode = StatusCodes.Status500InternalServerError)
            : base(message)
        {
            StatusCode = statusCode;
        }

        public override string ToString()
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            return JsonConvert.SerializeObject(new
            {
                StatusCode,
                Message = Message
            }, settings);
        }
    }
}
