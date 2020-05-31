using System.Collections.Generic;
using System.Threading.Tasks;
using YASDM.Model.DTO;

namespace YASDM.Model.Services
{
    public interface IUserPairService
    {
        Task<IEnumerable<UserPair>> GetAll();

        Task<PagedList<UserPair>> GetPaginated(PaginationDTO paginationParameters, UserPairSearchDTO searchParams = null);
        Task<UserPair> GetById(int id);

        Task<UserPair> GetEagerById(int id);

        Task<UserPair> Create(UserPairDTO userPairDTO);

        Task Update(int id, UserPairDTO userPairDTO);

        Task Delete(int id);
    }
}