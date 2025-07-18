using System;
using System.Collections.Generic;

namespace mBlog.Models
{
    public partial class BlogUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public int RoleId { get; set; }
        public string Email { get; set; }
        public DateTime? LastLogin { get; set; }

        public virtual Role Role { get; set; }
    }
}
