using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HD.EFCore.Extensions.Test
{
    public class MasterHDDbContext: HDDbContext
    {
        public MasterHDDbContext(DbContextOptions<MasterHDDbContext> options) : base(options)
        {

        }
    }
}
