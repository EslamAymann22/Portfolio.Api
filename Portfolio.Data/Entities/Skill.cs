namespace Portfolio.Data.Entities
{
    public class Skill : BaseEntity
    {
        public IEnumerable<Tag> Tags { get; set; } = new List<Tag>();
        public string Name { get; set; }
        public int PortfolioUserId { get; set; }
        public PortfolioUser portfolioUser { get; set; }
    }
}
