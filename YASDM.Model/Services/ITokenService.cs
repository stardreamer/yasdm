using System.Threading.Tasks;
using YASDM.Model.DTO;

namespace YASDM.Model.Services
{
    public interface ITokenService
    {
        string GetUserIdFromExpiredToken(string token);

        Task<RefreshTokenDTO>  RefreshTokens(RefreshTokenDTO tokens);

        Task<RefreshTokenDTO> GetTokens(string userId);

        Task RevokeRefreshToken(RefreshTokenDTO tokens);
    }
}