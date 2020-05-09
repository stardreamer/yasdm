using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YASDM.Api.Services;
using YASDM.Model;
using YASDM.Model.DTO;
using YASDM.Model.Services;

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
        public async Task<IEnumerable<UserDTO>> GetUsersAsync([FromQuery] PaginationDTO paginationParameters)
        {
            var users = await _userService.GetPaginated(paginationParameters);

            Response.Headers.Add("X-Total-Count", users.TotalCount.ToString());
            Response.Headers.Add("X-Total-Pages", users.TotalPages.ToString());
            Response.Headers.Add("X-Current-Page", users.CurrentPage.ToString());
            Response.Headers.Add("X-Page-Size", users.PageSize.ToString());
            Response.Headers.Add("X-Count", users.Count.ToString());

            return users.Select(u => u.AsDTO());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDetailsDTO>> GetUserDetailedAsync(int id)
        {
            var user = await _userService.GetEagerById(id);

            if (user is null)
            {
                throw new ApiNotFoundException();
            }

            return new UserDetailsDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.UserName,
                Email = user.Email,
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


            var user = await _userService.Create(registerDTO);

            return user.AsDTO();

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

            await _userService.UpdateUser(id, userDTO);

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

            await _userService.Delete(id);

            return Ok();
        }

    }
}