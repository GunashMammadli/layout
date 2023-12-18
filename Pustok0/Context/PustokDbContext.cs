using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pustok0.Models;
using System.Drawing;

namespace Pustok0.Context
{
    public class PustokDbContext : IdentityDbContext
    {
        public PustokDbContext(DbContextOptions options) : base(options) { }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<ProductTag> ProductTags { get; set; }
        public DbSet<ProductAuthor> ProductAuthors { get; set; }
        public DbSet<BlogTag> BlogTags { get; set; }
        public DbSet<BlogAuthor> BlogAuthors { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Setting>()
                .HasData(new Setting
                {
                    Address = "Baku, Metbuat prospekti",
                    Email = "gunash@gmail.com",
                    Number1 = "+994107181535",
                    Logo = "assets/image/logo.png",
                    Id = 1
                });
            base.OnModelCreating(modelBuilder);
        }
    }

}
