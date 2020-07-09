using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ContactAppi.Models
{
    public class SkillDB
    {
        [Key]
        public int SkillId { get; set; }
        public string SkillName { get; set; }
        public int Level { get; set; }

        public List<ContactSkill> ContactSkills { get; set; }
    }
}
