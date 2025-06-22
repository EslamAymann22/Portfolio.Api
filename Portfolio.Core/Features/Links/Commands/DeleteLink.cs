using MediatR;
using Portfolio.Core.ResponseBase.GeneralResponse;
using Portfolio.Data.Data;

namespace Portfolio.Core.Features.Links.Commands
{
    public class DeleteLinkModel : IRequest<BaseResponse<string>>
    {
        public int LinkId { get; set; }
        public string TokenName { get; set; }
        public int PortfolioId { get; set; }

    }


    public class DeleteLinkHandler : BaseResponseHandler, IRequestHandler<DeleteLinkModel, BaseResponse<string>>
    {
        private readonly PortfolioDbContext _portfolioDb;
        public DeleteLinkHandler(PortfolioDbContext portfolioDb)
        {
            _portfolioDb = portfolioDb;
        }
        public async Task<BaseResponse<string>> Handle(DeleteLinkModel request, CancellationToken cancellationToken)
        {
            var link = _portfolioDb.SocialLinks.FirstOrDefault(l => l.Id == request.LinkId);
            if (link is null)
                return Failed<string>(System.Net.HttpStatusCode.NotFound, "Link not found");

            var portfolio = _portfolioDb.Users.FirstOrDefault(p => p.Id == request.PortfolioId);

            if (portfolio.TokenName != request.TokenName)
                return Failed<string>(System.Net.HttpStatusCode.Unauthorized, "You are not authorized to delete this link");
            _portfolioDb.SocialLinks.Remove(link);
            var result = await _portfolioDb.SaveChangesAsync(cancellationToken);
            if (result > 0)
                return Success("Link deleted successfully");
            return Failed<string>(System.Net.HttpStatusCode.InternalServerError, "Failed to delete link");
        }
    }

}
