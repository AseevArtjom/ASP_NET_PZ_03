using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ASP_NET_PZ_03.Models.Forms
{
    public class InfoForm
    {
        public InfoForm() { }
        public InfoForm(Info model)
        {
            FirstName = model.FirstName;
            LastName = model.LastName;
            City = model.City;
            Description = model.Description;
            Age = model.Age;
            Busy = model.Busy;
            BirthDay = model.BirthDay;
            ProfessionId = model.ProfessionId;
        }

        public void Fill(Info model)
        {
            model.FirstName = FirstName;
            model.LastName = LastName;
            model.City = City;
            model.Description = Description;
            model.Age = Age;
            model.Busy = Busy;
            model.BirthDay = BirthDay;
            model.ProfessionId= ProfessionId;
        }

        [DisplayName("Имя")]
        [Required(ErrorMessage = "Имя обязательно")]
        [MinLength(3, ErrorMessage = "Минимум 3 символа")]
        [MaxLength(20, ErrorMessage = "Максимум 20 символов")]
        public string FirstName { get; set; }

        [DisplayName("Фамилия")]
        [Required(ErrorMessage = "Фамилия обязательна")]
        [MinLength(3, ErrorMessage = "Минимум 3 символа")]
        [MaxLength(20, ErrorMessage = "Максимум 20 символов")]
        public string LastName { get; set; }

        [DisplayName("Город")]
        [Required(ErrorMessage = "Название города обязательно")]
        [MinLength(3, ErrorMessage = "Минимум 3 символа")]
        [MaxLength(60, ErrorMessage = "Максимум 60 символов")]
        public string City { get; set; }

        [DisplayName("Описание")]
        [Required(ErrorMessage = "Напишите про себя,это обязательное поле")]
        [MinLength(3, ErrorMessage = "Минимум 3 символа")]
        [MaxLength(400, ErrorMessage = "Максимум 400 символов")]
        public string Description { get; set; }

        [DisplayName("Сколько лет")]
        [Required(ErrorMessage = "Это обязательное поле")]
        [Range(1, 200, ErrorMessage = "Некорректный возраст")]
        public int Age { get; set; }

        [DisplayName("Занятость")]
        public bool Busy { get; set; }

        public DateTime BirthDay { get; set; }

        public int ProfessionId { get; set; }
    }
}
