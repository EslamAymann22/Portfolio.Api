using MediatR;
using Portfolio.Core.ResponseBase.GeneralResponse;
using Portfolio.Data.Data;
using Portfolio.Data.Enums;

namespace Portfolio.Core.Features.Links.Commands
{
    public class EditLinkModel : IRequest<BaseResponse<string>>
    {
        public int LinkId { get; set; }
        public string Url { get; set; }
        public LinksType Type { get; set; }
        public string TokenName { get; set; }
        public int PortfolioId { get; set; }
    }

    public class EditLinkHandler : BaseResponseHandler, IRequestHandler<EditLinkModel, BaseResponse<string>>
    {
        private readonly PortfolioDbContext _portfolioDb;

        public EditLinkHandler(PortfolioDbContext portfolioDb)
        {
            this._portfolioDb = portfolioDb;
        }

        public async Task<BaseResponse<string>> Handle(EditLinkModel request, CancellationToken cancellationToken)
        {
            var link = _portfolioDb.SocialLinks.FirstOrDefault(l => l.Id == request.LinkId);
            if (link is null)
                return Failed<string>(System.Net.HttpStatusCode.NotFound, "Link not found");

            var portfolio = _portfolioDb.Users.FirstOrDefault(p => p.Id == request.PortfolioId);
            if (portfolio is null)
                return Failed<string>(System.Net.HttpStatusCode.NotFound, "Portfolio not found");

            if (link.PortfolioUserId != request.PortfolioId)
                return Failed<string>(System.Net.HttpStatusCode.Forbidden, "You are not allowed to edit this link");

            if (request.Type < 0 || request.Type > LinksType.Other)
                return Failed<string>(System.Net.HttpStatusCode.BadRequest, "Invalid link type");

            link.Url = request.Url;
            link.Type = request.Type;
            _portfolioDb.SocialLinks.Update(link);
            var result = await _portfolioDb.SaveChangesAsync(cancellationToken);
            if (result > 0)
                return Success("Link updated successfully");

            return Failed<string>(System.Net.HttpStatusCode.InternalServerError, "Failed to update link");

        }
    }
}
