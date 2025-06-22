namespace Portfolio.Data.Entities
{
    public class PortfolioUser : BaseEntity
    {

        public string TokenName { get; set; }
        public string? Name { get; set; }
        public string? MainPhotoUrl { get; set; }
        public string? About { get; set; }



        public IEnumerable<Link> Links { get; set; } = new List<Link>();
        public IEnumerable<Project> Projects { get; set; } = new List<Project>();
        public IEnumerable<Skill> Skills { get; set; } = new List<Skill>();
    }
}
