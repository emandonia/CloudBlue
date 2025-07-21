using CloudBlue.Domain.DomainModels.Users.UserSessions;
using CloudBlue.Domain.Enums;
using CloudBlue.Domain.Interfaces.Services;
using CloudBlue.Domain.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Reflection;

namespace CLoudBlue.Apis.Controllers.V1._0;

[ApiController]
public class CLoudBlueControllerBase : ControllerBase
{
    private readonly IUsersSessionService _usersSessionService;
    public LoggedInUserInfo LoggedInUserInfo;

    public CLoudBlueControllerBase(IUsersSessionService usersSessionService,
        IActionContextAccessor actionContextAccessor)
    {
        _usersSessionService = usersSessionService;

        var attributes =
            ((ControllerActionDescriptor)((ActionContextAccessor)actionContextAccessor).ActionContext
                ?.ActionDescriptor!).MethodInfo.CustomAttributes;

        var customAttributeData = attributes as CustomAttributeData[] ?? attributes.ToArray();

        if (customAttributeData.FirstOrDefault(z => z.AttributeType.Name == "AllowAnonymousAttribute") != null)
        {
            LoggedInUserInfo = new LoggedInUserInfo();

            return;
        }

        var headers = actionContextAccessor.ActionContext!.HttpContext.Request.Headers;
        var controller = actionContextAccessor.ActionContext!.ActionDescriptor.RouteValues["controller"];
        var action = actionContextAccessor.ActionContext!.ActionDescriptor.RouteValues["action"];

        if (headers.Keys.Contains(LiteralsHelper.ApiKey) == false || GetUserSession(headers[LiteralsHelper.ApiKey]
               .ToString(), controller, action) == false || LoggedInUserInfo == null ||
           LoggedInUserInfo.CurrentAccessPrivilege == false)
        {
            throw new NotSupportedException("Not Allowed");
        }
    }

    [NonAction]
    public string ConstructErrorMessage(List<Errors> errorsList)
    {
        if (errorsList.Count == 0)
        {
            return string.Empty;
        }

        var errors = new List<string>();

        foreach (var error in errorsList)
        {
            errors.Add(error.ToString());
        }

        var message = UtilityFunctions.ConstructMessage(errors);

        message = message.Trim()
            .ToLower();

        message = $"{message.Substring(0, 1).ToUpper()}{message.Substring(1)}";

        return message;
    }

    [NonAction]
    private bool GetUserSession(string apiKey, string? controller, string? action)
    {
        var userObj = _usersSessionService.GetUserSessionAsync(apiKey, controller, action)
            .GetAwaiter()
            .GetResult();

        if (userObj == null)
        {
            return false;
        }

        LoggedInUserInfo = userObj;

        return true;
    }
}