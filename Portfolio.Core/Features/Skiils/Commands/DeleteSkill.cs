using MediatR;
using Portfolio.Core.ResponseBase.GeneralResponse;
using Portfolio.Data.Data;

namespace Portfolio.Core.Features.Skiils.Commands
{
    public class DeleteSkillModel : IRequest<BaseResponse<string>>
    {
        public int SkillId { get; set; }
        public int PortfolioId { get; set; }
        public string TokenName { get; set; }
    }


    public class DeleteSkillHandler : BaseResponseHandler, IRequestHandler<DeleteSkillModel, BaseResponse<string>>
    {
        private readonly PortfolioDbContext _portfolioDb;

        public DeleteSkillHandler(PortfolioDbContext portfolioDb)
        {
            this._portfolioDb = portfolioDb;
        }

        public async Task<BaseResponse<string>> Handle(DeleteSkillModel request, CancellationToken cancellationToken)
        {
            var portfolio = _portfolioDb.Users.Find(request.PortfolioId);
            if (portfolio is null)
                return Failed<string>(System.Net.HttpStatusCode.NotFound, "Portfolio is not found");
            if (portfolio.TokenName != request.TokenName)
                return Failed<string>(System.Net.HttpStatusCode.Unauthorized, "You are not authorized to delete this skill");

            var skill = _portfolioDb.Skills.Where(S => S.Id == request.SkillId).FirstOrDefault();
            if (skill is null || skill.PortfolioUserId != portfolio.Id)
                return Failed<string>(System.Net.HttpStatusCode.NotFound, "Not allowed to delete this skill");

            _portfolioDb.Skills.Remove(skill);
            await _portfolioDb.SaveChangesAsync();

            return Success("skill deleted !!");

        }
    }
}
