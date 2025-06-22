using MediatR;
using Microsoft.AspNetCore.Http;
using Portfolio.Core.Helper;
using Portfolio.Core.ResponseBase.GeneralResponse;
using Portfolio.Data.Data;

namespace Portfolio.Core.Features.Portfolio.Commands
{
    public class EditMainPhotoModel : IRequest<BaseResponse<string>>
    {
        public string TokenName { get; set; }
        public IFormFile? Photo { get; set; }
    }

    public class EditMainPhotoHandler : BaseResponseHandler, IRequestHandler<EditMainPhotoModel, BaseResponse<string>>
    {
        private readonly PortfolioDbContext _portfolioDb;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public EditMainPhotoHandler(PortfolioDbContext portfolioDb, IHttpContextAccessor httpContextAccessor)
        {
            this._portfolioDb = portfolioDb;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<BaseResponse<string>> Handle(EditMainPhotoModel request, CancellationToken cancellationToken)
        {
            if (request.Photo == null)
                return Failed<string>(System.Net.HttpStatusCode.BadRequest, "No photo provided");

            var portfolio = _portfolioDb.Users.FirstOrDefault(p => p.TokenName == request.TokenName);

            if (portfolio is null)
                return Failed<string>(System.Net.HttpStatusCode.NotFound, "Portfolio not found");
            if (portfolio.TokenName != request.TokenName)
                return Failed<string>(System.Net.HttpStatusCode.Unauthorized, "You are not authorized to edit this portfolio");

            var filePath = FileServices.UploadFile(request.Photo, "MainPhotos");
            var Request = _httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            portfolio.MainPhotoUrl = $"{baseUrl}/{filePath}";
            _portfolioDb.Users.Update(portfolio);
            var result = await _portfolioDb.SaveChangesAsync(cancellationToken);
            // Here you would typically save the NewPhotoUrl to the database or perform other actions
            return Success("Photo updated!!");
        }
    }

}
