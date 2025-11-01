namespace AlzheimerApp.Data.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } // Basit örnek için düz metin, gerçek projede hash kullanmalısın
    }
}
