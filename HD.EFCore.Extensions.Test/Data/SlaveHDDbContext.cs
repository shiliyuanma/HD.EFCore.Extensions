using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HD.EFCore.Extensions.Test
{
    public class SlaveHDDbContext: HDDbContext
    {
        public SlaveHDDbContext(DbContextOptions<SlaveHDDbContext> options) : base(options)
        {

        }
    }
}
