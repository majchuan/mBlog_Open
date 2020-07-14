using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using mBlog.Models;
using mBlog.Service;

namespace mBlog.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly mblogContext _blogContext ;
        public IList<Blog> BlogList {get;set;} 

        public BlogUser BlogUser {get;set;}

        public Pagination PaginationSection {get;set;}

        public IndexModel(ILogger<IndexModel> logger, mblogContext blogContext)
        {
            _logger = logger;
            _blogContext = blogContext;
        }

        public ActionResult OnGet()
        {
            PaginationSection = new Pagination(new List<Object>(_blogContext.Blog.ToList().OrderByDescending(x => x.ModifyTime)), Pagination.FirstPage, null);
            BlogList = UsePagination(Pagination.FirstPage);
            BlogUser = _blogContext.BlogUser.FirstOrDefault();

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
            PaginationSection = new Pagination(new List<Object>(_blogContext.Blog.ToList()), pageNumber, true);
            BlogList = UsePagination(pageNumber);

            if(BlogList == null || BlogList.Count == 0)
            {
                return NotFound();
            }

            return Page();
        }

        public ActionResult OnGetPrevPage(int pageNumber)
        {
            PaginationSection = new Pagination(new List<Object>(_blogContext.Blog.ToList()), pageNumber, false);
            BlogList = UsePagination(pageNumber);

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
                return (x == null || x.Count == 0)? new List<Object>() : x.ToList().GetRange(((pageNumber-1) * Pagination.PageSize), count);
            }).Cast<Blog>().ToList();
        }
    }
}
