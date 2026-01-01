using ReclamationService.Models;

namespace ReclamationService.Repositories
{
    public interface IReclamationRepository
    {
        IEnumerable<Reclamation> GetAll();
        Reclamation GetById(int id);
        void Add(Reclamation reclamation);
        void Update(Reclamation reclamation);
        void Delete(int id);
        void Save();
    }
}
