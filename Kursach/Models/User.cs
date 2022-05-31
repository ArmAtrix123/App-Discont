using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kursach.Models
{
    public class User
    {
        [Key]
        public int Id_user { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password{ get; set; }
        public string Familia { get; set; }
        public string Ima { get; set; }
        public string Otchestvo { get; set; }
        public DateTime DataR { get; set; }

    }

    public enum StateSort
    {
        IdAsc,
        IdDesc,
        EmailAsc,
        EmailDesc
    }

    public class LoginUser
    {
        public string login { get; set; }
        public string password { get; set; }
    }
}
