using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YASDM.Api.Services;
using YASDM.Model.DTO;
using YASDM.Model.Services;

namespace YASDM.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private IRoomService _roomService;

        public RoomsController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet]
        public async Task<IEnumerable<RoomDTO>> GetRoomsAsync([FromQuery] PaginationDTO paginationParameters)
        {
            var rooms = await _roomService.GetPaginated(paginationParameters);


            Response.Headers.Add("X-Total-Count", rooms.TotalCount.ToString());
            Response.Headers.Add("X-Total-Pages", rooms.TotalPages.ToString());
            Response.Headers.Add("X-Current-Page", rooms.CurrentPage.ToString());
            Response.Headers.Add("X-Page-Size", rooms.PageSize.ToString());
            Response.Headers.Add("X-Count", rooms.Count.ToString());


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
            var room = await _roomService.GetEagerById(id);

            if (room is null)
            {
                throw new ApiNotFoundException();
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

            var room = await _roomService.Create(roomDTO);

            roomDTO.Id = room.Id;

            return roomDTO;
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutRoomAsync(int id, [FromBody] RoomDTO roomDTO)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }

            await _roomService.Update(id, roomDTO);

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

            await _roomService.Delete(id);

            return Ok();
        }


    }
}