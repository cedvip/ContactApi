using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactAppi.Models
{
    public class ContactSkill
    {
        public int ContactId { get; set; }
        public virtual ContactDB Contact { get; set; }
        public int SkillId { get; set; }
        public virtual SkillDB Skill { get; set; }
    }
}
