using System.ComponentModel;

namespace ASP_NET_PZ_03.Models.Forms
{
    public class ProfessionForm
    {
        [DisplayName("Название")]
        public string Name { get; set; }

        [DisplayName("Лого")]
        public IFormFile? Image { get; set; }
    }
}
