using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using startApp.Classes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace startApp
{
    public class AddMobilniModel : PageModel
    {
        [BindProperty]
        public Mobilni MojMobilni { get; set; }

        [BindProperty]
        public Mobilni ZeljeniMobilni { get; set; }
        [BindProperty]
        public Oglas oglas{get;set;}
        [BindProperty]
        public int userID{get;set;}

        private Context dbContext;

        

        public AddMobilniModel(Context db)
        {
            dbContext = db;
        }

        public void OnGet(int id)
        {
            userID = id;
        }

        public IActionResult OnPostAsync(int id)
        {
            return RedirectToPage(new{id});
        }
        public async Task<IActionResult> OnPostDodajMobilniAsync()
        {
            var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new { x.Key, x.Value.Errors })
                .ToArray();
            if(!ModelState.IsValid)
            {
                return Page();
            }
            else
            {
                User user1 = dbContext.users.Where(x=>x.username == this.User.Identity.Name).FirstOrDefault();
                if(oglas.picture1 != null)
                    oglas.picture1 = "Pictures\\" + oglas.picture1;
                if(oglas.picture2 != null)
                    oglas.picture2 = "Pictures\\" + oglas.picture2;
                if(oglas.picture3 != null)
                    oglas.picture3 = "Pictures\\" + oglas.picture3;

                if(MojMobilni.brand != null)
                    MojMobilni.brand = MojMobilni.brand.First().ToString().ToUpper() + MojMobilni.brand.Substring(1);
                if(MojMobilni.model != null)
                    MojMobilni.model = MojMobilni.model.First().ToString().ToUpper() + MojMobilni.model.Substring(1);
                if(ZeljeniMobilni.brand != null)
                    ZeljeniMobilni.brand = ZeljeniMobilni.brand.First().ToString().ToUpper() + ZeljeniMobilni.brand.Substring(1);
                if(ZeljeniMobilni.model != null)
                    ZeljeniMobilni.model = ZeljeniMobilni.model.First().ToString().ToUpper() + ZeljeniMobilni.model.Substring(1);


                Mobilni pomMobilni = dbContext.mobilni
                                .Select(x=>x).Where(x=>x.model==MojMobilni.model && x.brand==MojMobilni.brand && x.tip==MojMobilni.tip)
                                .FirstOrDefault();

                //proveri da li je neki korisnik do sada dodao ovakav mob...ako nije dodaj ga ti(u bazu), a ako jeste, uzmi samo tu referencu(on ce da uzme id iz baze)
                if(pomMobilni == null)
                {
                    oglas.mojMobilni=new Mobilni();
                    oglas.mojMobilni.brand=MojMobilni.brand;
                    oglas.mojMobilni.model=MojMobilni.model;
                    oglas.mojMobilni.tip=MojMobilni.tip;
                }
                else
                {
                   
                    oglas.mojMobilni = pomMobilni;
                }


                oglas.user = user1;
                if(oglas.user.admin==true)
                    oglas.prihvacen=true;

                if(ZeljeniMobilni.brand!=null && ZeljeniMobilni.model!=null && ZeljeniMobilni.tip!=null)
                {
                    pomMobilni = dbContext.mobilni
                                .Select(x=>x).Where(x=>x.model==ZeljeniMobilni.model 
                                && x.brand==ZeljeniMobilni.brand)
                                .FirstOrDefault();

                    if(pomMobilni == null)
                    {
                        oglas.zeljeniMobilni=new Mobilni();
                        oglas.zeljeniMobilni.model=ZeljeniMobilni.model;
                        oglas.zeljeniMobilni.brand=ZeljeniMobilni.brand;
                        dbContext.mobilni.Add(oglas.zeljeniMobilni);

                    }
                    else
                    {
                        oglas.zeljeniMobilni = pomMobilni;
                    }


                    
                }

                dbContext.oglasi.Add(oglas);

                await dbContext.SaveChangesAsync();

                int id = user1.id;
                return RedirectToPage("/UserProfile", new{id});
            }
        }

       
    }
}
