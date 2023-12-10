using System.Linq.Expressions;

namespace MagicVilla.Repository.IRepository
{
    public interface IRepostiory<T> where T : class
    {
        Task Create(T entity);
        //si no se el envia un filtro, devuelve toda la lista
        Task<List<T>> GetAll(Expression<Func<T, bool>>? filter = null);
        Task<T> Get(Expression<Func<T, bool>>? filter = null, bool tracked = true);
        Task Remove(T entity);
        Task Save();
    }
}
