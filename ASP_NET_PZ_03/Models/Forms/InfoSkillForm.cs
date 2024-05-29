using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace ASP_NET_PZ_03.Models.Forms
{
    public class InfoSkillForm
    {
        [DisplayName("Навык")]
        public int SkillId { get; set; }
        [DisplayName("Уровень владения")]
        public int Level { get; set; }
    }
}
