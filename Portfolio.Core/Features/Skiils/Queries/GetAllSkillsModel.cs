using MediatR;
using Microsoft.EntityFrameworkCore;
using Portfolio.Core.ResponseBase.GeneralResponse;
using Portfolio.Data.Data;

namespace Portfolio.Core.Features.Skiils.Queries
{
    public class GetAllSkillsDto
    {

        public string Name { get; set; }
        public IEnumerable<string> Tags { get; set; } = new List<string>();
    }

    public class GetAllSkillsModel : IRequest<BaseResponse<IEnumerable<GetAllSkillsDto>>>
    {
        public int PortfolioId { get; set; }
    }

    public class GetAllSkillsHandler : BaseResponseHandler, IRequestHandler<GetAllSkillsModel, BaseResponse<IEnumerable<GetAllSkillsDto>>>
    {
        private readonly PortfolioDbContext _portfolioDb;
        public GetAllSkillsHandler(PortfolioDbContext portfolioDb)
        {
            _portfolioDb = portfolioDb;
        }
        public async Task<BaseResponse<IEnumerable<GetAllSkillsDto>>> Handle(GetAllSkillsModel request, CancellationToken cancellationToken)
        {
            var portfolio = await _portfolioDb.Users.FindAsync(request.PortfolioId);
            if (portfolio is null)
                return Failed<IEnumerable<GetAllSkillsDto>>(System.Net.HttpStatusCode.NotFound, "Portfolio not found");


            var skills = await _portfolioDb.Skills
                .Where(s => s.PortfolioUserId == request.PortfolioId)
                .Select(s => new GetAllSkillsDto
                {
                    Name = s.Name,
                    Tags = s.Tags.Select(t => t.Name).ToList()
                })
                .ToListAsync(cancellationToken);

            return Success(skills.AsEnumerable());
        }
    }

}
