using MagicVilla.Models;

namespace MagicVilla.Repository.IRepository
{
    public interface IVillaRepository : IRepostiory<Villa>
    {
        Task<Villa> Update(Villa entidad);
    }
}
