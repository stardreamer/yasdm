using System.Threading.Tasks;
using YASDM.Model.DTO;

namespace YASDM.Client.Services
{
    public interface IAuthService
    {
        Task<AuthDTO> Login(AuthRegisterDTO loginModel);
        Task Logout();
        Task<AuthDTO> Register(AuthRegisterDTO registerModel);
    }
}