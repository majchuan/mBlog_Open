using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using mBlog.Models;
using mBlog.Service;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace mBlog.Pages.Admin
{
    public class LoginModel : PageModel
    {
        private readonly ILogger<LoginModel> _logger;
        private readonly mblogContext _blogContext;
        private readonly SecurityManager _securityManager = new SecurityManager();
        public string Message {get;set;}

        public LoginModel(ILogger<LoginModel> logger , mblogContext blogContext)
        {
            _logger = logger;
            _blogContext = blogContext;
        }

        public void OnGet()
        {
        }

        public void OnGetLogout()
        {
            _securityManager.SignOut(HttpContext);
            RedirectToPage("/Admin/Login");
        }

        public ActionResult OnPost()
        {
            var userEmail = Request.Form["userEmail"];
            var password = Request.Form["password"];
            var authenticationUser = FindBlogUser(userEmail);
            if(AuthenticateUser(authenticationUser, password) == false)
            {
                Message ="Invalid User name or password";
                return Page();
            }else
            {
                try{
                    authenticationUser.LastLogin = DateTime.Now ;
                    _blogContext.BlogUser.Update(authenticationUser);
                    _blogContext.SaveChanges();
                }catch(Exception)
                {
                    return NotFound();
                }
                return RedirectToPage("/Admin/About");
            }
        }

        private BlogUser FindBlogUser(string userEmail)
        {
            var aBlogUser = _blogContext.BlogUser.Include(x => x.Role).SingleOrDefault(x => x.Email == userEmail);
          
            return aBlogUser;
        }
        private Boolean AuthenticateUser(BlogUser authenticationUser, string password)
        {
            if(authenticationUser == null)
            {
                return false ; 
            } 
            
            if(_securityManager.VerifyPassword(password, Convert.FromBase64String(authenticationUser.Password), 
            Convert.FromBase64String(authenticationUser.PasswordSalt)))
            {
                _securityManager.SignIn(HttpContext,authenticationUser);
            }else{
                return false;
            } 

            _securityManager.SignIn(HttpContext,authenticationUser);
            return true;
        }
    }
}
