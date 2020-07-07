using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ContactAppi.Models
{
    public class SkillDB
    {
        public int SkillId { get; set; }
        public string SkillName { get; set; }
        public int Level { get; set; }

        public List<ContactSkill> ContactSkills { get; set; }
    }
}
