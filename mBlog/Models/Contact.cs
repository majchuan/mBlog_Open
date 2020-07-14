using System;
using System.Collections.Generic;

namespace mBlog.Models
{
    public partial class Contact
    {
        public int Id { get; set; }
        public string ContactName { get; set; }
        public string Email { get; set; }
        public string ContactMessage { get; set; }
    }
}
