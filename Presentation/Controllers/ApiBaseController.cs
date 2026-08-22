using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Shared.CommonResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiBaseController: ControllerBase
    {

        protected int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            //throw new UnauthorizedAccessException("غير مصرح لك بالوصول");
            return Error.Unauthorized().Type == ErrorType.Unauthorized ? throw new UnauthorizedAccessException("غير مصرح لك بالوصول") : 0;
        }


        protected IActionResult HandleResult(Result result)
        {
            if (result.IsSuccess)
                return NoContent();
            else
                return HandleProblem(result.Errors);
        }
        protected ActionResult<TValue> HandleResult<TValue>(Result<TValue> result)
        {
            if (result.IsSuccess)
                return Ok(result.Value);
            else
                return HandleProblem(result.Errors);

        }

        private ActionResult HandleProblem(IReadOnlyList<Error> errors)
        {
            if (errors.Count == 0)
                return Problem(statusCode: StatusCodes.Status500InternalServerError, title: "An  Unexpected Error Occurred");
            if (errors.All(e => e.Type == ErrorType.Validation))
                return HandleValidationProblem(errors);

            return HandleSingleError(errors[0]);
        }

        private ActionResult HandleSingleError(Error error)
        {
            return Problem(
                title: error.Code,
                detail: error.Description,
                type: error.Type.ToString(),
                statusCode: HandleStatusCode(error.Type)
                );
        }

        private static int HandleStatusCode(ErrorType errorType) => errorType switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.InvalidCrendentials => StatusCodes.Status401Unauthorized,
            ErrorType.Failure => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError,
        };
        private ActionResult HandleValidationProblem(IReadOnlyList<Error> errors)
        {
            var dic = new ModelStateDictionary();
            foreach (var r in errors)
                dic.AddModelError(r.Code, r.Description);

            return ValidationProblem(dic);
        }
       
    }
}
