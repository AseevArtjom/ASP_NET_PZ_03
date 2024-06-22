using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ASP_NET_PZ_03.Models
{
	public class SiteContext : IdentityDbContext<User,IdentityRole<int>,int>
	{
		public SiteContext(DbContextOptions<SiteContext> options) : base(options) { }

		public virtual DbSet<ImageFile> ImageFiles { get; set; }

		public virtual DbSet<Info> Infos { get; set; }

		public virtual DbSet<Profession> Professions { get; set; }

		public virtual DbSet<InfoSkill> InfoSkills { get; set; }

		public virtual DbSet<Skill> Skills { get; set; }

		public virtual DbSet<Review> Reviews { get; set; }
	}
}
