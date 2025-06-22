using MediatR;
using Portfolio.Core.ResponseBase.GeneralResponse;
using Portfolio.Data.Data;

namespace Portfolio.Core.Features.Portfolio.Commands
{
    public class EditAboutModel : IRequest<BaseResponse<string>>
    {
        public int PortfolioId { get; set; }
        public string About { get; set; }
        public string TokenName { get; set; }
    }
    public class EditAboutHandler : BaseResponseHandler, IRequestHandler<EditAboutModel, BaseResponse<string>>
    {
        private readonly PortfolioDbContext _portfolioDb;

        public EditAboutHandler(PortfolioDbContext portfolioDb)
        {
            this._portfolioDb = portfolioDb;
        }
        public async Task<BaseResponse<string>> Handle(EditAboutModel request, CancellationToken cancellationToken)
        {
            var portfolio = _portfolioDb.Users.FirstOrDefault(p => p.Id == request.PortfolioId);

            if (portfolio is null)
                return Failed<string>(System.Net.HttpStatusCode.NotFound, "Portfolio not found");
            if (portfolio.TokenName != request.TokenName)
                return Failed<string>(System.Net.HttpStatusCode.Unauthorized, "You are not authorized to edit this portfolio");

            portfolio.About = request.About;
            _portfolioDb.Users.Update(portfolio);
            var result = await _portfolioDb.SaveChangesAsync(cancellationToken);
            if (result > 0)
                return Success("Portfolio about section updated successfully");
            return Failed<string>(System.Net.HttpStatusCode.InternalServerError, "Failed to update portfolio about section");

        }
    }
}
