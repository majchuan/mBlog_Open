using System;
using Microsoft.AspNetCore.Http;
using mBlog.Models;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Cryptography;
using System.Text;


namespace mBlog.Service
{
    public class SecurityManager
    {
        public async void SignIn(HttpContext httpContext, BlogUser blogUser)
        {
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
            identity.AddClaims(GetUserClaims(blogUser));
            var principal = new ClaimsPrincipal(identity);
            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, 
            new AuthenticationProperties
                { 
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(20)
                });
        }

        public async void SignOut(HttpContext httpContext)
        {
            await httpContext.SignOutAsync();
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if(string.IsNullOrEmpty(password) || string.IsNullOrWhiteSpace(password)) throw new Exception("password can not be null");
            using(var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.ASCII.GetBytes(password));
            }
        }

        public Boolean VerifyPassword(string password, Byte[] passwordHash, Byte[] passwordSalt )
        {
            if(passwordHash.Length != 64) return false;
            if(passwordSalt.Length != 128) return false;

            using(var hmac = new HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(Encoding.ASCII.GetBytes(password));
                for(int i = 0; i < computeHash.Length ; i++)
                {
                    if(computeHash[i] != passwordHash[i]) return false ;
                }
            }

            return true;
        }

        private IEnumerable<Claim> GetUserClaims(BlogUser blogUser)
        {
            IList<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name,blogUser.Username));
            claims.Add(new Claim(ClaimTypes.Role,blogUser.Role.RoleName));

            return claims;
        }


    }
}