using System;
using System.Collections.Generic;

namespace HD.EFCore.Extensions.Test.Entity
{
    public partial class Blog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int CommentCount { get; set; }
        public DateTime CreateTime { get; set; }

        public List<Comment> Comments { get; set; }
    }
}
