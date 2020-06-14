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
    public class RoomService : IRoomService
    {
        private HttpClient _httpClient;
        public RoomService(HttpClient httpCLient)
        {
            _httpClient = httpCLient;
        }

        public Task<Room> Create(RoomDTO roomDTO)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Room>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<Room> GetById(int id)
        {
            var response = await _httpClient.GetAsync($"api/rooms/{id}", HttpCompletionOption.ResponseHeadersRead);
            if (!response.IsSuccessStatusCode)
            {
                throw await ApiUtils.GetClientException(response.Content);
            }

            var room = JsonSerializer.Deserialize<RoomDTO>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return new Room {
                Id = room.Id,
                Name = room.Name,
                CreationDate = room.CreationDate,
                ScheduledDate = room.ScheduledDate
            };

        }

        public Task<Room> GetEagerById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedList<Room>> GetPaginated(PaginationDTO paginationParameters)
        {
            var response = await _httpClient.GetAsync($"api/rooms?{paginationParameters.AsQueryString()}", HttpCompletionOption.ResponseHeadersRead);
            if (!response.IsSuccessStatusCode)
            {
                throw await ApiUtils.GetClientException(response.Content);
            }

            var rooms = JsonSerializer.Deserialize<RoomDTO[]>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var totalCount = int.Parse(response.Headers.GetValues("X-Total-Count").First());
            var currentPageNumber = int.Parse(response.Headers.GetValues("X-Current-Page").First());
            var pageSize = int.Parse(response.Headers.GetValues("X-Page-Size").First());

            return new PagedList<Room>(rooms.Select(room => new Room {
                Id = room.Id,
                Name = room.Name,
                CreationDate = room.CreationDate,
                ScheduledDate = room.ScheduledDate
            }).ToList(), totalCount, currentPageNumber, pageSize);
        }

        public Task PartialUpdate(int id, List<PatchDTO> patches)
        {
            throw new NotImplementedException();
        }

        public Task Update(int id, RoomDTO roomDTO)
        {
            throw new NotImplementedException();
        }
    }
}