using MediatR;
using Portfolio.Core.ResponseBase.GeneralResponse;
using Portfolio.Data.Data;

namespace Portfolio.Core.Features.Projects.Queries
{
    public class GetAllProjectsDto
    {

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhotoUrl { get; set; }
        public string LiveUrl { get; set; }
        public string GitHubUrl { get; set; }
        public List<string> Tools { get; set; } = new List<string>();


    }

    public class GetAllProjectsModel : IRequest<BaseResponse<List<GetAllProjectsDto>>>
    {
        public int PortfolioId { get; set; }
        public bool GetTop { get; set; } = false;
    }

    public class GetAllProjectsHandler : BaseResponseHandler, IRequestHandler<GetAllProjectsModel, BaseResponse<List<GetAllProjectsDto>>>
    {
        private readonly PortfolioDbContext _portfolioDb;
        public GetAllProjectsHandler(PortfolioDbContext portfolioDb)
        {
            _portfolioDb = portfolioDb;
        }
        public async Task<BaseResponse<List<GetAllProjectsDto>>> Handle(GetAllProjectsModel request, CancellationToken cancellationToken)
        {
            var Portfolio = await _portfolioDb.Users.FindAsync(request.PortfolioId);
            if (Portfolio is null)
                return Failed<List<GetAllProjectsDto>>(System.Net.HttpStatusCode.NotFound, "Portfolio not found");

            var Projects = _portfolioDb.Projects
                .Where(p => p.PortfolioUserId == request.PortfolioId &&
                (request.GetTop ? p.IsTop : true)
                )
                .Select(p => new GetAllProjectsDto
                {
                    Id = p.Id.ToString(),
                    Name = p.Name,
                    Description = p.Description,
                    PhotoUrl = p.PhotoUrl,
                    LiveUrl = p.LiveUrl,
                    GitHubUrl = p.GitHubUrl,
                    Tools = p.Tools.Select(t => t.Name).ToList()
                }).ToList();
            return Success(Projects);
        }
    }
}
