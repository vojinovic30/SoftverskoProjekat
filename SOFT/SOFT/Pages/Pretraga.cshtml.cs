using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using startApp.Classes;
using Microsoft.EntityFrameworkCore.Design;


using System.Web;  


namespace startApp
{
    public class PretragaModel : PageModel
    {
        private readonly Context dbContext;

        [BindProperty]
        public Mobilni MobilniZaPretragu { get; set;}

        public SelectList SviBrendovi{get;set;}

        [BindProperty]
        public IList<string> ListaBrendova {get;set;}
        public SelectList SviModeli{get;set;}        

        public SelectList model {get; set;}

        [BindProperty(SupportsGet=true)]
        public int sorter {get; set;}

        [BindProperty(SupportsGet=true)]
        public int yearFrom{get; set;}
        [BindProperty(SupportsGet=true)]
        public int yearTo{get;set;}

        [BindProperty(SupportsGet=true)]
        public int priceFrom{get; set;}
        [BindProperty(SupportsGet=true)]
        public int priceTo{get;set;}
        [BindProperty(SupportsGet=true)]
        public int memoryFrom{ get; set;}
        [BindProperty(SupportsGet=true)]
        public int memoryTo { get;set;}

        [BindProperty(SupportsGet=true)]
        public string byModel{get;set;}

        public IList<Oglas> oglasi { get; set; }

        public PretragaModel(Context db)
        {
            dbContext=db;
        }

        
        public IActionResult OnGetOdrediModel(string brend)
        {
           
            IQueryable<Mobilni> qMobilni=dbContext.mobilni.OrderBy(x=>x.model)
                    .Where(x=>x.brand==brend).Distinct();
            IQueryable<string> qModel=qMobilni.Select(x=>x.model).Distinct();

            SviModeli=new SelectList(qModel.ToList());

            JsonResult vrati=new JsonResult(SviModeli);
            return vrati;


        }
        public async Task OnGet()
        {
            if(MobilniZaPretragu!=null && MobilniZaPretragu.brand!=null && MobilniZaPretragu.brand!="")
            {
                OnGetOdrediModel(MobilniZaPretragu.brand);
            }
            oglasi=new List<Oglas>();
            IQueryable<string> qBrend=dbContext.mobilni
                .OrderBy(x=>x.brand).Select(x=>x.brand)
                .Distinct();
             
            IQueryable<string> qModel=dbContext.mobilni.OrderBy(x=>x.model).Select(x=>x.model).Distinct();

            SviBrendovi=new SelectList(qBrend.ToList());
            SviModeli=new SelectList(qModel.ToList());
        }

        public async Task<IActionResult> OnPostPretraziAsync()
        {
            if(MobilniZaPretragu != null && MobilniZaPretragu.brand!=null && MobilniZaPretragu.brand!="")
                OnGetOdrediModel(MobilniZaPretragu.brand);
            IQueryable<string> qBrend=dbContext.mobilni
                .OrderBy(x=>x.brand).Select(x=>x.brand)
                .Distinct();
             
            IQueryable<string> qModel=dbContext.mobilni.OrderBy(x=>x.model).Select(x=>x.model).Distinct();

            
            SviBrendovi=new SelectList(qBrend.ToList());
            
            if(MobilniZaPretragu != null)
            {
                IQueryable<Oglas> qOglasi = dbContext.oglasi.Include(x=>x.mojMobilni).Include(x=>x.user)
                        .Select(x=>x).Where(x=>x.prihvacen==true);

                if(MobilniZaPretragu.brand!=null)
                    qOglasi=qOglasi.Where(x=>x.mojMobilni.brand== MobilniZaPretragu.brand);

                if(MobilniZaPretragu.model!=null)
                    qOglasi=qOglasi.Where(x=>x.mojMobilni.model== MobilniZaPretragu.model);

                if(MobilniZaPretragu.tip!=null)
                    qOglasi=qOglasi.Where(x=>x.mojMobilni.tip== MobilniZaPretragu.tip);
            
                if(yearFrom != 0 && yearFrom != null)
                    qOglasi = qOglasi.Where(x=>x.year >= yearFrom);
                if(yearTo != 0 && yearTo != null)
                    qOglasi = qOglasi.Where(x=>x.year <= yearTo);

                if(priceFrom != 0 && priceFrom != null)
                    qOglasi = qOglasi.Where(x=>x.price >= priceFrom);
                if(priceTo != 0 && priceTo != null)
                    qOglasi = qOglasi.Where(x=>x.price <= priceTo);

                if(memoryFrom != 0 && memoryFrom != null)
                    qOglasi = qOglasi.Where(x=>x.memory >= memoryFrom);
                if(memoryTo != 0 && memoryTo != null)
                    qOglasi = qOglasi.Where(x=>x.memory <= memoryTo);

                if(sorter == 1)
                {
                    qOglasi = qOglasi.OrderBy(x=>x.price);
                }
                if(sorter == 2)
                {
                    qOglasi = qOglasi.OrderByDescending(x=>x.price);
                }
                if(sorter == 3)
                {
                    qOglasi = qOglasi.OrderBy(x=>x.memory);
                }
                if(sorter == 4)
                {
                    qOglasi = qOglasi.OrderByDescending(x=>x.memory);
                }

                if(!string.IsNullOrEmpty(byModel))
                {
                    qOglasi = qOglasi.Select(x=>x).Where(x=>x.mojMobilni.model.StartsWith(byModel));
                }

                oglasi = qOglasi.ToList();
            }
            
               
            return Page();
        }
        public async Task<IActionResult> OnPostRemoveAsync()
        {
            return RedirectToPage();
        }

       public async Task<IActionResult> OnPostBrendAsync()
       {
           return Page();
       }
    }
}
