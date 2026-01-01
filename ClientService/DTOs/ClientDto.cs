namespace ReclamationService.DTOs
{
    public class ClientDto
    {
        public int UserId { get; set; }   // 🔑 OBLIGATOIRE
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
    }
}
