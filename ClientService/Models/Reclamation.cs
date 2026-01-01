using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace ReclamationService.Models

{
    public class Reclamation
    {
        [Key]
        public int Id { get; set; }

        public string Description { get; set; }
        public DateTime DateReclamation { get; set; } = DateTime.Now;
        public string Statut { get; set; } = "En attente";

        // 🔗 Clé étrangère
        public int ClientId { get; set; }

        [JsonIgnore]
        public Client Client { get; set; }



        // 🔗 Article (externe – ArticleService)
        public int ArticleId { get; set; }
    }
}
