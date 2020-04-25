using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using YASDM.Model;
using YASDM.Model.DTO;
using YASDM.Model.Services;

namespace YASDM.Client.Services
{
    public class UserService : IUserService
    {
        private HttpClient _httpClient;
        public UserService(HttpClient httpCLient)
        {
            _httpClient = httpCLient;
        }
        public Task<User> Authenticate(string username, string password)
        {
            throw new System.NotImplementedException();
        }

        public Task<User> Create(AuthRegisterDTO registerDTO)
        {
            throw new System.NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<User>> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public Task<User> GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<User> GetEagerById(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<User>> GetPaginated(PaginationDTO paginationParameters)
        {
            var response = await _httpClient.GetAsync($"api/users?{paginationParameters.AsQueryString()}");
            if (!response.IsSuccessStatusCode)
            {
                throw await ApiUtils.GetClientException(response.Content);
            }
            var users = JsonSerializer.Deserialize<UserDTO[]>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return users.Select(userDTO => new User
            {
                UserName = userDTO.Username,
                Email = userDTO.Email,
                FirstName = userDTO.FirstName
            });

        }

        public Task UpdateCredentials(int id, AuthRegisterDTO updateDTO)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateUser(int id, UserDTO updateDTO)
        {
            throw new System.NotImplementedException();
        }
    }
}