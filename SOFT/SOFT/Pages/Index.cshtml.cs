using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using startApp.Classes;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.IO;

namespace startApp.Pages
{
    public class IndexModel : PageModel
    {
        public readonly Context dbContext;       

        public IList<User> users{get;set;}

        public IList<Oglas> oglasi{get;set;}


        [BindProperty]
        public User myUser { get; set; }

        public IndexModel(Context db)
        {
            dbContext = db;
        }
       
        public async Task OnGetAsync()
        {
             myUser=await dbContext.users
                            .Select(x=>x)
                            .Where(x=>x.username==this.User.Identity.Name)
                            .FirstOrDefaultAsync();
           
            IQueryable<Oglas> qOglasi = dbContext.oglasi.Include(x=>x.mojMobilni).Include(x=>x.user)
                    .Select(x=>x).Where(x=>x.prihvacen==true);
           
            oglasi = qOglasi.ToList();
            
        }
        
        public IActionResult OnPostRemoveAsync()
        {
            return RedirectToPage();
        }
        public IActionResult OnPostViewMobilniAsync(int id)
        {
            return RedirectToPage("./ViewMObilni", new {id});
        }
        public async Task<IActionResult> OnPostLogOutAsync()
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
    }
}
