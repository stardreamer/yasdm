using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YASDM.Model;
using YASDM.Model.DTO;

namespace YASDM.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MembershipsController : ControllerBase
    {
        private YASDMApiDbContext _db;

        public MembershipsController(YASDMApiDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IEnumerable<MembershipDTO>> GetMembershipsAsync()
        {
            var urs = await _db.UserRooms.ToListAsync();

            return urs.Select(ur => new MembershipDTO
            {
                Id = ur.Id,
                UserId = ur.UserId,
                RoomId = ur.RoomId
            });
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MembershipDetailDTO>> GetMembershipDetailedAsync(int id)
        {
            var ur = await _db.UserRooms.Where(r => r.Id == id).Include(r => r.User).Include(r => r.Room).SingleOrDefaultAsync();

            if (ur is null)
            {
                throw new ApiNotFoundException();
            }

            return new MembershipDetailDTO
            {
                Id = ur.Id,
                RoomId = ur.RoomId,
                Room = new RoomDTO
                {
                    Id = ur.Room.Id,
                    Name = ur.Room.Name,
                    CreationDate = ur.Room.CreationDate,
                    ScheduledDate = ur.Room.ScheduledDate
                },
                UserId = ur.UserId,
                User = new UserDTO
                {
                    Id = ur.User.Id,
                    FirstName = ur.User.FirstName,
                    LastName = ur.User.LastName,
                    Username = ur.User.UserName
                }

            };
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MembershipDTO>> PostMembership([FromBody] MembershipDTO membershipDTO)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }

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


            membershipDTO.Id = ur.Id;

            return membershipDTO;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteMembershipAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }

            var ur = await _db.UserRooms.Where(u => u.Id == id).SingleOrDefaultAsync();

            if (ur is null)
            {
                throw new ApiNotFoundException();
            }

            _db.UserRooms.Remove(ur);

            await _db.SaveChangesAsync();

            return Ok();
        }

    }
}