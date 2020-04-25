using System.Collections.Generic;
using System.Threading.Tasks;
using YASDM.Model;
using YASDM.Model.DTO;

namespace YASDM.Model.Services
{
    public interface IRoomService
    {
        Task<IEnumerable<Room>> GetAll();

        Task<IEnumerable<Room>> GetPaginated(PaginationDTO paginationParameters);
        Task<Room> GetById(int id);

        Task<Room> GetEagerById(int id);

        Task<Room> Create(RoomDTO roomDTO);

        Task Update(int id, RoomDTO roomDTO);

        Task Delete(int id);
    }
}