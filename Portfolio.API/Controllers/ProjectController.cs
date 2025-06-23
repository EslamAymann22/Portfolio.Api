using MediatR;
using Microsoft.AspNetCore.Mvc;
using Portfolio.API.Auth;
using Portfolio.Core.Features.Projects.Commands;
using Portfolio.Core.Features.Projects.Commands.AddProject;
using Portfolio.Core.Features.Projects.Commands.UpdateProject;
using Portfolio.Core.Features.Projects.Queries;
using Portfolio.Core.ResponseBase.GeneralResponse;

namespace Portfolio.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {

        private readonly IMediator _mediator;

        public ProjectController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpGet("GetAllProjects")]
        public async Task<ActionResult<BaseResponse<GetAllProjectsDto>>> GetAllProjects([FromQuery] GetAllProjectsModel model)
        {
            return Ok(await _mediator.Send(model));
        }

        [AuthToken]
        [HttpPost("AddProject")]
        public async Task<ActionResult<BaseResponse<string>>> AddProject([FromBody] AddProjectModel model)
        {
            var tokenName = HttpContext.Items["User"]?.ToString();
            model.TokenName = tokenName ?? string.Empty;
            return Ok(await _mediator.Send(model));
        }

        [AuthToken]
        [HttpPut("UpdateProject")]
        public async Task<ActionResult<BaseResponse<string>>> UpdateProject([FromBody] UpdateProjectModel model)
        {
            var tokenName = HttpContext.Items["User"]?.ToString();
            model.TokenName = tokenName ?? string.Empty;
            return Ok(await _mediator.Send(model));
        }

        [AuthToken]
        [HttpDelete("DeleteProject")]
        public async Task<ActionResult<BaseResponse<string>>> DeleteProject([FromQuery] DeleteProjectModel model)
        {
            var tokenName = HttpContext.Items["User"]?.ToString();
            model.TokenName = tokenName ?? string.Empty;
            return Ok(await _mediator.Send(model));
        }
    }
}
