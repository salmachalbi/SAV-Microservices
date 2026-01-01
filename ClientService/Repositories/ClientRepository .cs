using ReclamationService.Models;
using ReclamationService.Data;

namespace ReclamationService.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly AppDbContext _context;

        public ClientRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Client> GetAll() => _context.Clients.ToList();

        public Client GetById(int id) => _context.Clients.Find(id);

        public void Add(Client client) => _context.Clients.Add(client);

        public void Save() => _context.SaveChanges();
    }
}

