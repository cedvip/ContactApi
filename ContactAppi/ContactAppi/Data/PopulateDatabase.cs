using ContactAppi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace ContactAppi.Data
{
    public class PopulateDatabase
    {

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new ContactSkillContext(serviceProvider.GetRequiredService<DbContextOptions<ContactSkillContext>>());
            PopulateSkills(context);
            
        }

        private static void PopulateSkills(ContactSkillContext context)
        {
            if (context.Skills.Any())
                return;

            context.Skills.AddRange(
                new SkillDB
                {
                    SkillName = "Junior",
                    Level = 3
                },
                new SkillDB
                {
                    SkillName = "Senior",
                    Level = 6
                },
                new SkillDB
                {
                    SkillName = "Expert",
                    Level = 9
                });

            context.SaveChanges();
        }
    }
}
