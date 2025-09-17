using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using mBlog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Npgsql;
using Microsoft.AspNetCore.HttpOverrides;
using Google.Cloud.AspNetCore.DataProtection.Kms;
using Google.Cloud.AspNetCore.DataProtection.Storage;
using Microsoft.AspNetCore.DataProtection;
using mBlog.Service;

namespace mBlog
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            /*
             services.AddDataProtection()
                // Store keys in Cloud Storage so that multiple instances
                // of the web application see the same keys.
                .PersistKeysToGoogleCloudStorage(
                    Configuration["DataProtection:Bucket"],
                    Configuration["DataProtection:Object"])
                // Protect the keys with Google KMS for encryption and fine-
                // grained access control.
                .ProtectKeysWithGoogleKms(
                    Configuration["DataProtection:KmsKeyName"]);
            */

            services.AddAuthentication(options => {
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options => {
                options.LoginPath = "/Admin/Login";
                options.LogoutPath ="/Admin/Login/Signout";
                options.AccessDeniedPath ="/Error";
                options.Cookie.SameSite = SameSiteMode.Lax;
            });

            services.AddRazorPages();
            services.AddDbContext<mblogContext>(optionBuilder => 
                optionBuilder.UseNpgsql(
                    //Configuration.GetConnectionString("defaultConnection")
                    //Configuration.GetValue<string>("DEFAULT_CONNECTION")
                    "host=172.17.0.1;database=mablog;username=mblog_app_user;password=abc"
                    //Configuration.GetConnectionString("cloudProdConnection")
                    /*
                    new NpgsqlConnectionStringBuilder(Configuration.GetConnectionString("cloudProdConnection"))
                    {
                        SslMode = SslMode.Disable 
                    }.ConnectionString
                    */
                    ));
       
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions(){
                ForwardedHeaders = ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedFor 
            });
            
            app.UseForwardHttpToHttps(option =>{
                option.SwitchForwardToHtttps = Convert.ToBoolean(Configuration["HttpForwardToHttps:switch"]);
            });
                      
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
