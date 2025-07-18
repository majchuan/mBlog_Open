using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using mBlog.Models;
using Markdig;

namespace mBlog.Pages
{
    public class AboutModel : PageModel
    {
        private readonly ILogger<AboutModel> _logger;
        private readonly mblogContext _blogContext ;

        public UserInformation aboutMe {get;set;}

        public String MarkDownUserInfor {get;set;}


        public AboutModel(ILogger<AboutModel> logger , mblogContext blogContext)
        {
            _logger = logger;
            _blogContext = blogContext;
        }

        public ActionResult OnGet()
        {
            aboutMe = _blogContext.UserInformation.OrderByDescending(x => x.Id).FirstOrDefault();
            var markDownPipeline = new Markdig.MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            if(aboutMe != null){
                MarkDownUserInfor = Markdig.Markdown.ToHtml(aboutMe.UserIntro, markDownPipeline);
            }

            /*
            if(MarkDownUserInfor == null || MarkDownUserInfor.Length == 0)
            {
                return NotFound();
            }
            */

            return Page();
        }
    }
}
