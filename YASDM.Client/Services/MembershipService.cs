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

        public async Task Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/memberships/{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw await ApiUtils.GetClientException(response.Content);
            }
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

        public async Task<PagedList<UserRoom>> GetPaginated(PaginationDTO paginationParameters, MembershipSearchDTO searchDTO = null)
        {
            var response = await _httpClient.GetAsync($"api/memberships?{paginationParameters.AsQueryString()}&{searchDTO?.AsQueryString() ?? ""}", HttpCompletionOption.ResponseHeadersRead);
            if (!response.IsSuccessStatusCode)
            {
                throw await ApiUtils.GetClientException(response.Content);
            }

            var memberships = JsonSerializer.Deserialize<MembershipDTO[]>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var totalCount = int.Parse(response.Headers.GetValues("X-Total-Count").First());
            var currentPageNumber = int.Parse(response.Headers.GetValues("X-Current-Page").First());
            var pageSize = int.Parse(response.Headers.GetValues("X-Page-Size").First());

            return new PagedList<UserRoom>(memberships.Select(mDTO => new UserRoom
            {
                Id = mDTO.Id,
                RoomId = mDTO.RoomId,
                UserId = mDTO.UserId
            }).ToList(), totalCount, currentPageNumber, pageSize);
        }

        public Task Update(int id, MembershipDTO membershipDTO)
        {
            throw new NotImplementedException();
        }
    }
}