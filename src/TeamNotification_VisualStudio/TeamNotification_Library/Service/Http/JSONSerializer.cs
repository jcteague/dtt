
using System;
using Newtonsoft.Json;

namespace TeamNotification_Library.Service.Http
{
    public class JSONSerializer : ISerializeJSON
    {
        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}