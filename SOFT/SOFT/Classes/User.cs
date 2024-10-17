using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace startApp.Classes
{
    public class User
    {
        public int id {get; set;}
        public string name {get; set;}
        public string surname {get;set;}

        public string username { get; set; }
        public string password { get; set; }
        public string address{get;set;}
        public bool admin{get;set;}

        public bool approved { get; set; }
        public string picture{get;set;}


        public string sigurnosnoPitanje { get; set; }

        public string odgovorNaPitanje { get; set; }
        public string brojTelefona { get; set; }


        public IList<Response> primljeniOdgovori;
        public IList<Request> primljeniZahtevi;
        public IList<Oglas> oglasi { get; set; }

         public virtual ICollection<FavoriteMobilni> favorites { get; set; }
    }
}