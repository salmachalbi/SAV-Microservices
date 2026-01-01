using ReclamationService.Models;

namespace ReclamationService.Services
{
    public interface IReclamationService
    {
        IEnumerable<Reclamation> GetReclamations();
        Reclamation GetReclamation(int id);
        void CreateReclamation(Reclamation reclamation);
        void ChangeStatut(int id, string statut);
    }
}
