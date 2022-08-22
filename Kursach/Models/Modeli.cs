using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kursach.Models
{
    public class Modeli
    {
    [Key] 
    public int ID_Model { get; set; }
        public string Model { get; set; }
        public string skidki { get; set; }
        public string picture { get; set; }
        public string info { get; set; }

    }
}
