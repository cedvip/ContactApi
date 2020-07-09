using System.ComponentModel.DataAnnotations;

namespace ContactAppi.Models
{
    public class Skill
    {
        public int id { get; set; }

        [Required]
        public string SkillName { get; set; }

        [Required]
        public int Level { get; set; }
    }
}
