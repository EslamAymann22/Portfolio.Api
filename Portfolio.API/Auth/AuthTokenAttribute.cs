using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Portfolio.API.Auth
{
    public class AuthTokenAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var request = context.HttpContext.Request;
            var config = context.HttpContext.RequestServices.GetService(typeof(IConfiguration)) as IConfiguration;

            var Lotfy = config["Auth:Lotfy"];
            var Eslam = config["Auth:Eslam"];
            var providedToken = request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");

            if (providedToken != Eslam && providedToken != Lotfy)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (providedToken == Eslam)
                context.HttpContext.Items["User"] = "Eslam";
            else if (providedToken == Lotfy)
                context.HttpContext.Items["User"] = "Lotfy";

            await next();
        }
    }
}