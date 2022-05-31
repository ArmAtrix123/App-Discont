using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Kursach.Models
{
    public class ApplicationContex : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Modeli> Modelis { get; set; }
        public ApplicationContex(DbContextOptions<ApplicationContex> options)
            :base (options)
        {
            Database.EnsureCreated();
        }
    }
}
