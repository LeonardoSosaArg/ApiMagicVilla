using MagicVilla.Data;
using MagicVilla.Models;
using MagicVilla.Repository.IRepository;
using System.Linq.Expressions;

namespace MagicVilla.Repository
{
    public class VillaRepository : Repository<Villa>, IVillaRepository
    {
        private readonly ApplicationDbContext _db;
        public VillaRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }

        public async Task<Villa> Update(Villa entidad)
        {
            entidad.DateUpdated = DateTime.Now;
            _db.Villas.Update(entidad);
            await _db.SaveChangesAsync();
            return entidad;
        }
    }
}

