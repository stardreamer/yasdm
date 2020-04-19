using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YASDM.Model;
using YASDM.Model.DTO;

namespace YASDM.Api.Services
{
    public class MembershipService : IMembershipService
    {
        private YASDMApiDbContext _db;

        public MembershipService(YASDMApiDbContext db)
        {
            _db = db;
        }


        public async Task<UserRoom> Create(MembershipDTO membershipDTO)
        {
            var ur = new UserRoom
            {
                UserId = membershipDTO.UserId,
                RoomId = membershipDTO.RoomId
            };
            _db.UserRooms.Add(ur);
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new ApiException($"Possible cause: there might already exist a link between user(id: {membershipDTO.UserId}) and room(id: {membershipDTO.RoomId})");
            }

            return ur;
        }

        public async Task Delete(int id)
        {
            var ur = await _db.UserRooms.Where(u => u.Id == id).SingleOrDefaultAsync();

            if (ur is null)
            {
                throw new ApiNotFoundException();
            }

            _db.UserRooms.Remove(ur);

            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserRoom>> GetAll()
        {
            return await _db.UserRooms.ToListAsync();
        }

        public async Task<UserRoom> GetById(int id)
        {
            return await _db.UserRooms.FindAsync(id);
        }

        public async Task<UserRoom> GetEagerById(int id)
        {
            var ur = await _db.UserRooms.Where(r => r.Id == id).Include(r => r.User).Include(r => r.Room).SingleOrDefaultAsync();

            if (ur is null)
            {
                throw new ApiNotFoundException();
            }


            return ur;
        }

        public async Task<IEnumerable<UserRoom>> GetPaginated(PaginationDTO paginationParameters)
        {
            return await _db.UserRooms.ToPagedListAsync(u => u.Id, paginationParameters.PageNumber, paginationParameters.PageSize);
        }

        public async Task Update(int id, MembershipDTO membershipDTO)
        {
            var ur = await _db.UserRooms.Where(u => u.Id == id).SingleOrDefaultAsync();

            if (ur is null)
            {
                throw new ApiNotFoundException();
            }

            ur.RoomId = membershipDTO.RoomId;
            ur.UserId = membershipDTO.UserId;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new ApiException($"Possible cause: there might already exist a link between user(id: {membershipDTO.UserId}) and room(id: {membershipDTO.RoomId})");
            }
        }
    }
}