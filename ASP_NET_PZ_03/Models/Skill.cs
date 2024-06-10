namespace ASP_NET_PZ_03.Models
{
    public class Skill
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public virtual ImageFile? Image { get; set; }
    }
}
 