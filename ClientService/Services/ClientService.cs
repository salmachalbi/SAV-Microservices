using ReclamationService.Models;
using ReclamationService.Repositories;

namespace ReclamationService.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _repository;

        public ClientService(IClientRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Client> GetClients() => _repository.GetAll();

        public Client GetClient(int id) => _repository.GetById(id);

        public void CreateClient(Client client)
        {
            _repository.Add(client);
            _repository.Save();
        }
    }
}
