using System.Collections.Generic;
using System.Threading.Tasks;
using YASDM.Model;
using YASDM.Model.DTO;

namespace YASDM.Api.Services
{
    public interface IMembershipService
    {
        Task<IEnumerable<UserRoom>> GetAll();

        Task<IEnumerable<UserRoom>> GetPaginated(PaginationDTO paginationParameters);
        Task<UserRoom> GetById(int id);

        Task<UserRoom> GetEagerById(int id);

        Task<UserRoom> Create(MembershipDTO membershipDTO);

        Task Update(int id, MembershipDTO membershipDTO);

        Task Delete(int id);
    }
}