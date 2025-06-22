using MediatR;
using Portfolio.Core.ResponseBase.GeneralResponse;
using Portfolio.Data.Data;

namespace Portfolio.Core.Features.Projects.Commands
{
    public class DeleteProjectModel : IRequest<BaseResponse<string>>
    {

        public string TokenName { get; set; }
        public int portfolioId { get; set; }
        public int projectId { get; set; }

    }

    public class DeleteProjectHandler : BaseResponseHandler, IRequestHandler<DeleteProjectModel, BaseResponse<string>>
    {
        private readonly PortfolioDbContext _portfolioDb;

        public DeleteProjectHandler(PortfolioDbContext portfolioDb)
        {
            _portfolioDb = portfolioDb;
        }

        public async Task<BaseResponse<string>> Handle(DeleteProjectModel request, CancellationToken cancellationToken)
        {
            var portfolio = _portfolioDb.Users.Find(request.portfolioId);
            if (portfolio is null)
                return Failed<string>(System.Net.HttpStatusCode.NotFound, "Portfolio not found");
            var project = _portfolioDb.Projects.Find(request.projectId);
            if (project is null)
                return Failed<string>(System.Net.HttpStatusCode.NotFound, "Project not found");

            if (portfolio.TokenName != request.TokenName)
                return Failed<string>(System.Net.HttpStatusCode.Unauthorized, "You are not authorized to delete project in this portfolio");

            if (project.PortfolioUserId != request.portfolioId)
                return Failed<string>(System.Net.HttpStatusCode.Forbidden, "You are not allowed to delete this project");

            _portfolioDb.Projects.Remove(project);
            var result = await _portfolioDb.SaveChangesAsync(cancellationToken);
            if (result > 0)
                return Success("Project deleted successfully");
            return Failed<string>(System.Net.HttpStatusCode.InternalServerError, "Failed to delete project");

        }
    }

}
