

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ASP_NET_PZ_03.Models
{
    public class Info
    {
        public int Id { get; set; }

        [DisplayName("Имя")] 
        [Required(ErrorMessage = "Имя обязательно")]
        [MinLength(3,ErrorMessage = "Минимум 3 символа")]
        [MaxLength(20,ErrorMessage = "Максимум 20 символов")]
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
        [Range(1,200,ErrorMessage = "Некорректный возраст")]
        public int Age { get; set; }

        [DisplayName("Занятость")]
        public bool Busy { get; set; }

        public DateTime BirthDay { get; set; }

        public Info(string firstname,string lastname,string city,string desc,int age,bool busy,DateTime birth)
        {
            FirstName = firstname;
            LastName = lastname;
            City = city;
            Description = desc;
            Age = age;
            Busy = busy;
            BirthDay = birth;
        }

        public Info()
        {

        }
    }
}
