using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YASDM.Model;
using YASDM.Model.DTO;
using YASDM.Model.Services;

namespace YASDM.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserPairsController : ControllerBase
    {
        private IUserPairService _userpairService;

        public UserPairsController(IUserPairService userpairService)
        {
            _userpairService = userpairService;
        }

        [HttpGet]
        public async Task<IEnumerable<UserPairDTO>> GetMembershipsAsync([FromQuery] PaginationDTO paginationParameters, [FromQuery] UserPairSearchDTO searchDTO)
        {
            var realSearchDTO = searchDTO ?? new UserPairSearchDTO();

            realSearchDTO.User1Id = int.Parse(User.Identity.Name);
            

            var urs = await _userpairService.GetPaginated(paginationParameters, realSearchDTO);

            Response.Headers.Add("X-Total-Count", urs.TotalCount.ToString());
            Response.Headers.Add("X-Total-Pages", urs.TotalPages.ToString());
            Response.Headers.Add("X-Current-Page", urs.CurrentPage.ToString());
            Response.Headers.Add("X-Page-Size", urs.PageSize.ToString());
            Response.Headers.Add("X-Count", urs.Count.ToString());

            return urs.Select(ur => new UserPairDTO
            {
                Id = ur.Id,
                User1Id = ur.User1Id,
                User2Id = -1,
                User1Alias = ur.User1Alias,
                User2Alias = ur.User2Alias,
                RoomId = ur.RoomId
            });
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserPairDTO>> GetMembershipDetailedAsync(int id)
        {
            var ur = await _userpairService.GetEagerById(id);

            var userId = int.Parse(User.Identity.Name);

            if(userId != ur.User1Id)
                return Unauthorized();

            return new UserPairDTO
            {
                Id = ur.Id,
                User1Id = ur.User1Id,
                User2Id = ur.User2Id,
                User1Alias = ur.User1Alias,
                User2Alias = ur.User2Alias,
                RoomId = ur.RoomId
            };
        }

    }
}