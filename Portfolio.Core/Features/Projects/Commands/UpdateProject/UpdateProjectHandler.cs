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
        private readonly PortfolioDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateProjectHandler(PortfolioDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            this._dbContext = dbContext;
            this._httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse<string>> Handle(UpdateProjectModel request, CancellationToken cancellationToken)
        {
            var portfolio = _dbContext.Users.Find(request.PortfolioId);
            if (portfolio is null)
                return Failed<string>(System.Net.HttpStatusCode.NotFound, "Portfolio not found");
            var project = _dbContext.Projects.Find(request.ProjectId);
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
                Tools = request.Tools,
                PortfolioUserId = request.PortfolioId,
                IsTop = request.IsTop
            };

            _dbContext.Projects.Update(updatedProject);
            var result = await _dbContext.SaveChangesAsync(cancellationToken);

            if (result > 0) return Updated("Project updated successfully");
            return Failed<string>(System.Net.HttpStatusCode.InternalServerError, "Failed to update project");
        }
    }
}
