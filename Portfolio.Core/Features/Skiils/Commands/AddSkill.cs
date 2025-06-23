using MediatR;
using Microsoft.EntityFrameworkCore;
using Portfolio.Core.ResponseBase.GeneralResponse;
using Portfolio.Data.Data;
using Portfolio.Data.Entities;
namespace Portfolio.Core.Features.Skiils.Commands
{
    public class AddSkillModel : IRequest<BaseResponse<string>>
    {
        public string TokenName { get; set; }
        public int portfolioId { get; set; }
        public string Name { get; set; }
    }

    public class AddSkillHandler : BaseResponseHandler, IRequestHandler<AddSkillModel, BaseResponse<string>>
    {
        private readonly PortfolioDbContext _portfolioDb;

        public AddSkillHandler(PortfolioDbContext portfolioDb)
        {
            this._portfolioDb = portfolioDb;
        }
        public async Task<BaseResponse<string>> Handle(AddSkillModel request, CancellationToken cancellationToken)
        {
            var Skill = await _portfolioDb.Skills.Where(S => S.Name == request.Name && S.PortfolioUserId == request.portfolioId).FirstOrDefaultAsync();

            if (Skill is not null)
                return Failed<string>(System.Net.HttpStatusCode.Conflict, "Skill already exists");

            var portfolio = await _portfolioDb.Users.FindAsync(request.portfolioId);

            if (portfolio is null)
                return Failed<string>(System.Net.HttpStatusCode.NotFound, "Portfolio not found");

            if (portfolio.TokenName != request.TokenName)
                return Failed<string>(System.Net.HttpStatusCode.Unauthorized, "You are not authorized to add skill to this portfolio");

            Skill = new Skill
            {
                Name = request.Name,
                PortfolioUserId = request.portfolioId,
            };

            _portfolioDb.Skills.Add(Skill);
            var result = await _portfolioDb.SaveChangesAsync(cancellationToken);
            if (result > 0)
                return Success("Skill added successfully");
            return Failed<string>(System.Net.HttpStatusCode.InternalServerError, "Failed to add skill");
        }
    }
}
