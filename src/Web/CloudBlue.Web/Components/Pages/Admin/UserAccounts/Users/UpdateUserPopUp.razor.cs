using CloudBlue.Domain.DomainModels;
using CloudBlue.Domain.DomainModels.Users;
using CloudBlue.Domain.DomainModels.Users.UsersManagement;
using CloudBlue.Domain.Enums;
using CloudBlue.Domain.GenericTypes;
using CloudBlue.Domain.Interfaces.Services;
using CloudBlue.Web.Services;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace CloudBlue.Web.Components.Pages.Admin.UserAccounts.Users;

public partial class UpdateUserPopUp : ComponentBase
{
    private readonly UpdateUserModel _model = new();

    private bool _isBusy;
    private bool _isDisabled;

    [Inject]
    public SpinnerService SpinnerService { set; get; }

    private int SelectedItemId { set; get; }
    [Parameter]
    public UserItemForList CurrentUser { set; get; }
    [Parameter]
    public ManageUserActions Action { set; get; }

    [Parameter]
    public IEnumerable<LookupItem<int>> LookupItems { set; get; } = [];
    [Inject]
    private NotificationService NotificationService { set; get; } = null!;

    [Inject]
    public IUsersService UsersService { set; get; }
    [Parameter]
    public string? Title { set; get; }

    private void Close()
    {
        DialogService.Close(true);
    }

    protected override async Task OnParametersSetAsync()
    {
        if (Action == ManageUserActions.UpdateDirectManager)
        {
            SelectedItemId = CurrentUser.DirectManagerId;
        }
        else if (Action == ManageUserActions.UpdatePosition)
        {
            SelectedItemId = CurrentUser.PositionId;
        }
        else if (Action == ManageUserActions.EditMobile)
        {
            _model.UserPhone = new ClientPhoneModel
            {
                AreaCode = CurrentUser.AreaCode,
                Phone = CurrentUser.Phone,
                DeviceInfo = CurrentUser.DeviceInfo,
                Id = CurrentUser.DeviceId
            };
        }
        else if (Action == ManageUserActions.EditEmail)
        {
            _model.Email = CurrentUser.Email;
        }
        else if (Action == ManageUserActions.EditFullName)
        {
            _model.FullName = CurrentUser.FullName;
        }
        else if (Action == ManageUserActions.EditUsername)
        {
            _model.UserName = CurrentUser.UserName;
        }

        if (_model.SelectedItemId == 0)
        {
            _model.SelectedItemId = SelectedItemId;
        }

        await base.OnParametersSetAsync();
    }

    private async Task ApplyActionAsync(UpdateUserModel arg)
    {
        _isDisabled = true;

        if (_isBusy)
        {
            return;
        }

        _isBusy = true;
        SpinnerService.Show();
        var actionResult = false;

        switch (Action)
        {
            case ManageUserActions.UpdatePosition:
                actionResult = await UsersService.UpdatePositionAsync(CurrentUser.Id, _model.SelectedItemId,
                    _model.PromotionDate!.Value);

                break;

            case ManageUserActions.SetResigned:
                actionResult = await UsersService.SetResignedAsync(CurrentUser.Id, _model.PromotionDate!.Value);

                break;

            case ManageUserActions.UpdateDirectManager:
                actionResult = await UsersService.UpdateDirectManagerAsync(CurrentUser.Id, _model.SelectedItemId);

                break;

            case ManageUserActions.ChangePassword:
                actionResult = await UsersService.ChangePasswordAsync(CurrentUser.Id, _model.Password);

                break;

            case ManageUserActions.EditEmail:
                actionResult = await UsersService.UpdateEmailAsync(CurrentUser.Id, _model.Email);

                break;

            case ManageUserActions.EditFullName:
                actionResult = await UsersService.UpdateFullNameAsync(CurrentUser.Id, _model.FullName);

                break;

            case ManageUserActions.EditMobile:
                actionResult = await UsersService.UpdateMobilAsync(CurrentUser.Id, _model.UserPhone);

                break;

            case ManageUserActions.EditUsername:
                actionResult = await UsersService.UpdateUsernameAsync(CurrentUser.Id, _model.UserName);

                break;
        }

        if (actionResult)
        {
            NotificationService.Notify(new NotificationMessage
            {
                Severity = NotificationSeverity.Success,
                Summary = "",
                Detail = "User has been updated successfully",
                Duration = 3000
            });
        }
        else
        {
            NotificationService.Notify(new NotificationMessage
            {
                Severity = NotificationSeverity.Error,
                Summary = "",
                Detail = "An error has been occurred while updating user",
                Duration = 3000
            });
        }

        _isBusy = false;
        SpinnerService.Hide();
    }
}