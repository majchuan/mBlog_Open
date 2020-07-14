using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using mBlog.Models;
using mBlog.Service;
using Microsoft.AspNetCore.Authorization;

namespace mBlog.Pages.Admin
{
    [Authorize(Roles="admin")]
    public class ContactModel : PageModel
    {
        private readonly ILogger<ContactModel> _logger;
        private readonly mblogContext _blogContext;

        [BindProperty]
        public IList<Contact> ContactInfoList {get;set;}
        public Pagination PaginationSection { get;set;}
        public ContactModel(ILogger<ContactModel> logger , mblogContext blogContext)
        {
            _logger = logger;
            _blogContext = blogContext;
        }

        public void OnGet()
        {
            SetPageTitle();
            PaginationSection = new Pagination(new List<Object>(_blogContext.Contact.ToList()), Pagination.FirstPage, null);
            ContactInfoList = UsePagination();
        }
        public ActionResult OnGetNextPage(int pageNumber)
        {
            SetPageTitle();
            PaginationSection = new Pagination(new List<Object>(_blogContext.Contact.ToList()), pageNumber, true);
            ContactInfoList = UsePagination();

            if(ContactInfoList == null || ContactInfoList.Count == 0)
            {
                
                return NotFound();
            }

            return Page();
        }

        public ActionResult OnGetPrevPage(int pageNumber)
        {
            SetPageTitle();            
            PaginationSection = new Pagination(new List<Object>(_blogContext.Contact.ToList()), pageNumber, false);
            ContactInfoList = UsePagination();

            if(ContactInfoList == null || ContactInfoList.Count == 0)
            {
                return NotFound();
            }

            return Page();
        }

        private IList<Contact> UsePagination()
        {
            return PaginationSection.UsePagination().Cast<Contact>().ToList();
        }
        private void SetPageTitle()
        {
            ViewData["Title"] = "Admin Contact Manage page";
        }

    }
}
