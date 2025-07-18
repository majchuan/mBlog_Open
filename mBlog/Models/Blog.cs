using System;
using System.Collections.Generic;

namespace mBlog.Models
{
    public partial class Blog
    {
        public Blog()
        {
            Comment = new HashSet<Comment>();
        }

        public int Id { get; set; }
        public string BlogTitle { get; set; }
        public string BlogSubTitle { get; set; }
        public string BlogText { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? ModifyTime { get; set; }

        public virtual ICollection<Comment> Comment { get; set; }
    }
}
