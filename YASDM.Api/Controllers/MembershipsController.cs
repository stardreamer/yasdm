using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class MembershipsController : ControllerBase
    {
        private IMembershipService _membershipService;

        public MembershipsController(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }

        [HttpGet]
        public async Task<IEnumerable<MembershipDTO>> GetMembershipsAsync([FromQuery] PaginationDTO paginationParameters, [FromQuery] MembershipSearchDTO searchDTO)
        {
            var urs = await _membershipService.GetPaginated(paginationParameters, searchDTO);

             Response.Headers.Add("X-Total-Count", urs.TotalCount.ToString());
            Response.Headers.Add("X-Total-Pages", urs.TotalPages.ToString());
            Response.Headers.Add("X-Current-Page", urs.CurrentPage.ToString());
            Response.Headers.Add("X-Page-Size", urs.PageSize.ToString());
            Response.Headers.Add("X-Count", urs.Count.ToString());

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
            var ur = await _membershipService.GetEagerById(id);

            return new MembershipDetailDTO
            {
                Id = ur.Id,
                RoomId = ur.RoomId,
                Room = ur.Room.AsDTO(),
                UserId = ur.UserId,
                User = ur.User.AsDTO()

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

            var ur = await _membershipService.Create(membershipDTO);


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

            await _membershipService.Delete(id);

            return Ok();
        }

    }
}