using System.Runtime.Serialization;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace OrderSystemPlus
{
    public class BussinessException : Exception
    {
        public BussinessException()
        {
        }

        public BussinessException(string message)
            : base(message)
        {
        }

        public BussinessException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("error", Message);
        }

        public override string ToString()
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            return JsonConvert.SerializeObject(this, settings);
        }
    }
}
