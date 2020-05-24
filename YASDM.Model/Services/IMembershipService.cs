using System.Collections.Generic;
using System.Threading.Tasks;
using YASDM.Model.DTO;

namespace YASDM.Model.Services
{
    public interface IMembershipService
    {
        Task<IEnumerable<UserRoom>> GetAll();

        Task<PagedList<UserRoom>> GetPaginated(PaginationDTO paginationParameters, MembershipSearchDTO searchParams = null);
        Task<UserRoom> GetById(int id);

        Task<UserRoom> GetEagerById(int id);

        Task<UserRoom> Create(MembershipDTO membershipDTO);

        Task Update(int id, MembershipDTO membershipDTO);

        Task Delete(int id);
    }
}