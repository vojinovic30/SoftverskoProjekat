using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using startApp.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;

namespace startApp.Pages
{
    public class LogInModel : PageModel
    {
        private readonly Context dbContext;

       public LogInModel(Context db)
       {
           dbContext=db;
       }

       [BindProperty]
       public LoginViewModel LoginModel { get; set; }

       public String Poruka { get; set; }="";

       [BindProperty]
       public string pitanje { get; set; }

       [BindProperty]
       public string odgovor { get; set; }

       [BindProperty]
       public string KlasaSigPitanje { get; set; }="nePrikazuj";

       [BindProperty]
       public string KlasaRegistracijaDugme { get; set; }="nePrikazuj";

        public IActionResult OnGet()
        {
            try
            {
                //Verifikacija
                if(this.User.Identity.IsAuthenticated)
                {
                    var logovanKorisnik=dbContext.users.Select(x=>x)
                                .Where(y=>y.username==LoginModel.Username && y.password==LoginModel.Password)
                                .FirstOrDefault();
                    int id=logovanKorisnik.id;
                    return RedirectToPage("/UserProfile", new {id});
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            return Page();
        }

        public async Task<IActionResult> OnPostLogIn()
        {
            try
            {
                   

                if(LoginModel.Username==null)
                {
                    Poruka="Molimo Vas unesite username.";
                    return Page();
                }
                User loginInfo=dbContext.users.Select(x=>x)
                        .Where(y=>y.username==LoginModel.Username).FirstOrDefault();

                if(loginInfo==null)
                {
                    Poruka="Ne postoji korisnik sa ovim username-om.";
                    KlasaRegistracijaDugme="";
                    return Page();
                }
                if(loginInfo.approved==false)
                {
                    Poruka="Niste jos prihvaceni od strane administratora.";
                    return Page();
                }

                if(LoginModel.Password==null)
                {
                    Poruka="Molimo Vas unesite sifru.";
                    return Page();
                }


                if(LoginModel.Password!=loginInfo.password)
                {
                    Poruka="Ne slazu se username i password.";
                    return Page();
                }
                else
                {
                    await this.SignInUser(loginInfo.username,false);
                    int id=loginInfo.id;
                    return RedirectToPage("/UserProfile", new{id});

                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }

            return Page();
        }

        private async Task SignInUser(object username, bool isPersistent)
        {
            //Inicijalizacija
            var claims=new List<Claim>();

            //koristimo claim da bismo omogucili autorizaciju
            try
            {
                claims.Add(new Claim(ClaimTypes.Name,(string)username));
                var claimIdenties=new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
                var claimPrincipal=new ClaimsPrincipal(claimIdenties);
                var authenticationManager=Request.HttpContext;

                await authenticationManager.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    claimPrincipal, new AuthenticationProperties(){IsPersistent=isPersistent});
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult OnPostZaboravioSamSifru()
        {
            

            if(LoginModel.Username==null)
            {
                Poruka="Molimo Vas unesite username.";
                return Page();
            }
            
           
                    //Inicijalizacija
            User loginInfo=dbContext.users.Select(x=>x)
                        .Where(y=>y.username==LoginModel.Username).FirstOrDefault();
            if(loginInfo==null)
            {
                Poruka="Ne postoji korisnik sa datim username-om";
                 KlasaRegistracijaDugme="";
                return Page();
            }
            KlasaSigPitanje="";
                if(loginInfo.approved==false)
                {
                    Poruka="Niste jos prihvaceni od strane administratora.";
                    return Page();
                }

                if(loginInfo.sigurnosnoPitanje==null)
                    Poruka="Niste definisali sigurnosno pitanje pri registraciji.";
                else
                    pitanje=loginInfo.sigurnosnoPitanje;
            
            
            return Page();
        }

        public async Task<IActionResult> OnPostProveriOdgovorAsync()
        {
            User loginInfo=dbContext.users.Select(x=>x)
                        .Where(y=>y.username==LoginModel.Username).FirstOrDefault();
            if(loginInfo==null)
            {
                Poruka="Unesite korisnicko ime.";
                return Page();
            }
            if(loginInfo.approved==false)
            {
                 Poruka="Niste jos prihvaceni od strane administratora.";
                return Page();
            }
            pitanje=loginInfo.sigurnosnoPitanje;
            if(odgovor==null)
            {
                Poruka="Unesite odgovor na sigurnosno pitanje.";
                KlasaSigPitanje="";
                
                return Page();
            }
            

            if(loginInfo.odgovorNaPitanje.Equals(odgovor))
            {
                await this.SignInUser(loginInfo.username,false);
                int id=loginInfo.id;
                return RedirectToPage("/UserProfile", new{id});
            }
            else
            {
                Poruka="Los odgovor na sigurnosno pitanje";
                KlasaSigPitanje="";
                return Page();
            }
        }
    }
}
