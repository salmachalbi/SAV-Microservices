using ReclamationService.Models;

namespace ReclamationService.Repositories
{
    public interface IClientRepository
    {
        IEnumerable<Client> GetAll();
        Client GetById(int id);
        void Add(Client client);
        void Save();
    }
}
