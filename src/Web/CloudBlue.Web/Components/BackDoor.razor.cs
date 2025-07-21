using Blazored.Toast.Services;
using CloudBlue.Domain.DomainModels.Users.UserAuth;
using CloudBlue.Domain.DomainModels.Users.UserSessions;
using CloudBlue.Domain.Interfaces.Services;
using CloudBlue.Web.Components.Layout;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace CloudBlue.Web.Components;

public partial class BackDoor : ComponentBase
{
    private readonly LoginItem _loginModel = new();
    private bool _busy;
    [Inject]
    private IUsersAuthService UsersAuthService { set; get; } = null!;

    [CascadingParameter]
    public BasicLayout Layout { get; set; } = null!;

    [Inject]
    public LoggedInUserInfo CurrentUser { set; get; } = null!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    private async Task LoginInAsync(LoginArgs args)
    {
        if (_busy)
        {
            return;
        }



        _busy = true;
        Layout.ShowhideSpinner(true);
        var loginItem = new LoginItem { Username = args.Username, Password = args.Password };
        var apiKey = await UsersAuthService.SignIn(loginItem, true);

        if (string.IsNullOrEmpty(apiKey))
        {
            _busy = false;
            Layout.ShowhideSpinner(false);
            Layout.ShowMessage(ToastLevel.Error, UsersAuthService.LastErrors);

            return;
        }

        await Layout.SetLoggedInUserSessionAsync(apiKey);
        Layout.ShowhideSpinner(false);
        Layout.NavigateTo("/");
    }

    private Task ResetPasswordAsync(string args)
    {
        return Task.CompletedTask;
    }
}

