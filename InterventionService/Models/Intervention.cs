namespace InterventionService.Models
{
    public class Intervention
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        public int ArticleId { get; set; }

        public DateTime DateIntervention { get; set; }

        public bool SousGarantie { get; set; }

        public decimal CoutPieces { get; set; }
        public decimal CoutMainOeuvre { get; set; }
        public decimal MontantFacture { get; set; }
    }
}
