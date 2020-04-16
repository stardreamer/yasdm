using System.Collections.Generic;
using System.Threading.Tasks;
using YASDM.Model;
using YASDM.Model.DTO;

namespace YASDM.Api.Services
{
    public interface IUserService
    {
        Task<User> Authenticate(string username, string password);
        Task<IEnumerable<User>> GetAll();

        Task<IEnumerable<User>> GetPaginated(PaginationDTO paginationParameters);
        Task<User> GetById(int id);

        Task<User> GetEagerById(int id);

        Task<User> Create(AuthRegisterDTO registerDTO);
        Task UpdateCredentials(int id, AuthRegisterDTO updateDTO);

        Task UpdateUser(int id, UserDTO updateDTO);
        Task Delete(int id);
    }
}