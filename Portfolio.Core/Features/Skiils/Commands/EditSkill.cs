using MediatR;
using Microsoft.EntityFrameworkCore;
using Portfolio.Core.ResponseBase.GeneralResponse;
using Portfolio.Data.Data;
using Portfolio.Data.Entities;

namespace Portfolio.Core.Features.Skiils.Commands
{
    public class EditSkillModel : IRequest<BaseResponse<string>>
    {
        public string TokenName { get; set; }
        public int PortfolioId { get; set; }
        public int SkillId { get; set; }
        public string Name { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
    }

    public class EditSkillHandler : BaseResponseHandler, IRequestHandler<EditSkillModel, BaseResponse<string>>
    {
        private readonly PortfolioDbContext _portfolioDb;

        public EditSkillHandler(PortfolioDbContext portfolioDb)
        {
            this._portfolioDb = portfolioDb;
        }

        public async Task<BaseResponse<string>> Handle(EditSkillModel request, CancellationToken cancellationToken)
        {
            var portfolio = _portfolioDb.Users.Find(request.PortfolioId);

            #region Checker
            if (portfolio is null)
                return Failed<string>(System.Net.HttpStatusCode.NotFound, "Portfolio not found");

            if (portfolio.TokenName != request.TokenName)
                return Failed<string>(System.Net.HttpStatusCode.Unauthorized, "You are not authorized to edit this skill");

            var skill = await _portfolioDb.Skills.Where(S => S.PortfolioUserId == request.PortfolioId && S.Name == request.Name).FirstOrDefaultAsync(cancellationToken);

            if (skill is not null)
                return Failed<string>(System.Net.HttpStatusCode.Conflict, "Skill already exists");

            skill = await _portfolioDb.Skills.FindAsync(request.SkillId);
            if (skill is null)
                return Failed<string>(System.Net.HttpStatusCode.NotFound, "Skill not found");
            #endregion


            var tags = await _portfolioDb.Tags
                .Where(T => T.SkillId == request.SkillId).ToListAsync(cancellationToken);
            _portfolioDb.Tags.RemoveRange(tags);

            foreach (var tagName in request.Tags)
            {
                var tag = new Tag
                {
                    Name = tagName,
                    SkillId = skill.Id
                };
                _portfolioDb.Tags.Add(tag);
            }

            skill.Name = request.Name;
            _portfolioDb.Skills.Update(skill);
            var result = await _portfolioDb.SaveChangesAsync(cancellationToken);

            if (result > 0)
                return Success("Skill updated successfully");
            return Failed<string>(System.Net.HttpStatusCode.InternalServerError, "Failed to update skill");
        }
    }
}
