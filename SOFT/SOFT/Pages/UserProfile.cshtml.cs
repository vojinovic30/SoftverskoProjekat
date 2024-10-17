using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using startApp.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace startApp
{
    [Authorize]
    public class UserProfileModel : PageModel
    {
        
        [BindProperty]
        public  User user{get;set;}
        public int id { get; set;}
        static public Context dbContext;

        [BindProperty]
        public List<User> NeprihvaceniKorisnici { get; set; }

        [BindProperty]
        public List<Oglas> NeprihvaceniOglasi { get; set; }



        [BindProperty]
        public List<Response> odgovoriGdePokazujem{get;set;}
        public UserProfileModel(Context db)
        {
            dbContext = db;
        }

        public List<Oglas> VratiParnjakZaZamenu(Oglas moji)
        {
            List<Oglas> vrati=new List<Oglas>();
            List<Oglas> sviOglasi=dbContext.oglasi.Where(x=>x.prihvacen==true).ToList();
            if(moji.prihvacen==true)
            {
                foreach(var oglas in sviOglasi)
                {
                    if(moji.zeljeniMobilni!=null && oglas.zeljeniMobilni!=null && moji.mojMobilni.Equals(oglas.zeljeniMobilni) && moji.zeljeniMobilni.Equals(oglas.mojMobilni)
                            && !vrati.Any(x=>x.Equals(oglas)))
                        vrati.Add(oglas);   
                }
            }
            return vrati;
        }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            this.id=id;

            User userZaProveru=await dbContext.users.Where(x=>x.username==this.User.Identity.Name).FirstOrDefaultAsync();
            if(userZaProveru.id!=id)
            {
                return RedirectToPage("Index");
            }

            user = await dbContext.users.FindAsync(id);
            user.oglasi = dbContext.oglasi.Include(x=>x.mojMobilni).Include(x=>x.zeljeniMobilni)
                .Include(x=>x.user).Select(x=>x).Where(x=>x.user.id == id)
                .OrderByDescending(x=>x.id).ToList();
            
            user.favorites = dbContext.favorites.Include(x=>x.user).Include(x=>x.oglas)
                            .ThenInclude(x=>x.mojMobilni)
                            .Select(x=>x).Where(x=>x.user == user).ToList();
            user.primljeniZahtevi = dbContext.requests.Include(x=>x.sender).Include(x=>x.reciever)
                            .Include(x=>x.oglas).Select(x=>x)
                            .Where(x=>x.reciever.id == id && x.odgovoren == false && x.date > System.DateTime.Now)
                            .OrderByDescending(x=>x.id).ToList();
            user.primljeniOdgovori = dbContext.responses
                        .Include(x=>x.request).ThenInclude(x=>x.sender)
                        .Include(x=>x.request).ThenInclude(x=>x.reciever)
                        .Include(x=>x.request).ThenInclude(x=>x.oglas).ThenInclude(x=>x.mojMobilni)
                        .Include(x=>x.request).ThenInclude(x=>x.oglas).ThenInclude(x=>x.zeljeniMobilni)
                        .Select(x=>x)
                            .Where(x=>x.request.sender.id == id && x.accept == true && x.request.date > System.DateTime.Now)
                            .OrderByDescending(x=>x.id).ToList();

            odgovoriGdePokazujem = dbContext.responses
                    .Include(x=>x.request).ThenInclude(x=>x.sender)
                    .Include(x=>x.request).ThenInclude(x=>x.reciever)
                    .Include(x=>x.request).ThenInclude(x=>x.oglas).ThenInclude(x=>x.mojMobilni)
                    .Include(x=>x.request).ThenInclude(x=>x.oglas).ThenInclude(x=>x.zeljeniMobilni)
                    .Select(x=>x)
                    .Where(x=>x.request.reciever.id == id && x.accept == true && x.request.date > System.DateTime.Now)
                    .ToList();

        
            if(user.admin==true)
            {
                NeprihvaceniKorisnici=await dbContext.users.Select(x=>x).Where(x=>x.approved==false).ToListAsync();
                NeprihvaceniOglasi=await dbContext.oglasi.Include(x=>x.mojMobilni).Include(x=>x.zeljeniMobilni).Include(x=>x.user).Select(x=>x).Where(x=>x.prihvacen==false).ToListAsync();
            }

            return Page();
        }

        public IActionResult OnPostAsync(int id)
        {
            return RedirectToPage("./UserProfile", new{id});
        }


        public async Task<IActionResult> OnPostPrihvatiKorisnikaAsync(int idKorisnikaZaPrihvatanje, int idAdministratora)
        {
            User korisnikZaPrihvatanje=await dbContext.users.FindAsync(idKorisnikaZaPrihvatanje);

            korisnikZaPrihvatanje.approved=true;
            
            dbContext.users.Update(korisnikZaPrihvatanje);

            await dbContext.SaveChangesAsync();

            int id=idAdministratora;
            return RedirectToPage(new {id});
            
        }
        public async Task<IActionResult> OnPostOdbijKorisnikaAsync(int idKorisnikaZaBrisanje, int idAdministratora)
        {
            User korisnikZaBrisanje=await dbContext.users.FindAsync(idKorisnikaZaBrisanje);

            dbContext.users.Remove(korisnikZaBrisanje);

            await dbContext.SaveChangesAsync();

           int id=idAdministratora;
           return RedirectToPage(new {id});
        }
        public async Task<IActionResult> OnPostPrihvatiOglasAsync(int idOglasa, int idAdm)
        {
            Oglas oglasZaPrihvatanje=await dbContext.oglasi.FindAsync(idOglasa);
            oglasZaPrihvatanje.prihvacen=true;

            
            dbContext.oglasi.Update(oglasZaPrihvatanje);
            await dbContext.SaveChangesAsync();

            int id=idAdm;
            return RedirectToPage(new{id});
        }

        public async Task<IActionResult> OnPostOdbijOglasAsync(int idOglasa, int idAdm)
        {
            Oglas oglasZaBrisanje=await dbContext.oglasi.FindAsync(idOglasa);

            dbContext.oglasi.Remove(oglasZaBrisanje);
            await dbContext.SaveChangesAsync();

            int id=idAdm;
            return RedirectToPage(new{id});
        }
        public async Task<IActionResult> OnPostLogOut()
        {
            try
            {
                var authenticationManager=Request.HttpContext;

                await authenticationManager.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return RedirectToPage("/Index");
        }
        public async Task<IActionResult> OnPostDeclineAsync(int id)
        {
            Request theOne = dbContext.requests.Include(x=>x.sender).Include(x=>x.reciever)
                .Include(x=>x.oglas).Select(x=>x).Where(x=>x.id == id).FirstOrDefault();

            Response response = new Response();
            response.request = theOne;
            response.accept = false;

            theOne.odgovoren = true;
            theOne.prihvacen = false;
            dbContext.Update(theOne);
            dbContext.Attach(theOne).State = EntityState.Modified;

            dbContext.responses.Add(response);
            await dbContext.SaveChangesAsync();

            id = theOne.reciever.id;
            return RedirectToPage(new {id});
        }

        public async Task<IActionResult> OnPostAcceptAsync(int id)
        {
           Request theOne = dbContext.requests.Include(x=>x.sender).Include(x=>x.reciever)
                .Include(x=>x.oglas).Select(x=>x).Where(x=>x.id == id).FirstOrDefault();

            Response response = new Response();
            response.request = theOne;
            response.accept = true;

            theOne.odgovoren = true;
            theOne.prihvacen = true;
            dbContext.Update(theOne);
            dbContext.Attach(theOne).State = EntityState.Modified;

            dbContext.responses.Add(response);
            await dbContext.SaveChangesAsync();

            id = theOne.reciever.id;
            return RedirectToPage(new {id});
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var mobToDel = dbContext.oglasi.Include(x=>x.mojMobilni).Include(x=>x.user).Select(x=>x).
                Where(x=>x.id == id).FirstOrDefault();

            if(mobToDel != null)
            {
                List<FavoriteMobilni> userOglasLista=await dbContext.favorites.Where(x=>x.oglasID==id).ToListAsync();

                foreach (FavoriteMobilni uo in userOglasLista)
                {
                    dbContext.favorites.Remove(uo);
                }
                await dbContext.SaveChangesAsync();

                List<Response> responsesToDel = await dbContext.responses.Where(x=>x.request.oglas.id == id).ToListAsync();
                foreach(Response res in responsesToDel)
                {
                    dbContext.responses.Remove(res);
                }
                await dbContext.SaveChangesAsync();

                List<Request> requestsToDel = await dbContext.requests.Where(x=>x.oglas.id == id).ToListAsync();
                foreach(Request req in requestsToDel)
                {
                    dbContext.requests.Remove(req);
                }
                await dbContext.SaveChangesAsync();

                dbContext.oglasi.Remove(mobToDel);
                await dbContext.SaveChangesAsync();
            }

            id = mobToDel.user.id;
            
            return RedirectToPage(new{id});

        }

        public async Task<IActionResult> OnPostIzbaciAsync(int id)
        {
            FavoriteMobilni oglasZaIzbacivanje = dbContext.favorites.Include(x=>x.user).Include(x=>x.oglas)
                                        .Select(x=>x).Where(x=>x.oglasID == id).FirstOrDefault();

            dbContext.favorites.Remove(oglasZaIzbacivanje);
            await dbContext.SaveChangesAsync();

            id = oglasZaIzbacivanje.userID;

            return RedirectToPage("./UserProfile", new{id});
            
        }



    }
}
