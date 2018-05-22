using System;
using System.Collections.Generic;

namespace HD.EFCore.Extensions.Test.Entity
{
    public partial class Comment
    {
        public int Id { get; set; }
        public int BlogId { get; set; }
        public int UserId { get; set; }
        public int ToUserId { get; set; }
        public string Body { get; set; }
        public DateTime CreateTime { get; set; }

        public Blog Blog { get; set; }
    }
}
