using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using HowToDoIt.Models.Classes_for_Db;
using System.Collections.Generic;

namespace HowToDoIt.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public bool IsLock { get; set; }
        public virtual ICollection<Instruction> Instructions { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual Profil Profil { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Обратите внимание, что authenticationType должен совпадать с типом, определенным в CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Здесь добавьте утверждения пользователя
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Instruction> Instructions { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Block> Blocks { get; set; }
        public DbSet<Step> Steps { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Profil> Profils { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Connection> Connections { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}