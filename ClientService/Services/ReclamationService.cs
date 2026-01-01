using ReclamationService.Models;
using ReclamationService.Repositories;

namespace ReclamationService.Services
{
    public class ReclamationService : IReclamationService
    {
        private readonly IReclamationRepository _repository;

        public ReclamationService(IReclamationRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Reclamation> GetReclamations()
            => _repository.GetAll();

        public Reclamation GetReclamation(int id)
            => _repository.GetById(id);

        public void CreateReclamation(Reclamation reclamation)
        {
            _repository.Add(reclamation);
            _repository.Save();
        }

        public void ChangeStatut(int id, string statut)
        {
            var rec = _repository.GetById(id);
            if (rec != null)
            {
                rec.Statut = statut;
                _repository.Update(rec);
                _repository.Save();
            }
        }
    }
}