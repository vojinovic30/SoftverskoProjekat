using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using startApp.Classes;

namespace startApp
{
    public class ViewMobilniModel : PageModel
    {
        [BindProperty]
        public Oglas oglas { get; set; }

        [BindProperty]
        public Komentar komentar { get; set; }
        [BindProperty]
        public Ocena ocena{ get; set; }

        [BindProperty]
        public List<Oglas> thisOwnerOglasi { get; set; }
        public int id { get; set; }
        public IList<Oglas> recommendedMobilni { get; set; }
        public List<Komentar> Komentari { get; set; }    
        public string prosek { get; set; }
        public double prosek2 { get; set; }
        public List<Ocena> Ocene{ get; set; }    
        [Required]
        [BindProperty]
        public Request request{ get; set; }

        public bool save = true; //da li je snimljen u omiljene ili ne 
        public bool send = true; // da li je zahtev poslat, a ne odgovoren
        public bool boolocenio = true; // da li je trenutni korisnik ocenio ovog

        private readonly Context dbContext;

        public String message { get; set; }="";
        public ViewMobilniModel(Context db)
        {
            dbContext =db;
        }

        private async Task dobavi(int id)
        {
            this.id = id;
            oglas = dbContext.oglasi.Include(x=>x.user).Include(x=>x.mojMobilni)
                .Select(x=>x).Where(x=>x.id == id).FirstOrDefault();

            Komentari = await dbContext.komentari.Include(x=>x.Komentator).Include(x=>x.KomentarisanOglas).Select(x=>x). Where(x=>x.KomentarisanOglas.id == id).ToListAsync();
            Ocene = await dbContext.ocene.Include(x=>x.ocenjen).Include(x=>x.ocenio).Select(x=>x)
                .Where(x=>x.ocenjen.id == oglas.user.id).ToListAsync();

            thisOwnerOglasi = await dbContext.oglasi.Include(x=>x.user).Include(x=>x.mojMobilni)
                    .Select(x=>x).Where(x=>x.user.id == oglas.user.id && x.prihvacen == true && x.id != this.id).ToListAsync();

            recommendedMobilni = await dbContext.oglasi.Include(x=>x.mojMobilni).Include(x=>x.user).Select(x=>x)
                .Where(x=>x.id != oglas.id && x.mojMobilni.tip == oglas.mojMobilni.tip).ToListAsync();  

            FavoriteMobilni favoritesCheck = dbContext.favorites.Include(x=>x.user).Include(x=>x.oglas).
                        Select(x=>x).Where(x=>x.user.username == this.User.Identity.Name && x.oglas.id == oglas.id).FirstOrDefault();
            if(favoritesCheck != null)
            {
                save = false;
            }

            Request requestCheck = dbContext.requests.Include(x=>x.sender).Include(x=>x.reciever)
                        .Include(x=>x.oglas).Select(x=>x)
                        .Where(x=>x.odgovoren == false && x.oglas == oglas && x.sender.username == this.User.Identity.Name)
                        .FirstOrDefault();
        }
        public async Task OnGetAsync(int id)
        {
            
            this.id = id;
            oglas = dbContext.oglasi.Include(x=>x.user).Include(x=>x.mojMobilni)
                .Select(x=>x).Where(x=>x.id == id).FirstOrDefault();
            Komentari = await dbContext.komentari.Include(x => x.Komentator).Include(x => x.KomentarisanOglas)
                .Select(x => x).Where(x => x.KomentarisanOglas.id == id).OrderByDescending(x => x.Datum).ToListAsync();
            Ocene = await dbContext.ocene.Include(x => x.ocenjen).Include(x => x.ocenio).Select(x => x)
               .Where(x => x.ocenjen.id == oglas.user.id).ToListAsync();
            thisOwnerOglasi = await dbContext.oglasi.Include(x=>x.user).Include(x=>x.mojMobilni)
                    .Select(x=>x).Where(x=>x.user.id == oglas.user.id && x.prihvacen == true && x.id != this.id).ToListAsync();

            recommendedMobilni = await dbContext.oglasi.Include(x=>x.mojMobilni).Include(x=>x.user).Select(x=>x)
                .Where(x=>x.id != oglas.id && x.mojMobilni.tip == oglas.mojMobilni.tip).ToListAsync();
            
            if (Ocene.Count() != 0)
            {
                prosek2= 0;
                foreach (var o in Ocene)
                {
                    prosek2 += o.ocena;
                }
                prosek2 = prosek2 / Ocene.Count();
                prosek2 = Math.Round(prosek2, 2);  
                prosek = "";
            }
            else
            {
                prosek = "Korisnik trenutno nije ocenjivan";
            }
            User ocenjen = oglas.user;
            User ocenio = dbContext.users.Include(x => x.oglasi).Select(x => x)
            .Where(x => x.username == this.User.Identity.Name).FirstOrDefault();
            if (ocenio != null)
            {
                var pomOcena = dbContext.ocene.Include(x => x.ocenjen).Include(x => x.ocenio).Select(x => x)
               .Where(x => x.ocenjen.id == ocenjen.id && x.ocenio.id == ocenio.id).FirstOrDefault(); if (pomOcena != null)
                {
                    boolocenio = true;
                }
                else
                {
                    boolocenio = false;
                }
            }
            

            FavoriteMobilni favoritesCheck = dbContext.favorites.Include(x=>x.user).Include(x=>x.oglas).
                        Select(x=>x).Where(x=>x.user.username == this.User.Identity.Name && x.oglas.id == oglas.id).FirstOrDefault();
            if(favoritesCheck != null)
            {
                save = false;
            }

            Request requestCheck = dbContext.requests.Include(x=>x.sender).Include(x=>x.reciever)
                        .Include(x=>x.oglas).Select(x=>x)
                        .Where(x=>x.oglas == oglas && x.sender.username == this.User.Identity.Name)
                        .FirstOrDefault();
            
            if(requestCheck != null)
            {
                if(requestCheck.date < System.DateTime.Now)
                {
                    send = false;
                }
                if(requestCheck.odgovoren == false)
                {
                    send = false;
                    message = "Poslali ste zahtev za razgledanje ovog telefona, ceka se na odgovor vlasnika";
                }
                if(requestCheck.odgovoren == true && requestCheck.prihvacen == true)
                {    
                    send = false;
                    message = "Poslali ste zahtev za razgledanje ovog telefona. Pogledajte na profilu u delu 'Dogadjaji' za vise informacija.";
                }
            }
        }

