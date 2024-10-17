using System;
using System.Collections.Generic;

namespace startApp.Classes
{
    public class Oglas
    {
        public int id{get; set;}

        public Mobilni mojMobilni { get; set; }
        public Mobilni zeljeniMobilni { get; set; }


        public bool prihvacen { get; set; }
        public int? year {get; set;}
        public string picture1 {get; set;}
        public string picture2 {get; set;}
        public string picture3 {get; set;}
        public int price {get; set;} 
        public int color {get; set;}
        public int memory {get; set;}
        public int batteryHealth {get; set;}
        public User user {get; set;}
        public IList<Komentar> Komentari { get; set; }

        public virtual ICollection<FavoriteMobilni> favorites { get; set; }

    }
}