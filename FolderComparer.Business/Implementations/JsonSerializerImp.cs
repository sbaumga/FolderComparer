using FolderComparer.Business.Interfaces;
using Newtonsoft.Json;

namespace FolderComparer.Business.Implementations
{
    public class JsonSerializerImp : ISerializer
    {
        public string Serialize<T>(T data)
        {
            var result = JsonConvert.SerializeObject(data);
            return result;
        }

        public T Deserialize<T>(string data)
        {
            var result = JsonConvert.DeserializeObject<T>(data);
            return result;
        }
    }
}