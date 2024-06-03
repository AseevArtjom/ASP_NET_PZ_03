using Microsoft.EntityFrameworkCore;

namespace ASP_NET_PZ_03.Models
{
	public class SiteContext : DbContext
	{
		public SiteContext(DbContextOptions<SiteContext> options) : base(options) { }

		public virtual DbSet<ImageFile> ImageFiles { get; set; }

		public virtual DbSet<Info> Infos { get; set; }

		public virtual DbSet<Profession> Professions { get; set; }

		public virtual DbSet<InfoSkill> InfoSkills { get; set; }

		public virtual DbSet<Skill> Skills { get; set; }
	}
}
