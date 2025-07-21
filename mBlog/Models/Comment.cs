using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mBlog.Models
{
    public partial class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string CommentText { get; set; }
        public int BlogId { get; set; }
        public string CommentUser { get; set; }
        public DateTime? CreateTime { get; set; }

        public virtual Blog Blog { get; set; }
    }
}
