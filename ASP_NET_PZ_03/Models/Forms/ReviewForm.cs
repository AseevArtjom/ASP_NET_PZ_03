namespace ASP_NET_PZ_03.Models.Forms
{
    public class ReviewForm
    {
        public string? Text { get; set; }
        public int Rate { get; set; }
        public string? ClientName { get; set; }
        public string? ClientEmail { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
