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
    public class EditUserModel : PageModel
    {
        [BindProperty]
        public User user{get;set;}
        public Context dbContext;
        public String message { get; set; }="";
        [BindProperty]
        public string oldPassword{get;set;}
        [BindProperty]
        public string passwordNew1{get;set;}
        [BindProperty]
        public string passwordNew2{get;set;}

        public EditUserModel(Context db)
        {
            dbContext =db;
        }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            User userZaProveru=await dbContext.users.Where(x=>x.username==this.User.Identity.Name).FirstOrDefaultAsync();
            if(userZaProveru.id!=id)
                return RedirectToPage("Index");

            user = dbContext.users.Where(x=>x.id == id).FirstOrDefault();

            return Page();
        }
        public IActionResult OnPostAsync(int id)
        {
            return RedirectToPage("./EditUser", new{id});
        }

        public async Task<IActionResult> OnPostChange()
        {    
            if((user.sigurnosnoPitanje == null && user.odgovorNaPitanje != null) ||
                (user.sigurnosnoPitanje != null && user.odgovorNaPitanje == null))
            {
                message = "Potrebno je uneti i sigurnosno pitanje i odgovor na pitanje.";
                return Page();
            }
            if(oldPassword == null && passwordNew1 == null && passwordNew2 == null)
            {

            }            
            else
            {
                if(oldPassword == null || passwordNew1 == null || passwordNew2 == null)
                {
                    message = "Za promenu passworda morate da unesete stari i dva puta novi password.";
                    return Page();
                }
                else
                {
                    if(user.password == oldPassword)
                    {
                        if(passwordNew1 == passwordNew2)
                        {
                            user.password = passwordNew1;
                        }
                        else
                        {
                            message = "Ne poklapaju se dva nova uneta passworda.";
                            return Page();
                        }
                    }
                    else
                    {
                        message = "Stari password se ne poklapa.";
                        return Page();
                    }
                }
            }


            dbContext.Update(user);
            dbContext.Attach(user).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
            int id = user.id;

            return RedirectToPage("./UserProfile", new{id});
        }
    }
}