        public IActionResult OnPostAsync(int id)
        {
            return RedirectToPage("./ViewMobilni", new{id});
        }

        public async Task<IActionResult> OnPostSendRequest(int id)
        {
             var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new { x.Key, x.Value.Errors })
                .ToArray();
            if(!ModelState.IsValid)
            {
                await dobavi(id);
                return Page();
            }
            else
            {
                Oglas pomocni = dbContext.oglasi.Include(x=>x.user).Include(x=>x.mojMobilni).
                    Select(x=>x).Where(x=>x.id == id).FirstOrDefault();            
         
                Request newRequest = dbContext.requests.Include(r=>r.oglas).Include(r=>r.sender).Where(r=>r.sender.username.Equals(this.User.Identity.Name)==true  && r.oglas.id==pomocni.id).FirstOrDefault();
                
                bool pom=true;
                if(newRequest==null)
                {
                    newRequest= new Request();
                }
                else
                {
                    pom=false;
                    newRequest.odgovoren=false;
                    newRequest.prihvacen=false;
                }
                newRequest.time = request.time;
                newRequest.date = request.date;

                User sender = dbContext.users.Select(x=>x).Where(x=>x.username == this.User.Identity.Name).FirstOrDefault();

                newRequest.sender = sender;
                newRequest.reciever = pomocni.user;

                newRequest.oglas = pomocni;
                if(pom==false)
                    dbContext.Update(newRequest);
                else
                    dbContext.requests.Add(newRequest);
                await dbContext.SaveChangesAsync();
    
                return RedirectToPage("/ViewMobilni", new{id});
            }
        }
        
        public async Task<IActionResult> OnPostSaveFavoritesAsync(int id)
        {
            FavoriteMobilni theOne = new FavoriteMobilni();
            User user = dbContext.users.Include(x=>x.oglasi).Select(x=>x).Where(x=>x.username == this.User.Identity.Name).FirstOrDefault();
            theOne.user = user;
            Oglas oglas = dbContext.oglasi.Include(x=>x.user).Include(x=>x.mojMobilni)
                .Select(x=>x).Where(x=>x.id == id).FirstOrDefault();
            theOne.oglas = oglas;

            dbContext.favorites.Add(theOne);
            await dbContext.SaveChangesAsync();
            
            return RedirectToPage("./ViewMobilni", new{id});
        }

        public async Task<IActionResult> OnPostRemoveFavoritesAsync(int id)
        {
            FavoriteMobilni oglasZaIzbacivanje = dbContext.favorites.Include(x=>x.user).Include(x=>x.oglas)
                                        .Select(x=>x).Where(x=>x.oglasID == id).FirstOrDefault();

            dbContext.favorites.Remove(oglasZaIzbacivanje);
            await dbContext.SaveChangesAsync();

            id = oglasZaIzbacivanje.userID;
            
            return RedirectToPage("./UserProfile", new{id});
        }

        public async Task<IActionResult> OnPostDodajKomentar(int id)
        {
                oglas = dbContext.oglasi.Include(x => x.user).Include(x => x.mojMobilni)
                .Select(x => x).Where(x => x.id == id).FirstOrDefault();
            User user = dbContext.users.Include(x => x.oglasi).Select(x => x).Where(x => x.username == this.User.Identity.Name).FirstOrDefault();
            Komentar kom = new Komentar();
                kom.Komentator = user;
                kom.tekst = komentar.tekst;
                kom.Datum = DateTime.Now;
                kom.KomentarisanOglas = oglas;

                //oglas.Komentari.Add(kom);
                //Komentari.Add(kom);

                dbContext.komentari.Add(kom);

                await dbContext.SaveChangesAsync();
                return RedirectToPage("./ViewMobilni", new { id });
        }
        public async Task<IActionResult> OnPostDodajOcenu(int id)
        {
            oglas = dbContext.oglasi.Include(x => x.user).Include(x => x.mojMobilni)
                .Select(x => x).Where(x => x.id == id).FirstOrDefault();
            User ocenjen = oglas.user;
            User ocenio = dbContext.users.Include(x => x.oglasi).Select(x => x)
                .Where(x => x.username == this.User.Identity.Name).FirstOrDefault();

            Ocena oc = new Ocena();
            oc.ocenjen = ocenjen;
            oc.ocenio = ocenio;
            oc.ocena = ocena.ocena;
            dbContext.ocene.Add(oc);
            await dbContext.SaveChangesAsync();
            return RedirectToPage("./ViewMobilni", new { id });
        }
    }
}
