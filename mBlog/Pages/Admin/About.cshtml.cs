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
    public class AboutModel : PageModel
    {
        private readonly ILogger<AboutModel> _logger;
        private readonly mblogContext _blogContext ;

        public IList<UserInformation> UserInfoList {get ;set;}

        public BlogUser BlogUser {get;set;}

        public Pagination PaginationSection {get;set;}

        public AboutModel(ILogger<AboutModel> logger, mblogContext blogContext)
        {
            _logger = logger;
            _blogContext = blogContext;
        }

        public ActionResult OnGet()
        {
            SetPageTitle();
            PaginationSection = new Pagination(new List<Object>(
                _blogContext.UserInformation.OrderByDescending(x => x.Id)), Pagination.FirstPage, null);
            BlogUser = _blogContext.BlogUser.FirstOrDefault();
            UserInfoList = UsePagination(Pagination.FirstPage);

            return Page();
        }

        public ActionResult OnGetNextPage(int pageNumber)
        {
            SetPageTitle();
            PaginationSection = new Pagination(new List<Object>(_blogContext.UserInformation.ToList()), pageNumber, true);
            UserInfoList = UsePagination(pageNumber);

            return Page();
        }

        public ActionResult OnGetPrevPage(int pageNumber)
        {
            SetPageTitle();
            PaginationSection = new Pagination(new List<Object>(_blogContext.UserInformation.ToList()), pageNumber, false);
            UserInfoList = UsePagination(pageNumber);

            if(UserInfoList == null || UserInfoList.Count == 0)
            {
                return NotFound();
            }

            return Page();
        }

        private IList<UserInformation> UsePagination(int pageNumber)
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
                return (x == null || x.Count == 0) ? new List<Object>() : x.ToList().GetRange(((pageNumber-1) * Pagination.PageSize), count);
            }).Cast<UserInformation>().ToList();
        }

        private void SetPageTitle()
        {
            ViewData["Title"] = "Admin about me page";
        }
    }
}
