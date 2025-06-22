namespace Portfolio.Data.Entities
{
    public class Tag : BaseEntity
    {
        public string Name { get; set; }
        public int SkillId { get; set; }
        public Skill Skill { get; set; }
    }
}
