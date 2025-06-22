namespace Portfolio.Data.Entities
{
    public class Project : BaseEntity
    {

        public string? Name { get; set; }
        public string? PhotoUrl { get; set; }
        public string? Description { get; set; }
        public IEnumerable<Tool> Tools { get; set; } = new List<Tool>();
        public string? GitHubUrl { get; set; }
        public string? LiveUrl { get; set; }
        public bool IsTop { get; set; } = false;

        public int PortfolioUserId { get; set; }
        public PortfolioUser? PortfolioUser { get; set; }


    }
}
