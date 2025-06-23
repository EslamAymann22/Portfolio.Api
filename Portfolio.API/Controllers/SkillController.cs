using MediatR;
using Microsoft.AspNetCore.Mvc;
using Portfolio.API.Auth;
using Portfolio.Core.Features.Skiils.Commands;
using Portfolio.Core.Features.Skiils.Queries;

namespace Portfolio.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillController : ControllerBase
    {

        private readonly IMediator _mediator;

        public SkillController(IMediator mediator)
        {
            this._mediator = mediator;
        }


        [HttpGet("GetAllSkills")]
        public async Task<IActionResult> GetAllSkills([FromQuery] GetAllSkillsModel model)
        {
            return Ok(await _mediator.Send(model));
        }

        [AuthToken]
        [HttpPost("AddSkill")]
        public async Task<IActionResult> AddSkill([FromBody] AddSkillModel model)
        {
            var tokenName = HttpContext.Items["User"]?.ToString();
            model.TokenName = tokenName ?? string.Empty;
            return Ok(await _mediator.Send(model));
        }

        [AuthToken]
        [HttpDelete("DeleteSkill")]
        public async Task<IActionResult> DeleteSkill([FromQuery] DeleteSkillModel model)
        {
            var tokenName = HttpContext.Items["User"]?.ToString();
            model.TokenName = tokenName ?? string.Empty;
            return Ok(await _mediator.Send(model));
        }

        [AuthToken]
        [HttpPut("EditSkill")]
        public async Task<IActionResult> UpdateSkill([FromBody] EditSkillModel model)
        {
            var tokenName = HttpContext.Items["User"]?.ToString();
            model.TokenName = tokenName ?? string.Empty;
            return Ok(await _mediator.Send(model));
        }
    }
}
