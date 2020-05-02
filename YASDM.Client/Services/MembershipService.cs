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
    public class MembershipService : IMembershipService
    {
        private HttpClient _httpClient;
        public MembershipService(HttpClient httpCLient)
        {
            _httpClient = httpCLient;
        }

        public async Task<UserRoom> Create(MembershipDTO membershipDTO)
        {
            var result = await _httpClient.PostAsJsonAsync<MembershipDTO>("api/memberships", membershipDTO);
            if (!result.IsSuccessStatusCode)
            {
                throw await ApiUtils.GetClientException(result.Content);
            }

            var mDTO = await result.Content.ReadFromJsonAsync<MembershipDTO>();

            return new UserRoom {
                Id = mDTO.Id,
                RoomId = mDTO.RoomId,
                UserId = mDTO.UserId
            };
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserRoom>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<UserRoom> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<UserRoom> GetEagerById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedList<UserRoom>> GetPaginated(PaginationDTO paginationParameters)
        {
            throw new NotImplementedException();
        }

        public Task Update(int id, MembershipDTO membershipDTO)
        {
            throw new NotImplementedException();
        }
    }
}