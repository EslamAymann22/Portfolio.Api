using Portfolio.Data.Enums;

namespace Portfolio.Data.Entities
{
    public class Link : BaseEntity
    {
        public string? Url { get; set; }
        public LinksType Type { get; set; } = LinksType.Other;
        public int PortfolioUserId { get; set; }
        public PortfolioUser PortfolioUser { get; set; }

    }
}
