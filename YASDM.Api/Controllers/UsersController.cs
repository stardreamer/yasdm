using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YASDM.Api.Services;
using YASDM.Model.DTO;

namespace YASDM.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IEnumerable<UserDTO>> GetUsersAsync()
        {
            var users = await _userService.GetAll();

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
            var user = await _userService.GetEagerById(id);

            if (user is null)
            {
                return NotFound();
            }

            return new UserDetailsDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.UserName,
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


        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDTO>> PostUser([FromBody] AuthRegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }

            try
            {
                var user = await _userService.Create(registerDTO);

                return new UserDTO
                {
                    Id = user.Id,
                    Username = user.UserName
                };
            }
            catch (ApiException apiException)
            {
                return new ObjectResult(apiException.Message) { StatusCode = StatusCodes.Status500InternalServerError };
            }
            catch
            {
                return new ObjectResult("Unexpected Error") { StatusCode = StatusCodes.Status500InternalServerError };
            }

        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutUserAsync(int id, [FromBody] UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }

            try
            {
                await _userService.UpdateUser(id, userDTO);
            }
            catch (KeyNotFoundException)
            {
                NotFound();
            }

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

            try
            {
                await _userService.Delete(id);
            }
            catch (KeyNotFoundException)
            {
                NotFound();
            }

            return Ok();
        }

    }
}