using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
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
        private IConfiguration _configuration;
        public TokenController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
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

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info and authentication token
            return new AuthDTO
            {
                Id = user.Id,
                Username = user.UserName,
                Token = tokenString
            };
        }

    }
}