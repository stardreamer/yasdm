using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
using YASDM.Api.Services;
using YASDM.Model.DTO;



namespace YASDM.Api.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private IUserService _userService;
        private ITokenService _tokenService;
        public TokenController(IUserService userService, ITokenService refreshService)
        {
            _userService = userService;
            _tokenService = refreshService;
        }

        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AuthDTO>> GetToken([FromBody]AuthRegisterDTO loginDTO)
        {

            var user = await _userService.Authenticate(loginDTO.Username, loginDTO.Password);

            if (user == null)
                return new ObjectResult("Username or password is incorrect") { StatusCode = StatusCodes.Status500InternalServerError };

            var tokens = await _tokenService.GetTokens(user.Id.ToString());

            // return basic user info and authentication token
            return new AuthDTO
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Token = tokens.Token,
                RefreshToken = tokens.RefreshToken
            };

        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<RefreshTokenDTO>> Refresh([FromBody]RefreshTokenDTO refreshTokenDTO)
        {

            var tokens = await _tokenService.RefreshTokens(refreshTokenDTO);

            // return basic user info and authentication token
            return new RefreshTokenDTO
            {
                Token = tokens.Token,
                RefreshToken = tokens.RefreshToken
            };

        }

        [HttpPost("revokeRefreshToken")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> RevokeRefresh([FromBody]RefreshTokenDTO refreshTokenDTO)
        {

            var tokens = await _tokenService.RefreshTokens(refreshTokenDTO);

            // return basic user info and authentication token
            return Ok();

        }

    }
}