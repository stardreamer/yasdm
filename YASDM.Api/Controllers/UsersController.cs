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

    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private YASDMApiDbContext _db;

        public UsersController(YASDMApiDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IEnumerable<UserDTO>> GetUsersAsync()
        {
            var users = await _db.Users.ToListAsync();

            return users.Select(u => new UserDTO
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Username = u.UserName
            });
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDetailsDTO>> GetUserDetailedAsync(int id)
        {
            var user = await _db.Users.Where(u => u.Id == id).Include(u => u.UserRooms).SingleOrDefaultAsync();

            if (user is null)
            {
                return NotFound();
            }

            return new UserDetailsDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Rooms = user.UserRooms.Select(
                    ur => new RoomDTO
                    {
                        Id = ur.Room.Id,
                        CreationDate = ur.Room.CreationDate,
                        Name = ur.Room.Name,
                        ScheduledDate = ur.Room.ScheduledDate
                    }).ToList()
            };
        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> PostUser([FromBody] UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }

            var user = new User
            {
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                UserName = userDTO.Username

            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            userDTO.Id = user.Id;

            return userDTO;
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutUserAsync([FromBody] UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }

            var user = await _db.Users.Where(u => u.Id == userDTO.Id).SingleOrDefaultAsync();

            if (user is null)
            {
                return NotFound();
            }

            user.FirstName = userDTO.FirstName;
            user.LastName = userDTO.LastName;
            user.UserName = userDTO.Username;

            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUserAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }

            var user = await _db.Users.Where(u => u.Id == id).SingleOrDefaultAsync();

            if (user is null)
            {
                return NotFound();
            }

            _db.Users.Remove(user);

            await _db.SaveChangesAsync();

            return Ok();
        }

    }
}