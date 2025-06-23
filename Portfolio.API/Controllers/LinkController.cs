using MediatR;
using Microsoft.AspNetCore.Mvc;
using Portfolio.API.Auth;
using Portfolio.Core.Features.Links.Commands;
using Portfolio.Core.Features.Links.Queries;
using Portfolio.Core.ResponseBase.GeneralResponse;

namespace Portfolio.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinkController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LinkController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpGet("GetAllLinks")]
        public async Task<ActionResult<BaseResponse<GetAllLinksDto>>> GetAllLinks([FromQuery] GetAllLinksModel model)
        {
            return Ok(await _mediator.Send(model));
        }

        [AuthToken]
        [HttpPost("AddLink")]
        public async Task<ActionResult<BaseResponse<string>>> AddLink([FromBody] AddLinkModel model)
        {
            var tokenName = HttpContext.Items["User"]?.ToString();
            model.TokenName = tokenName ?? string.Empty;
            return Ok(await _mediator.Send(model));
        }

        [AuthToken]
        [HttpPut("EditLink")]
        public async Task<ActionResult<BaseResponse<string>>> EditLink([FromBody] EditLinkModel model)
        {
            var tokenName = HttpContext.Items["User"]?.ToString();
            model.TokenName = tokenName ?? string.Empty;
            return Ok(await _mediator.Send(model));
        }

        [AuthToken]
        [HttpDelete("DeleteLink")]
        public async Task<ActionResult<BaseResponse<string>>> DeleteLink([FromQuery] DeleteLinkModel model)
        {
            var tokenName = HttpContext.Items["User"]?.ToString();
            model.TokenName = tokenName ?? string.Empty;
            return Ok(await _mediator.Send(model));
        }

    }
}
