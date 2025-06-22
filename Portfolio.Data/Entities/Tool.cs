namespace Portfolio.Data.Entities
{
    public class Tool : BaseEntity
    {
        public string Name { get; set; }
        public Project Project { get; set; }
        public int ProjectId { get; set; }
    }
}
