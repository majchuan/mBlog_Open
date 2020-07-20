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
    public class BlogModel : PageModel
    {
        private readonly ILogger<BlogModel> _logger;
        private readonly mblogContext _blogContext ;

        public IList<Blog> BlogList {get ;set;}

        public BlogUser BlogUser {get;set;}

        public Pagination PaginationSection {get;set;}

        [BindProperty]
        public int PageNumber { get;set;}

        public BlogModel(ILogger<BlogModel> logger, mblogContext blogContext)
        {
            _logger = logger;
            _blogContext = blogContext;
        }

        public ActionResult OnGet()
        {
            SetPageTitle();
            PaginationSection = new Pagination(new List<Object>(_blogContext.Blog.OrderByDescending(x => x.ModifyTime)), Pagination.FirstPage, null);
            BlogList = UsePagination(Pagination.FirstPage);
            PageNumber = Pagination.FirstPage ;

            /*
            if(BlogList == null || BlogList.Count == 0 )
            {
                return NotFound();
            }
            */

            return Page();
        }

        public ActionResult OnGetNextPage(int pageNumber)
        {
            SetPageTitle();
            PaginationSection = new Pagination(new List<Object>(_blogContext.Blog.OrderByDescending(x => x.ModifyTime) ), pageNumber, true);
            PageNumber = PaginationSection.CurrentPageNumber;
            BlogList = UsePagination(PageNumber);

            if(BlogList == null || BlogList.Count == 0)
            {

                return NotFound();
            }

            return Page();
        }

        public ActionResult OnGetPrevPage(int pageNumber)
        {
            SetPageTitle();            
            PaginationSection = new Pagination(new List<Object>(_blogContext.Blog.OrderByDescending(x => x.ModifyTime) ), pageNumber, false);
            PageNumber = PaginationSection.CurrentPageNumber;
            BlogList = UsePagination(PageNumber);

            if(BlogList == null || BlogList.Count == 0)
            {
                return NotFound();
            }

            return Page();
        }

        private IList<Blog> UsePagination(int pageNumber)
        {
            return PaginationSection.UsePagination( (x, pageNumber) => {
                var count = 0 ;
                if(x != null && x.Count > 0)
                {
                    var lastPage = (int)Math.Ceiling((decimal)x.Count / Pagination.PageSize);

                    switch (pageNumber)
                    {
                        case Pagination.FirstPage :
                            if(x.Count < Pagination.PageSize)
                            {
                                count = x.Count ;
                            }else{
                                count = Pagination.PageSize;
                            }
                            break;
                        case int currentPage when lastPage == pageNumber :
                            count = x.Count - (currentPage - 1) * Pagination.PageSize;
                            break;
                        default :
                            count = Pagination.PageSize ;
                            break;
                    }
                }
                return (x== null || x.Count == 0) ? new List<Object>() : x.ToList().GetRange(((pageNumber-1) * Pagination.PageSize), count);
            }).Cast<Blog>().ToList();
        }

        private void SetPageTitle()
        {
            ViewData["Title"] = "Admin Blog Manage page";
        }
        
    }
}
