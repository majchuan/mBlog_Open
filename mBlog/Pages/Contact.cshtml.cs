using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using mBlog.Models;

namespace mBlog.Pages
{
    public class ContactModel : PageModel
    {
        private readonly ILogger<ContactModel> _logger;
        private readonly mblogContext _blogContext;

        [BindProperty]
        public Contact ContactInfo {get;set;}
        public ContactModel(ILogger<ContactModel> logger , mblogContext blogContext)
        {
            _logger = logger;
            _blogContext = blogContext;
        }

        public void OnGet()
        {
            ContactInfo = new Contact{
                ContactName = string.Empty,
                Email = string.Empty,
                ContactMessage = string.Empty
            };
        }

        public ActionResult OnPost()
        {
            if(ModelState.IsValid == false)
            {
                return Page();
            }

            try
            {
                _blogContext.Contact.Add(ContactInfo);
                _blogContext.SaveChanges();
            }catch(Exception)
            {
                return NotFound();
            }
            
            return RedirectToPage("/Index");
        }

    }
}
