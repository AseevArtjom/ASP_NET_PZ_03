namespace ASP_NET_PZ_03.Models
{
    public class InfoSkill
    {
        public int Id { get; set; }
        public virtual Skill Skill { get; set; }
        public int Level { get; set; }
    }
}
