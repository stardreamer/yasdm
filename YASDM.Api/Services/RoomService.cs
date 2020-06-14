using System;
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

        public async Task PartialUpdate(int id, List<PatchDTO> patches)
        {
            var room = await _db.Rooms.Include(r => r.UserRooms).Include(r => r.UserPairs).Where(u => u.Id == id).SingleOrDefaultAsync();

            if (room is null)
            {
                throw new ApiNotFoundException();
            }

            foreach (var patch in patches)
            {
                if (string.Equals(patch.PropertyName, nameof(room.State), StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        var value = (RoomState)Enum.Parse(typeof(RoomState), patch.PropertyValue);

                        room.State = value;

                        if (value == RoomState.Closed)
                        {
                            var members = room.UserRooms.Select(ur => ur.UserId).ToList();

                            members.Shuffle();

                            var membersCopy = members.ToList();
                            var first = membersCopy[0];
                            membersCopy.RemoveAt(0);
                            membersCopy.Add(first);

                            _db.UserPairs.RemoveRange(room.UserPairs);

                            await _db.UserPairs.AddRangeAsync(
                                members.Zip(membersCopy)
                                .Select(
                                    lr => new UserPair
                                    {
                                        RoomId = room.Id,
                                        User1Id = lr.First,
                                        User2Id = lr.Second,
                                        User1Alias = members.IndexOf(lr.First).ToString(),
                                        User2Alias = members.IndexOf(lr.Second).ToString(),
                                    }
                                )
                            );

                            await _db.SaveChangesAsync();

                        }
                    }
                    catch
                    {
                        throw new ApiException($"Unknown state {patch.PropertyValue}");
                    }
                }
            }
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