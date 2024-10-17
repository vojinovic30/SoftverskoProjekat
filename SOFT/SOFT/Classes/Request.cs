using System;
using System.ComponentModel.DataAnnotations;

namespace startApp.Classes
{
    public class Request
    {
        public int id{get;set;}

        [DataType(DataType.Time)]
        public DateTime time{get;set;}
        [DataType(DataType.Date)]
        [MojDateValidator]
        public DateTime date{get;set;}
        public User sender{get;set;}
        public User reciever{get;set;}
        public Oglas oglas{get;set;}
        public bool odgovoren{get;set;} //da li je korisnik odgovorio na zahtev
        public bool prihvacen{get; set;} //da li je prihvatio ili odbio zahtev
    }
}