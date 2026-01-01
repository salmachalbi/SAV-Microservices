namespace ArticleService.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public DateTime DateAchat { get; set; }
        public int GarantieMois { get; set; }
    }
}
