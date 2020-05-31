using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YASDM.Model;
using YASDM.Model.DTO;
using YASDM.Model.Services;

namespace YASDM.Api.Services
{
    public class UserPairService : IUserPairService
    {
        private YASDMApiDbContext _db;

        public UserPairService(YASDMApiDbContext db)
        {
            _db = db;
        }


        public async Task Delete(int id)
        {
            var up = await _db.UserPairs.Where(u => u.Id == id).SingleOrDefaultAsync();

            if (up is null)
            {
                throw new ApiNotFoundException();
            }

            _db.UserPairs.Remove(up);

            await _db.SaveChangesAsync();
        }

        public async Task<UserPair> GetEagerById(int id)
        {
            var up = await _db.UserPairs.Where(r => r.Id == id).Include(r => r.Room).SingleOrDefaultAsync();

            if (up is null)
            {
                throw new ApiNotFoundException();
            }


            return up;
        }


        public async Task<PagedList<UserPair>> GetPaginated(PaginationDTO paginationParameters, UserPairSearchDTO searchDTO = null)
        {
            if (searchDTO is null || (searchDTO.RoomId is null) && (searchDTO.User1Id is null) && (searchDTO.User2Id is null) && (string.IsNullOrWhiteSpace(searchDTO.User1Alias)) && (string.IsNullOrWhiteSpace(searchDTO.User2Alias)))
            {
                return await _db.UserPairs.ToPagedListAsync(u => u.Id, paginationParameters.PageNumber, paginationParameters.PageSize);
            }
            var exp = _db.UserPairs.AsQueryable();
            if (!(searchDTO.RoomId is null))
            {
                exp = exp.Where(ur => ur.RoomId == searchDTO.RoomId.Value);
            }

            if (!(searchDTO.User1Id is null))
            {
                exp = exp.Where(ur => ur.User1Id == searchDTO.User1Id.Value);
            }

            if (!(searchDTO.User2Id is null))
            {
                exp = exp.Where(ur => ur.User2Id == searchDTO.User2Id.Value);
            }

            if (!(string.IsNullOrWhiteSpace(searchDTO.User1Alias)))
            {
                exp = exp.Where(ur => ur.User1Alias == searchDTO.User1Alias);
            }

            if (!(string.IsNullOrWhiteSpace(searchDTO.User2Alias)))
            {
                exp = exp.Where(ur => ur.User2Alias == searchDTO.User2Alias);
            }

            return await exp.ToPagedListAsync(u => u.Id, paginationParameters.PageNumber, paginationParameters.PageSize);
        }

        public async Task Update(int id, UserPairDTO userPairDTO)
        {
            var ur = await _db.UserPairs.Where(u => u.Id == id).SingleOrDefaultAsync();

            if (ur is null)
            {
                throw new ApiNotFoundException();
            }

            ur.RoomId = userPairDTO.RoomId;
            ur.User1Id = userPairDTO.User1Id;
            ur.User2Id = userPairDTO.User2Id;
            ur.User1Alias = userPairDTO.User1Alias;
            ur.User2Alias = userPairDTO.User1Alias;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new ApiException($"Possible cause: there might already exist a link between user(id: {userPairDTO.User1Id}) or user(id: {userPairDTO.User2Id}) and room(id: {userPairDTO.RoomId})");
            }
        }

        public async Task<IEnumerable<UserPair>> GetAll()
        {
            return await _db.UserPairs.ToListAsync();
        }


        public async Task<UserPair> Create(UserPairDTO userPairDTO)
        {
            var up = new UserPair
            {
                User1Id = userPairDTO.User1Id,
                RoomId = userPairDTO.RoomId,
                User2Id = userPairDTO.User2Id,
                User1Alias = userPairDTO.User1Alias,
                User2Alias = userPairDTO.User2Alias
            };
            _db.UserPairs.Add(up);
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new ApiException($"Possible cause: there might already exist a link between user(id: {userPairDTO.User1Id}) and room(id: {userPairDTO.RoomId}) or between user(id: {userPairDTO.User2Id}) and room(id: {userPairDTO.RoomId})");
            }

            return up;
        }

        public async Task<UserPair> GetById(int id)
        {
            var up = await _db.UserPairs.FindAsync(id);

            if (up is null)
            {
                throw new ApiNotFoundException();
            }


            return up;
        }
    }
}