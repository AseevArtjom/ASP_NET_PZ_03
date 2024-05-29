using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ASP_NET_PZ_03.Models.Forms
{
	public class SkillForm
	{
		[DisplayName("Название навыка")]
		public string Title { get; set; }

		[DisplayName("Лого")]
		public IFormFile? Image { get; set; }
	}
}
