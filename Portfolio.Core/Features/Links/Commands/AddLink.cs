using MediatR;
using Portfolio.Core.ResponseBase.GeneralResponse;
using Portfolio.Data.Data;
using Portfolio.Data.Entities;
using Portfolio.Data.Enums;

namespace Portfolio.Core.Features.Links.Commands
{
    public class AddLinkModel : IRequest<BaseResponse<string>>
    {
        public string TokenName { get; set; }
        public string Url { get; set; }
        public LinksType linkType { get; set; }
        public int PortfolioId { get; set; }

    }

    public class AddLinkHandler : BaseResponseHandler, IRequestHandler<AddLinkModel, BaseResponse<string>>
    {
        private readonly PortfolioDbContext _portfolioDb;

        public AddLinkHandler(PortfolioDbContext portfolioDb)
        {
            this._portfolioDb = portfolioDb;
        }

        public async Task<BaseResponse<string>> Handle(AddLinkModel request, CancellationToken cancellationToken)
        {
            var portfolio = _portfolioDb.Users.FirstOrDefault(p => p.Id == request.PortfolioId);

            if (portfolio is null)
                return Failed<string>(System.Net.HttpStatusCode.NotFound, "Portfolio not found");
            if (portfolio.TokenName != request.TokenName)
                return Failed<string>(System.Net.HttpStatusCode.Unauthorized, "You are not authorized to add links to this portfolio");


            if (request.linkType < 0 || request.linkType > LinksType.Other)
                return Failed<string>(System.Net.HttpStatusCode.BadRequest, "Invalid link type");

            var link = new Link
            {
                PortfolioUserId = request.PortfolioId,
                Url = request.Url,
                Type = request.linkType
            };

            _portfolioDb.SocialLinks.Add(link);
            var result = await _portfolioDb.SaveChangesAsync(cancellationToken);

            if (result > 0)
                return Created("Link added successfully!");
            return Failed<string>(System.Net.HttpStatusCode.InternalServerError, "Failed to add link");
        }
    }


}
