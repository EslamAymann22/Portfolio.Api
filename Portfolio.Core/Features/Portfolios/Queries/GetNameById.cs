using MediatR;
using Portfolio.Core.ResponseBase.GeneralResponse;
using Portfolio.Data.Data;

namespace Portfolio.Core.Features.Portfolio.Queries
{
    public class GetPortfolioDataByIdDto
    {
        public string? PortfolioName { get; set; }
        public string? PortfolioPhotoUrl { get; set; }
        public string? PortfolioAbout { get; set; }

    }
    public class GetNameByIdModel : IRequest<BaseResponse<GetPortfolioDataByIdDto>>
    {
        public int PortfolioId { get; set; }
    }

    public class GetNameByIdHandler : BaseResponseHandler, IRequestHandler<GetNameByIdModel, BaseResponse<GetPortfolioDataByIdDto>>
    {
        private readonly PortfolioDbContext _portfolioDb;
        public GetNameByIdHandler(PortfolioDbContext portfolioDb)
        {
            _portfolioDb = portfolioDb;
        }
        public async Task<BaseResponse<GetPortfolioDataByIdDto>> Handle(GetNameByIdModel request, CancellationToken cancellationToken)
        {
            var portfolio = await _portfolioDb.Users.FindAsync(request.PortfolioId);
            if (portfolio is null)
                return Failed<GetPortfolioDataByIdDto>(System.Net.HttpStatusCode.NotFound, "Portfolio not found");
            return Success(new GetPortfolioDataByIdDto
            {
                PortfolioName = portfolio.Name,
                PortfolioAbout = portfolio.About,
                PortfolioPhotoUrl = portfolio.MainPhotoUrl
            });
        }
    }

}
