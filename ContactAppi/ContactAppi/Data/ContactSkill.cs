namespace ContactAppi.Models
{
    public class ContactSkill
    {
        public int ContactId { get; set; }
        public ContactDB Contact { get; set; }
        public int SkillId { get; set; }
        public SkillDB Skill { get; set; }
    }
}
