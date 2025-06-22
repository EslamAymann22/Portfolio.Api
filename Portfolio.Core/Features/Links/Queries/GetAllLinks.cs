using MediatR;
using Portfolio.Core.ResponseBase.GeneralResponse;
using Portfolio.Data.Data;

namespace Portfolio.Core.Features.Links.Queries
{
    public class GetAllLinksDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string type { get; set; }
    }
    public class GetAllLinksModel : IRequest<BaseResponse<List<GetAllLinksDto>>>
    {
        public int portfolioId { get; set; }
    }

    public class GetAllLinksHandler : BaseResponseHandler, IRequestHandler<GetAllLinksModel, BaseResponse<List<GetAllLinksDto>>>
    {
        private readonly PortfolioDbContext _portfolioDb;

        public GetAllLinksHandler(PortfolioDbContext portfolioDb)
        {
            this._portfolioDb = portfolioDb;
        }

        public async Task<BaseResponse<List<GetAllLinksDto>>> Handle(GetAllLinksModel request, CancellationToken cancellationToken)
        {
            var portfolio = _portfolioDb.Users.FirstOrDefault(p => p.Id == request.portfolioId);
            if (portfolio is null)
                return Failed<List<GetAllLinksDto>>(System.Net.HttpStatusCode.NotFound, "Portfolio not found");

            var Links = _portfolioDb.SocialLinks.Where(L => L.PortfolioUserId == request.portfolioId)
                .Select(L => new GetAllLinksDto
                {
                    Id = L.Id,
                    Url = L.Url,
                    type = L.Type.ToString()
                }).ToList();

            return Success(Links);

        }
    }
}
