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
    public class ManageAboutModel : PageModel
    {
        private readonly ILogger<ManageAboutModel> _logger;
        private readonly mblogContext _blogContext;

        [BindProperty]
        public UserInformation UserInfo {get;set;}
        public ManageAboutModel(ILogger<ManageAboutModel> logger , mblogContext blogContext)
        {
            _logger = logger;
            _blogContext = blogContext;
        }

        public ActionResult OnGet(int? id)
        {
            SetPageTitle();
            if(id == null)
            {
                UserInfo = new UserInformation{
                    UserIntro = string.Empty 
                };
            }else
            {
                UserInfo = _blogContext.UserInformation.Find(id);
            }
            
            /*
            if(UserInfo == null)
            {
                return NotFound();
            }
            */

            return Page();
        }

        public ActionResult OnPostSave(int? id)
        {
            if(ModelState.IsValid == false)
            {
                return Page();
            }

            try
            {
                if(id == null || id == 0) 
                {
                    _blogContext.UserInformation.Add(UserInfo);
                    _blogContext.SaveChanges();
                }else
                {
                    UserInfo.Id = (int)id ;
                    _blogContext.UserInformation.Update(UserInfo);
                    _blogContext.SaveChanges();
                }
                
            }catch(Exception)
            {
                return NotFound();
            }
            
            return RedirectToPage("/Admin/About");

        }

        public ActionResult OnPostDelete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var userInfoRemove =_blogContext.UserInformation.Find(id);
            
            if(userInfoRemove == null)
            {
                return NotFound();
            }

            try{
                _blogContext.UserInformation.Remove(userInfoRemove);
                _blogContext.SaveChanges();
            }catch(Exception)
            {
                return NotFound();
            }

            return RedirectToPage("/Admin/About");
        }

        private void SetPageTitle()
        {
             ViewData["Title"] = "Admin Manage About Me";
        }
    }
}
