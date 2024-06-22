namespace ASP_NET_PZ_03.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public int Rate { get; set; }
        public string? ClientName { get; set; }
        public string? ClientEmail { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}
