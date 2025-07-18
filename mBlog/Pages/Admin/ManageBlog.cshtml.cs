using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using mBlog.Models;
using Microsoft.AspNetCore.Authorization;

namespace mBlog.Pages.Admin
{
    [Authorize(Roles="admin")]
    public class ManageBlogModel : PageModel
    {
        private readonly ILogger<ManageBlogModel> _logger;
        private readonly mblogContext _blogContext;

        [BindProperty]
        public Blog BlogDetail {get;set;}
        public ManageBlogModel(ILogger<ManageBlogModel> logger , mblogContext blogContext)
        {
            _logger = logger;
            _blogContext = blogContext;
        }

        public ActionResult OnGet(int? id)
        {
            SetPageTitle();
            
            if(id == null)
            {
                BlogDetail = new Blog {
                    BlogTitle = string.Empty,
                    BlogSubTitle = string.Empty,
                    BlogText = string.Empty
                };
                return Page();
            }

            BlogDetail = _blogContext.Blog.Find(id);

            /*
            if(BlogDetail == null)
            {
                return NotFound();
            }
            */

            return Page();
        }

        public ActionResult OnPostSave(int? id)
        {
            var currentDateTime = DateTime.Now;
            if(id == null || id == 0)
            {
                BlogDetail.CreateTime = currentDateTime;
                BlogDetail.ModifyTime = currentDateTime;
            }else{
                BlogDetail.ModifyTime = currentDateTime;
            }
            
            var text = BlogDetail.BlogText ; 
            try
            {
                if(id == null || id == 0)
                {
                    _blogContext.Blog.Add(BlogDetail);
                    _blogContext.SaveChanges();
                }else{
                    BlogDetail.Id = (int)id ;
                    _blogContext.Blog.Update(BlogDetail);
                    _blogContext.SaveChanges();
                }
            }catch(Exception)
            {
                return NotFound();
            }
            
            return RedirectToPage("/Admin/Blog");
        }

        public ActionResult OnPostDelete(int? id)
        {
            if(id == null)
            {
                return Page();
            }

            try
            {
                var RemovedBlog = _blogContext.Blog.Find(id);
                if(RemovedBlog == null)
                {
                    return NotFound();
                }
                _blogContext.Blog.Remove(RemovedBlog);
                _blogContext.SaveChanges();
            }catch(Exception)
            {
                return NotFound();
            }
            
            return RedirectToPage("/Admin/Blog");
        }

        private void SetPageTitle()
        {
               ViewData["Title"] = "Admin Manage Blog";
        }
    }
}
