

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ASP_NET_PZ_03.Models
{
    public class Info
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string City { get; set; }

        public string Description { get; set; }

        public int Age { get; set; }

        public bool Busy { get; set; }

        public DateTime BirthDay { get; set; }

        public int ProfessionId { get; set; }
        public Profession? Profession { get; set; }

        public List<InfoSkill> Skills { get; set; }

        public string? ImageSrc { get; set; }

        public List<ImageFile>? Images { get; set; }
    }
}
