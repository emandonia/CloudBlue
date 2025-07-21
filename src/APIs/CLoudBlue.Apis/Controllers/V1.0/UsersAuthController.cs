using CloudBlue.Domain.DomainModels.Users.UserAuth;
using CloudBlue.Domain.Interfaces.Services;
using CloudBlue.Domain.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace CLoudBlue.Apis.Controllers.V1._0;

[Route("api/v1.0/users-auth")]
[ApiController]
public class UsersAuthController(
    IUsersAuthService usersAuthService,
    IUsersSessionService usersSessionService,
    IActionContextAccessor actionContextAccessor) : CLoudBlueControllerBase(usersSessionService, actionContextAccessor)
{
    [HttpPost("sign-in")]
    public async Task<ActionResult<string?>> SignIn(LoginItem model)
    {
        if (ModelState.IsValid == false)
        {
            return BadRequest(LiteralsHelper.BadRequestMessage);
        }

        var result = await usersAuthService.SignIn(model);

        if (string.IsNullOrEmpty(result))
        {
            return Unauthorized();
        }

        return Ok(result);
    }

    //Task<bool> ForgetPassword(LoginItem loginItem);
}