using MediatR;
using Microsoft.AspNetCore.Http;
using Portfolio.Core.Helper;
using Portfolio.Core.ResponseBase.GeneralResponse;
using Portfolio.Data.Data;
using Portfolio.Data.Entities;

namespace Portfolio.Core.Features.Projects.Commands.AddProject
{
    public class AddProjectModel : IRequest<BaseResponse<string>>
    {
        public string TokenName { set; get; }
        public int PortfolioId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string GitHubUrl { get; set; }
        public string LiveUrl { get; set; }
        public IFormFile PhotoUrl { get; set; }
        public List<string> Tools { get; set; } = new List<string>();
        public bool IsTop { get; set; } = false;
    }


    public class AddProjectHandler : BaseResponseHandler, IRequestHandler<AddProjectModel, BaseResponse<string>>
    {
        private readonly PortfolioDbContext _portfolioDb;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddProjectHandler(PortfolioDbContext portfolioDb, IHttpContextAccessor httpContextAccessor)
        {
            _portfolioDb = portfolioDb;
            this._httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse<string>> Handle(AddProjectModel request, CancellationToken cancellationToken)
        {
            var Portfolio = await _portfolioDb.Users.FindAsync(request.PortfolioId);

            if (Portfolio is null)
                return Failed<string>(System.Net.HttpStatusCode.NotFound, "Portfolio not found");

            if (Portfolio.TokenName != request.TokenName)
                return Failed<string>(System.Net.HttpStatusCode.Unauthorized, "You are not authorized to add project to this portfolio");


            // Add Photo

            var filePath = FileServices.UploadFile(request.PhotoUrl, "Projects");
            var Request = _httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            string? NewPhotoUrl;
            if (request.PhotoUrl is null)
                NewPhotoUrl = null;
            else
                NewPhotoUrl = $"{baseUrl}/{filePath}";

            var NewProject = new Project
            {
                Name = request.Name,
                Description = request.Description,
                GitHubUrl = request.GitHubUrl,
                LiveUrl = request.LiveUrl,
                PhotoUrl = NewPhotoUrl,
                //Tools = request.Tools,
                PortfolioUserId = request.PortfolioId,
                IsTop = request.IsTop
            };

            _portfolioDb.Projects.Add(NewProject);
            var result = await _portfolioDb.SaveChangesAsync(cancellationToken);

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

            if (result > 0)
                return Success("Project added successfully");

            return Failed<string>(System.Net.HttpStatusCode.InternalServerError, "Failed to add project");

        }

    }
}
