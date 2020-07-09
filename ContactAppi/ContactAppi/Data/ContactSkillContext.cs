using ContactAppi.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactAppi.Data
{
    //Install-Package Microsoft.EntityFrameworkCore.Tools
    //Add-Migration MyFirstMigration
    //Update-Database
    public class ContactSkillContext : DbContext
    {
        public DbSet<ContactDB> Contacts { get; set; }
        public DbSet<SkillDB> Skills { get; set; }
        public DbSet<ContactSkill> ContactSkills { get; set; }

        public ContactSkillContext(DbContextOptions<ContactSkillContext> options)
            : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // Define SQLite DataBase
            options.UseSqlite("Data Source=ContactApi.db");
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define link between Contacts and Skills
            modelBuilder.Entity<ContactSkill>()
                .HasKey(cs => new { cs.ContactId, cs.SkillId });

            modelBuilder.Entity<ContactSkill>()
                .HasOne(cs => cs.Contact)
                .WithMany(c => c.ContactSkills)
                .HasForeignKey(cs => cs.ContactId);

            modelBuilder.Entity<ContactSkill>()
                .HasOne(cs => cs.Skill)
                .WithMany(s => s.ContactSkills)
                .HasForeignKey(cs => cs.SkillId);
        }





    }
}
