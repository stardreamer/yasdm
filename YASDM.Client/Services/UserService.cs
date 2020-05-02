using System;
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

        public async Task<User> GetById(int id)
        {
            return await GetEagerById(id);
        }

        public async Task<User> GetEagerById(int id)
        {
            var response = await _httpClient.GetAsync($"api/users/{id}", HttpCompletionOption.ResponseHeadersRead);
            if (!response.IsSuccessStatusCode)
            {
                throw await ApiUtils.GetClientException(response.Content);
            }

            var user = JsonSerializer.Deserialize<UserDetailsDTO>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return new User {
                Email = user.Email,
                FirstName = user.FirstName,
                Id = user.Id,
                LastName = user.LastName,
                UserName = user.Username,
                UserRooms = user.Rooms.Select(u => new UserRoom() {RoomId = u.Id, UserId = user.Id}).ToList()
            };
        }

        public async Task<PagedList<User>> GetPaginated(PaginationDTO paginationParameters)
        {
            var response = await _httpClient.GetAsync($"api/users?{paginationParameters.AsQueryString()}", HttpCompletionOption.ResponseHeadersRead);
            if (!response.IsSuccessStatusCode)
            {
                throw await ApiUtils.GetClientException(response.Content);
            }

            var users = JsonSerializer.Deserialize<UserDTO[]>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var totalCount = int.Parse(response.Headers.GetValues("X-Total-Count").First());
            var currentPageNumber = int.Parse(response.Headers.GetValues("X-Current-Page").First());
            var pageSize = int.Parse(response.Headers.GetValues("X-Page-Size").First());

            return new PagedList<User>(users.Select(userDTO => new User
            {
                Id = userDTO.Id,
                UserName = userDTO.Username,
                Email = userDTO.Email,
                FirstName = userDTO.FirstName
            }).ToList(), totalCount, currentPageNumber, pageSize);

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