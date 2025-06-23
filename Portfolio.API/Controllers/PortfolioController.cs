using MediatR;
using Microsoft.AspNetCore.Mvc;
using Portfolio.API.Auth;
using Portfolio.Core.Features.Portfolio.Commands;
using Portfolio.Core.Features.Portfolio.Queries;
using Portfolio.Core.ResponseBase.GeneralResponse;

namespace Portfolio.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PortfolioController(IMediator mediator)
        {
            this._mediator = mediator;
        }


        [HttpPost("AddPortfolio")]
        public async Task<ActionResult<BaseResponse<string>>> AddPortfolio([FromBody] AddPortfolioModel model)
        {
            return Ok(await _mediator.Send(model));
        }

        [HttpPut("ChangeName")]
        [AuthToken]
        public async Task<ActionResult<BaseResponse<string>>> ChangeName([FromBody] ChangeNameModel model)
        {
            var tokenName = HttpContext.Items["User"]?.ToString();
            model.TokenName = tokenName ?? string.Empty;
            return Ok(await _mediator.Send(model));
        }

        [HttpPut("EditAbout")]
        [AuthToken]
        public async Task<ActionResult<BaseResponse<string>>> EditAbout([FromBody] EditAboutModel model)
        {
            var tokenName = HttpContext.Items["User"]?.ToString();
            model.TokenName = tokenName ?? string.Empty;
            return Ok(await _mediator.Send(model));
        }

        [HttpPut("EditProfileImage")]
        [AuthToken]
        public async Task<ActionResult<BaseResponse<string>>> EditProfileImage([FromForm] EditMainPhotoModel model)
        {
            var tokenName = HttpContext.Items["User"]?.ToString();
            model.TokenName = tokenName ?? string.Empty;
            return Ok(await _mediator.Send(model));
        }

        [HttpGet("GetPortfolioData")]

        public async Task<ActionResult<BaseResponse<GetPortfolioDataByIdDto>>> GetPortfolioData([FromQuery] GetNameByIdModel model)
        {
            return Ok(await _mediator.Send(model));
        }

    }
}
