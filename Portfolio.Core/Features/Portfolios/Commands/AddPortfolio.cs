using MediatR;
using Portfolio.Core.ResponseBase.GeneralResponse;
using Portfolio.Data.Data;
using Portfolio.Data.Entities;

namespace Portfolio.Core.Features.Portfolio.Commands
{
    public class AddPortfolioModel : IRequest<BaseResponse<string>>
    {
        public string Name { get; set; }
        public string TokenName { get; set; }
    }

    public class AddPortfolioHandler : BaseResponseHandler, IRequestHandler<AddPortfolioModel, BaseResponse<string>>
    {
        private readonly PortfolioDbContext _portfolioDb;

        public AddPortfolioHandler(PortfolioDbContext portfolioDb)
        {
            _portfolioDb = portfolioDb;
        }

        public async Task<BaseResponse<string>> Handle(AddPortfolioModel request, CancellationToken cancellationToken)
        {
            var Portfolio = _portfolioDb.Users.Where(P => P.TokenName == request.TokenName).FirstOrDefault();
            if (Portfolio is not null)
                return Failed<string>(System.Net.HttpStatusCode.Conflict, "Portfolio with this token name already exists");

            Portfolio = new PortfolioUser
            {
                Name = request.Name,
                TokenName = request.TokenName
            };
            _portfolioDb.Users.Add(Portfolio);
            var result = await _portfolioDb.SaveChangesAsync(cancellationToken);

            if (result > 0)
                return Created("Portfolio Created!");
            return Failed<string>(System.Net.HttpStatusCode.InternalServerError, "Failed to create portfolio");
        }
    }
}
