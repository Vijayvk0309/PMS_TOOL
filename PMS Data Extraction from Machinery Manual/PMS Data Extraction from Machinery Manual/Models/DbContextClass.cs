using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace PMS_Data_Extraction_from_Machinery_Manual.Models
{
    public class DbContextClass : DbContext
    {

        public DbContextClass(DbContextOptions<DbContextClass> options)
             : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<ExcelValidate> ExcelValidate { get; set; }
        /* public DbSet<UserCreate> UserCreates { get; set; }*/
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>(entity => {
                entity.HasKey(k => k.Id);
            });
        }
    }
}
