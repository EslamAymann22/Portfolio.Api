
using MediatR;
using Portfolio.Core.ResponseBase.GeneralResponse;
using Portfolio.Data.Data;

namespace Portfolio.Core.Features.Portfolio.Commands
{
    public class ChangeNameModel : IRequest<BaseResponse<string>>
    {
        public int PortfolioId { set; get; }
        public string Name { set; get; }
        public string TokenName { set; get; }
    }

    public class ChangeNameHandler : BaseResponseHandler, IRequestHandler<ChangeNameModel, BaseResponse<string>>
    {
        private readonly PortfolioDbContext _portfolioDb;

        public ChangeNameHandler(PortfolioDbContext portfolioDb)
        {
            _portfolioDb = portfolioDb;
        }

        public async Task<BaseResponse<string>> Handle(ChangeNameModel request, CancellationToken cancellationToken)
        {
            var portfolio = _portfolioDb.Users.FirstOrDefault(p => p.Id == request.PortfolioId);
            if (portfolio is null)
                return Failed<string>(System.Net.HttpStatusCode.NotFound, "Portfolio not found");

            if (portfolio.TokenName != request.TokenName)
                return Failed<string>(System.Net.HttpStatusCode.Unauthorized, "You are not authorized to change the name of this portfolio");

            portfolio.Name = request.Name;
            _portfolioDb.Users.Update(portfolio);
            var result = await _portfolioDb.SaveChangesAsync(cancellationToken);
            if (result > 0)
                return Success("Portfolio name changed successfully");
            return Failed<string>(System.Net.HttpStatusCode.InternalServerError, "Failed to change portfolio name");

        }
    }

}
