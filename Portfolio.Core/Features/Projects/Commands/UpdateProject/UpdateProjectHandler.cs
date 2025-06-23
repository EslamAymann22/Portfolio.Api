using MediatR;
using Microsoft.AspNetCore.Http;
using Portfolio.Core.Helper;
using Portfolio.Core.ResponseBase.GeneralResponse;
using Portfolio.Data.Data;
using Portfolio.Data.Entities;

namespace Portfolio.Core.Features.Projects.Commands.UpdateProject
{
    public class UpdateProjectModel : IRequest<BaseResponse<string>>
    {
        public string TokenName { set; get; }
        public int PortfolioId { get; set; }
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string GitHubUrl { get; set; }
        public string LiveUrl { get; set; }
        public IFormFile PhotoUrl { get; set; }
        public List<string> Tools { get; set; } = new List<string>();
        public bool IsTop { get; set; } = false;
    }
    public class UpdateProjectHandler : BaseResponseHandler, IRequestHandler<UpdateProjectModel, BaseResponse<string>>
    {
        private readonly PortfolioDbContext _portfolioDb;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateProjectHandler(PortfolioDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            this._portfolioDb = dbContext;
            this._httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse<string>> Handle(UpdateProjectModel request, CancellationToken cancellationToken)
        {
            var portfolio = _portfolioDb.Users.Find(request.PortfolioId);
            if (portfolio is null)
                return Failed<string>(System.Net.HttpStatusCode.NotFound, "Portfolio not found");
            var project = _portfolioDb.Projects.Find(request.ProjectId);
            if (project is null)
                return Failed<string>(System.Net.HttpStatusCode.NotFound, "Project not found");
            if (project.PortfolioUserId != request.PortfolioId)
                return Failed<string>(System.Net.HttpStatusCode.Forbidden, "You are not allowed to update this project");

            if (portfolio.TokenName != request.TokenName)
                return Failed<string>(System.Net.HttpStatusCode.Unauthorized, "You are not authorized to update project in this portfolio");

            var filePath = FileServices.UploadFile(request.PhotoUrl, "Projects");
            var Request = _httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{Request.Scheme}://{Request.Host}";

            string? NewPhotoUrl = $"{baseUrl}/{filePath}";
            if (request.PhotoUrl is null) NewPhotoUrl = project.PhotoUrl;

            var updatedProject = new Project
            {
                Id = request.ProjectId,
                Name = request.Name,
                Description = request.Description,
                GitHubUrl = request.GitHubUrl,
                LiveUrl = request.LiveUrl,
                PhotoUrl = NewPhotoUrl,
                PortfolioUserId = request.PortfolioId,
                IsTop = request.IsTop
            };



            _portfolioDb.Projects.Update(updatedProject);
            var result = await _portfolioDb.SaveChangesAsync(cancellationToken);

            var toolsToRemove = _portfolioDb.Tools.Where(t => t.ProjectId == request.ProjectId).ToList();
            _portfolioDb.Tools.RemoveRange(toolsToRemove);

            int ProjectId = _portfolioDb.Projects.Where(P => P.Name == request.Name && P.PortfolioUserId == request.PortfolioId).Select(P => P.Id).FirstOrDefault();

            // Add Tools

            foreach (var tool in request.Tools)
            {
                _portfolioDb.Tools.Add(new Tool
                {
                    Name = tool,
                    ProjectId = ProjectId
                });
            }
            var resultTools = await _portfolioDb.SaveChangesAsync(cancellationToken);
            if (resultTools <= 0)
                return Failed<string>(System.Net.HttpStatusCode.InternalServerError, "Failed to add tools to project");



            if (result > 0) return Updated("Project updated successfully");
            return Failed<string>(System.Net.HttpStatusCode.InternalServerError, "Failed to update project");
        }
    }
}
