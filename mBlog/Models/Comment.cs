using System;
using System.Collections.Generic;

namespace mBlog.Models
{
    public partial class Comment
    {
        public int Id { get; set; }
        public string CommentText { get; set; }
        public int BlogId { get; set; }
        public string CommentUser { get; set; }
        public DateTime? CreateTime { get; set; }

        public virtual Blog Blog { get; set; }
    }
}
