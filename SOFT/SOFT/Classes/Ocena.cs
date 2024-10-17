using System.ComponentModel.DataAnnotations;
using System;

namespace startApp.Classes
{
    public class Ocena
            {
        public int Id { get; set; }
        public User ocenio { get; set; }
        public int ocena{ get; set; }
        public User ocenjen{ get; set; }
    }
}
