using ReclamationService.Models;

namespace ReclamationService.Services
{
    public interface IClientService
    {
        IEnumerable<Client> GetClients();
        Client GetClient(int id);
        void CreateClient(Client client);
    }
}
