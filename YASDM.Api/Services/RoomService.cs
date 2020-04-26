using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YASDM.Model;
using YASDM.Model.DTO;
using YASDM.Model.Services;

namespace YASDM.Api.Services
{
    public class RoomService : IRoomService
    {
        private YASDMApiDbContext _db;

        public RoomService(YASDMApiDbContext db)
        {
            _db = db;
        }

        public async Task<Room> Create(RoomDTO roomDTO)
        {
            var room = new Room
            {
                CreationDate = roomDTO.CreationDate,
                Name = roomDTO.Name,
                ScheduledDate = roomDTO.ScheduledDate
            };
            _db.Rooms.Add(room);
            await _db.SaveChangesAsync();

            return room;
        }

        public async Task Delete(int id)
        {
            var room = await _db.Rooms.Where(u => u.Id == id).SingleOrDefaultAsync();

            if (room is null)
            {
                throw new ApiNotFoundException();
            }

            _db.Rooms.Remove(room);

            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Room>> GetAll()
        {
            return await _db.Rooms.ToListAsync();

        }

        public async Task<Room> GetById(int id)
        {
            return await _db.Rooms.FindAsync(id);
        }

        public async Task<Room> GetEagerById(int id)
        {
            var room = await _db.Rooms.Where(r => r.Id == id).Include(u => u.UserRooms).SingleOrDefaultAsync();

            if (room is null)
            {
                throw new ApiNotFoundException();
            }

            return room;
        }

        public async Task<PagedList<Room>> GetPaginated(PaginationDTO paginationParameters)
        {
            return await _db.Rooms.ToPagedListAsync(u => u.Id, paginationParameters.PageNumber, paginationParameters.PageSize);
        }

        public async Task Update(int id, RoomDTO roomDTO)
        {
            var room = await _db.Rooms.Where(u => u.Id == id).SingleOrDefaultAsync();

            if (room is null)
            {
                throw new ApiNotFoundException();
            }

            room.Name = roomDTO.Name;
            room.CreationDate = roomDTO.CreationDate;
            room.ScheduledDate = roomDTO.ScheduledDate;

            await _db.SaveChangesAsync();
        }
    }
}