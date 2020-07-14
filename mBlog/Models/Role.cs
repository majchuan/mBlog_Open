using System;
using System.Collections.Generic;

namespace mBlog.Models
{
    public partial class Role
    {
        public Role()
        {
            BlogUser = new HashSet<BlogUser>();
        }

        public int Id { get; set; }
        public string RoleName { get; set; }

        public virtual ICollection<BlogUser> BlogUser { get; set; }
    }
}
