using ContactAppi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactAppi.Data
{
    public class ContactSkillContext : DbContext
    {
        public DbSet<ContactDB> Contacts { get; set; }
        public DbSet<SkillDB> Skills { get; set; }
        //public DbSet<ContactAppi.Models.ContactSkill> ContactSkill { get; set; }

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
            modelBuilder.Entity<ContactDB>()
                .HasKey(c => c.ContactId);

            modelBuilder.Entity<SkillDB>()
                .HasKey(s => s.SkillId);
            
            modelBuilder.Entity<ContactSkill>()
                .HasKey(cs => new { cs.ContactId, cs.SkillId });

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
