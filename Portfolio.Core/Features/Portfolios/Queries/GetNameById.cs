using MediatR;
using Portfolio.Core.ResponseBase.GeneralResponse;
using Portfolio.Data.Data;

namespace Portfolio.Core.Features.Portfolio.Queries
{
    public class GetNameByIdDto
    {
        public string PortfolioName { get; set; }
    }


    public class GetNameByIdModel : IRequest<BaseResponse<GetNameByIdDto>>
    {
        public int PortfolioId { get; set; }
    }

    public class GetNameByIdHandler : BaseResponseHandler, IRequestHandler<GetNameByIdModel, BaseResponse<GetNameByIdDto>>
    {
        private readonly PortfolioDbContext _portfolioDb;
        public GetNameByIdHandler(PortfolioDbContext portfolioDb)
        {
            _portfolioDb = portfolioDb;
        }
        public async Task<BaseResponse<GetNameByIdDto>> Handle(GetNameByIdModel request, CancellationToken cancellationToken)
        {
            var portfolio = await _portfolioDb.Users.FindAsync(request.PortfolioId);
            if (portfolio is null)
                return Failed<GetNameByIdDto>(System.Net.HttpStatusCode.NotFound, "Portfolio not found");
            return Success(new GetNameByIdDto { PortfolioName = portfolio.Name });
        }
    }

}
