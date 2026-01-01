using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ReclamationService.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }   // 🔑 lien avec AuthService
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }

        // Navigation (ne jamais exposer en JSON)
        [JsonIgnore]
        public ICollection<Reclamation> Reclamations { get; set; }
    }
}
