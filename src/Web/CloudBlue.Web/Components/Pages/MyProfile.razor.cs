using Blazored.Toast.Services;
using CloudBlue.Domain.DomainModels.Users;
using CloudBlue.Domain.Interfaces.Services;
using CloudBlue.Web.Components.Layout;
using Microsoft.AspNetCore.Components;

namespace CloudBlue.Web.Components.Pages;
public partial class MyProfile : ComponentBase
{

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await MainLayout.SetCurrentPageTitle("My Profile");
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    [Inject]
    private IUsersService ProfileService { get; set; }

    [CascadingParameter]
    public MainLayout MainLayout { set; get; }
    private ChangePasswordModel _model = new();
    private bool _isBusy;

    private bool IsBusy
    {
        set
        {
            _isBusy = value;
            MainLayout.ShowhideSpinner(_isBusy);
        }
        get => _isBusy;
    }

    private async Task SavePassword()
    {
        if (IsBusy)
        {
            return;

        }

        if (_model.Password != _model.ConfirmPassword)
        {
            MainLayout.ShowMessage(ToastLevel.Error, "Password and confirm password should be the same");
            return;
        }

        IsBusy = true;
        var result = await ProfileService.ChangePasswordAsync(_model);

        if (result)
        {
            MainLayout.ShowMessage(ToastLevel.Success, "Password has been changed");

        }
        else
        {
            MainLayout.ShowMessage(ToastLevel.Error, ProfileService.LastErrors);

        }

        IsBusy = false;




    }
}
