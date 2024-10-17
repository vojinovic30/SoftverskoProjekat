using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using startApp.Classes;

namespace startApp
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public Oglas oglas {get; set;}
        private readonly Context dbContext;

        public EditModel(Context db)
        {
            dbContext =db;
        }
        public async Task<IActionResult> OnGetAsync(int id)
        {
       
            oglas =await dbContext.oglasi.Include(x=>x.user).Include(x=>x.mojMobilni).
                            Select(x=>x).Where(x=>x.id==id).FirstOrDefaultAsync();

            User userZaProveru=await dbContext.users.Include(x=>x.oglasi).Where(x=>x.username==this.User.Identity.Name).FirstOrDefaultAsync();
        
            if(userZaProveru.oglasi==null || !userZaProveru.oglasi.Contains(oglas))
                return RedirectToPage("Index");

            return Page();
            
        }

         public IActionResult OnPostAsync(int id)
        {
            return RedirectToPage("./Edit", new{id});
        }

        public async Task<IActionResult> OnPostChange()
        {
            //oglas.prihvacen = true;
            dbContext.Update(oglas);
            dbContext.Attach(oglas).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
           
            return RedirectToPage("./Index");
        }
    }
}