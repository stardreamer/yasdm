using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using YASDM.Model.DTO;
using System.Security.Cryptography;
using YASDM.Model;
using System.Threading.Tasks;

namespace YASDM.Api.Services
{
    public class TokenService : ITokenService
    {
        private IConfiguration _configuration;
        private YASDMApiDbContext _db;
        
        public TokenService(YASDMApiDbContext db, IConfiguration configuration)
        {
            _configuration = configuration;
            _db = db;
        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Secret"])),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new ApiException("Invalid token");

            return principal;
        }
        public string GetUserIdFromExpiredToken(string token)
        {
            return GetPrincipalFromExpiredToken(token).Identity.Name;
        }

        public async Task<RefreshTokenDTO> GetTokens(string userId)
        {
            var expirationDate = DateTime.UtcNow.AddDays(7);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userId)
                }),
                Expires = expirationDate,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            var refreshToken = GenerateRefreshToken();

            var user = await _db.Users.FindAsync(int.Parse(userId));

            if (user is null)
            {
                throw new ApiException("There is not a single user associated with this token");
            }

            await _db.RefreshTokens.AddAsync(new RefreshToken { Token = refreshToken, UserId = user.Id, ExpirationDate = expirationDate.AddDays(1) });
            await _db.SaveChangesAsync();


            return new RefreshTokenDTO
            {
                Token = tokenString,
                RefreshToken = refreshToken
            };
        }

        public async Task<RefreshTokenDTO> RefreshTokens(RefreshTokenDTO tokens)
        {
            var userId = GetUserIdFromExpiredToken(tokens.Token);
            var refreshToken = tokens.RefreshToken;
            var savedToken = await _db.RefreshTokens.SingleOrDefaultAsync(rt => rt.Token == refreshToken);

            if (savedToken == null)
            {
                throw new ApiException("Refresh token was not found");
            }

            if (DateTime.UtcNow > savedToken.ExpirationDate)
            {
                await RemoveToken(savedToken);
                throw new SecurityTokenExpiredException();
            }

            var realUserId = int.Parse(userId);

            if (realUserId != savedToken.UserId)
            {
                throw new ApiException("This refresh token doesn't correspond with your access token");
            }

            await RemoveToken(savedToken);

            return await GetTokens(userId);
        }

        private async Task RemoveToken(RefreshToken token)
        {
            _db.RefreshTokens.Remove(token);
            await _db.SaveChangesAsync();
        }

        public async Task RevokeRefreshToken(RefreshTokenDTO tokens)
        {
            var userId = GetUserIdFromExpiredToken(tokens.Token);
            var refreshToken = tokens.RefreshToken;

            var savedToken = await _db.RefreshTokens.SingleOrDefaultAsync(rt => rt.Token == refreshToken);

            if (savedToken == null)
            {
                throw new ApiException("Refresh token was not found");
            }

            var realUserId = int.Parse(userId);

            if (realUserId != savedToken.UserId)
            {
                throw new ApiException("This refresh token doesn't correspond with your access token");
            }

            await RemoveToken(savedToken);
        }
    }
}