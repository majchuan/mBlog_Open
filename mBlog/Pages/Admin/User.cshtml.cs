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
    public class UserModel : PageModel
    {
        private readonly ILogger<ContactModel> _logger;
        private readonly mblogContext _blogContext;

        [BindProperty]
        public BlogUser BlogAdminUser {get;set;}
        public readonly SecurityManager _SecurityManager = new SecurityManager();

        public String Message{get;set;}
        public UserModel(ILogger<ContactModel> logger , mblogContext blogContext)
        {
            _logger = logger;
            _blogContext = blogContext;
        }

        public void OnGet()
        {
            SetPageTitle();
            BlogAdminUser = _blogContext.BlogUser.FirstOrDefault();
        }
        public ActionResult OnPost(int id)
        {
            SetPageTitle();
            var newUserName = Request.Form["userName"];
            var currentPassword = Request.Form["oldPassword"];
            var newPassword = Request.Form["newPassword"];
            var newConfirmationPassword = Request.Form["newConfirmationPassword"];
            var adminUser = _blogContext.BlogUser.Find(id);
            byte[] passwordHash;
            byte[] passwordSalt;

            if(newPassword != newConfirmationPassword)
            {
                Message = "new password is not same";
                return Page(); 
            }

            
            if(_SecurityManager.VerifyPassword(currentPassword, 
            Convert.FromBase64String(adminUser.Password), 
            Convert.FromBase64String(adminUser.PasswordSalt)) == true)
            {
                
                _SecurityManager.CreatePasswordHash(newPassword,out passwordHash, out passwordSalt);
                adminUser.Username = newUserName ;
                adminUser.Password = Convert.ToBase64String(passwordHash);
                adminUser.PasswordSalt = Convert.ToBase64String(passwordSalt);
                try{
                    _blogContext.BlogUser.Update(adminUser);
                    _blogContext.SaveChanges();
                }catch(Exception)
                {
                    return NotFound();
                } 
            
            }else{
                Message ="please enter correctly current password";
                return Page();
            }
            

            return RedirectToPage("/Admin/User");
        }

        private void SetPageTitle()
        {
            ViewData["Title"] = "Admin Contact Manage page";
        }

    }
}
