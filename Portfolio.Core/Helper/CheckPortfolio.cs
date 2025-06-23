using Portfolio.Data.Data;

namespace Portfolio.Core.Helper
{
    public static class CheckPortfolio
    {

        public static bool IsValidPortfolioId(int portfolioId, string TokenName, PortfolioDbContext portfolioDb)
        {

            var portfolio = portfolioDb.Users.Find(portfolioId);
            if (portfolio is null)
                return false;
            if (portfolio.TokenName != TokenName)
                return false;
            return true;

        }

    }
}
