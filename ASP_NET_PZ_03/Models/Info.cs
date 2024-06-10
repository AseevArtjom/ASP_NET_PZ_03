

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ASP_NET_PZ_03.Models
{
    public class Info
    {
        public Info()
        {
            Skills = new List<InfoSkill>();
            Images = new List<ImageFile>();
        }

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string City { get; set; }

        public string Description { get; set; }

        public int Age { get; set; }

        public bool Busy { get; set; }

        public DateTime BirthDay { get; set; }

        public virtual Profession? Profession { get; set; }

        public virtual ICollection<InfoSkill> Skills { get; set; }

        public virtual ICollection<ImageFile> Images { get; set; }
    }
}
