using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BackgroundService.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace BackgroundService.Data
{
    public class BackgroundServiceContext : IdentityDbContext
    {
        public BackgroundServiceContext (DbContextOptions<BackgroundServiceContext> options)
            : base(options)
        {
        }

        public DbSet<Message> Message { get; set; } = default!;
    }
}
