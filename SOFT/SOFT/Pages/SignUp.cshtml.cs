using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using startApp.Classes;

namespace startApp.Pages
{
    public class SignUpModel : PageModel
    {
        private readonly Context dbContext;
       public SignUpModel(Context db)
       {
           dbContext=db;
       }


       
       [BindProperty]
       public string Poruka { get; set; }
       [BindProperty]
       public User MojKorisnik { get; set; }


       [BindProperty]
       public string ponovljeniPassword { get; set; }
   
       public async Task<IActionResult> OnPostAsync()
       {
           User potencijalniKorisnik=dbContext.users
                        .Select(x=>x)
                        .Where(x=>x.username==MojKorisnik.username)
                        .FirstOrDefault();

            if(potencijalniKorisnik!=null)
            {
                Poruka="Ovo korisnicko ime vec postoji.";
                return Page();
            }
            else
            {

                if(MojKorisnik.username==null)
                {
                    Poruka="Unesite korisnicko ime";
                    return Page();
                }

                if(MojKorisnik.password==null)
                {
                    Poruka="Unesite sifru";
                    return Page();
                }

                if(MojKorisnik.password!=ponovljeniPassword)
                {
                    Poruka="Niste potvrdili lepo sifru";
                    return Page();
                }
                string slika = "Pictures\\"+MojKorisnik.picture;
                MojKorisnik.picture = slika;

                dbContext.users.Add(MojKorisnik);

                

                await dbContext.SaveChangesAsync();

                MojKorisnik=dbContext.users
                        .Select(x=>x)
                        .Where(x=>x.username==MojKorisnik.username)
                        .FirstOrDefault();

                int idKorisnika=MojKorisnik.id;

                return RedirectToPage("Index");
            }
       }
    }
}
