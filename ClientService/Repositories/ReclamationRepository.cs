using ReclamationService.Models;
using ReclamationService.Data;

namespace ReclamationService.Repositories
{
    public class ReclamationRepository : IReclamationRepository
    {
        private readonly AppDbContext _context;

        public ReclamationRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Reclamation> GetAll()
            => _context.Reclamations.ToList();

        public Reclamation GetById(int id)
            => _context.Reclamations.Find(id);

        public void Add(Reclamation reclamation)
            => _context.Reclamations.Add(reclamation);

        public void Update(Reclamation reclamation)
            => _context.Reclamations.Update(reclamation);

        public void Delete(int id)
        {
            var rec = GetById(id);
            if (rec != null)
                _context.Reclamations.Remove(rec);
        }

        public void Save()
            => _context.SaveChanges();
    }
}

