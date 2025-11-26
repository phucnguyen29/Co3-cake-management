using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebClient.Models;
using WebClient.DTOs;

namespace WebClient.Data
{
    public class WebClientContext : DbContext
    {
        public WebClientContext (DbContextOptions<WebClientContext> options)
            : base(options)
        {
        }

        public DbSet<WebClient.Models.User> User { get; set; } = default!;
        public DbSet<WebClient.DTOs.ReadProductDTO> ReadProductDTO { get; set; } = default!;
    }
}
