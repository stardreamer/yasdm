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
    public class RoomsController : ControllerBase
    {
        private YASDMApiDbContext _db;

        public RoomsController(YASDMApiDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IEnumerable<RoomDTO>> GetRoomsAsync()
        {
            var rooms = await _db.Rooms.ToListAsync();

            return rooms.Select(r => new RoomDTO
            {
                Id = r.Id,
                Name = r.Name,
                CreationDate = r.CreationDate,
                ScheduledDate = r.ScheduledDate
            });
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RoomDetailsDTO>> GetRoomDetailedAsync(int id)
        {
            var room = await _db.Rooms.Where(r => r.Id == id).Include(u => u.UserRooms).SingleOrDefaultAsync();

            if (room is null)
            {
                return NotFound();
            }

            return new RoomDetailsDTO
            {
                Id = room.Id,
                CreationDate = room.CreationDate,
                Name = room.Name,
                ScheduledDate = room.ScheduledDate,
                Users = room.UserRooms.Select(
                    ur => new UserDTO
                    {
                        Id = ur.User.Id,
                        FirstName = ur.User.FirstName,
                        LastName = ur.User.LastName,
                        Username = ur.User.UserName,
                        Email = ur.User.Email
                    }).ToList()
            };
        }

        [HttpPost]
        public async Task<ActionResult<RoomDTO>> PostUser([FromBody] RoomDTO roomDTO)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }

            var room = new Room
            {
                CreationDate = roomDTO.CreationDate,
                Name = roomDTO.Name,
                ScheduledDate = roomDTO.ScheduledDate
            };
            _db.Rooms.Add(room);
            await _db.SaveChangesAsync();

            roomDTO.Id = room.Id;

            return roomDTO;
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutRoomAsync([FromBody] RoomDTO roomDTO)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }

            var room = await _db.Rooms.Where(u => u.Id == roomDTO.Id).SingleOrDefaultAsync();

            if (room is null)
            {
                return NotFound();
            }

            room.Name = roomDTO.Name;
            room.CreationDate = roomDTO.CreationDate;
            room.ScheduledDate = roomDTO.ScheduledDate;

            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRoomAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }

            var room = await _db.Rooms.Where(u => u.Id == id).SingleOrDefaultAsync();

            if (room is null)
            {
                return NotFound();
            }

            _db.Rooms.Remove(room);

            await _db.SaveChangesAsync();

            return Ok();
        }


    }
}