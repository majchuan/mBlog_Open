using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using mBlog.Models;
using Microsoft.EntityFrameworkCore;
using Markdig;

namespace mBlog.Pages
{
    public class BlogModel : PageModel
    {
        private readonly ILogger<BlogModel> _logger;
        private readonly mblogContext _blogContext ;

        public Blog BlogDetail {get ;set;}

        public BlogUser BlogUser {get;set;}

        [BindProperty]
        public Comment BlogComment {get;set;}

        public BlogModel(ILogger<BlogModel> logger, mblogContext blogContext)
        {
            _logger = logger;
            _blogContext = blogContext;
        }

        public ActionResult OnGet(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }

            BlogDetail = _blogContext.Blog.Include(b => b.Comment).ToList().Find(x => x.Id == id);
            BlogUser = _blogContext.BlogUser.FirstOrDefault();
            BlogComment = new Comment {
                CommentUser = string.Empty,
                CommentText = string.Empty
            };
            var markDownPipeline = new Markdig.MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            if(BlogDetail != null){
                BlogDetail.BlogText = Markdig.Markdown.ToHtml(BlogDetail.BlogText, markDownPipeline);
            }

            return Page();
        }

        public ActionResult OnPost(int? id)
        {
            try{
                BlogDetail = _blogContext.Blog.Find(id);
                BlogComment.Blog = BlogDetail;
                BlogComment.CreateTime = DateTime.Now;
                BlogDetail.Comment.Add(BlogComment);
                _blogContext.SaveChanges();
            }catch(Exception)
            {
                return NotFound();
            }

            return RedirectToPage("/Blog", new { id = BlogDetail.Id});
       
        }
    }
}
