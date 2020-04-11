using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using YASDM.Model.DTO;

namespace YASDM.Client
{
    public static class ApiUtils
    {
        public async static Task<string> GetErrorString(HttpContent content)
        {
            return (await content.ReadFromJsonAsync<ErrorDTO>()).Message;
        }

        public async static Task<ClientException> GetClientException(HttpContent content)
        {
            return new ClientException(await GetErrorString(content));
        }
    }
}