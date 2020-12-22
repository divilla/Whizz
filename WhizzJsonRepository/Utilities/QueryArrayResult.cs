using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WhizzJsonRepository.Base;
using JsonSerializer = Utf8Json.JsonSerializer;

namespace WhizzJsonRepository.Utilities
{
    public class QueryArrayResult : JsonResponse
    {
        public QueryArrayResult(string result = null)
        {
            _result = result;
        }

        private readonly string _result;

        public override string ToString()
        {
            return _result;
        }

        public JArray ToJArray()
        {
            return JArray.Parse(_result);
        }

        public T[] Deserialize<T>()
        {
            return JsonSerializer.Deserialize<T[]>(_result);
        }

        public async Task<T[]> DeserializeAsync<T>()
        {
            return await JsonSerializer.DeserializeAsync<T[]>(new MemoryStream(Encoding.UTF8.GetBytes(_result)));
        }
    }
}