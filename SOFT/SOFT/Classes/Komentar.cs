using System.ComponentModel.DataAnnotations;
using System;

namespace startApp.Classes
{
    public class Komentar
    {
        public int Id { get; set; }
        public DateTime Datum { get; set; }
        public User Komentator { get; set; }
        public string tekst {  get; set; }
        public Oglas KomentarisanOglas { get; set; }
    }
}
