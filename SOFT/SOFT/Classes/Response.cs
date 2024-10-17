using System;
using System.ComponentModel.DataAnnotations;

namespace startApp.Classes
{
    public class Response
    {
        public int id{get;set;}
        public Request request{get;set;}
        public bool accept{get;set;}
    }
}