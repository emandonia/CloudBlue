using CloudBlue.Domain.DomainModels;
using CloudBlue.Domain.Enums;
using CloudBlue.Domain.GenericTypes;
using CloudBlue.Domain.Interfaces.Services;
using CloudBlue.Domain.Utilities;
using CloudBlue.Web.Services;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace CloudBlue.Web.Components.Pages;

public partial class CreateDevice : ComponentBase
{
    #region Add Devices Methods

    private readonly ClientPhoneModel _newDevice = new();
    private bool _isEmail;
    private bool _showAll = true;
    private bool _isBusy;

    [Parameter]
    public List<ClientPhoneModel> ClientPhoneItems { set; get; } = new();

    [Inject]
    protected SpinnerService SpinnerService { get; set; } = null!;

    [Inject]
    private DialogService DialogService { get; set; } = null!;

    [Parameter]
    public IEnumerable<LookupItem<int>> Countries { set; get; } = [];

    [Parameter]
    public IEnumerable<LookupItem<int>> DeviceTypesList { set; get; } = [];

    [Parameter]
    public EventCallback RefreshCallback { get; set; }

    [Inject]
    private NotificationService NotificationService { set; get; } = null!;

    [Inject]
    private IClientsService ClientsService { get; set; } = null!;

    private void DeviceTypeChanged()
    {
        if (_newDevice.DeviceTypeId == 0)
        {
            _showAll = false;
            _isEmail = true;

            return;
        }

        _isEmail = _showAll = true;

        if (_newDevice.DeviceTypeId != (int)DeviceTypes.Email)
        {
            _isEmail = false;
        }
    }

    private bool IsBusy
    {
        set
        {
            _isBusy = value;

            if (_isBusy)
            {
                SpinnerService.Show();
            }
            else
            {
                SpinnerService.Hide();
            }
        }
        get => _isBusy;
    }

    private async Task AddDevice(ClientPhoneModel obj)
    {
        if (IsBusy)
        {
            return;
        }

        IsBusy = true;
        string? deviceInfo;

        if (obj.DeviceTypeId == (int)DeviceTypes.Email)
        {
            deviceInfo = obj.Email;
        }
        else
        {
            var item = Countries.FirstOrDefault(z => z.ItemId == obj.CountryId);

            if (item == null)
            {
                ShowNotification(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "",
                    Detail = "Invalid Phone",
                    Duration = 4000
                });

                IsBusy = false;

                return;
            }

            obj.CountryCode = item.ExtraId;
            deviceInfo = UtilityFunctions.GetCompositePhoneForSearch(obj.CountryCode, obj.AreaCode, obj.Phone);
        }

        if (string.IsNullOrEmpty(deviceInfo))
        {
            ShowNotification(new NotificationMessage
            {
                Severity = NotificationSeverity.Error,
                Summary = "",
                Detail = "Invalid Phone",
                Duration = 4000
            });

            IsBusy = false;

            return;
        }

        obj.DeviceInfo = deviceInfo;

        ShowNotification(new NotificationMessage
        {
            Severity = NotificationSeverity.Info,
            Summary = "",
            Detail = "Validating new device",
            Duration = 4000
        });

        obj.DeviceType = (DeviceTypes)obj.DeviceTypeId;

        if (ClientPhoneItems.Any(z => z.DeviceInfo == obj.DeviceInfo))
        {
            ShowNotification(new NotificationMessage
            {
                Severity = NotificationSeverity.Warning,
                Summary = "",
                Detail = "You have already added this device",
                Duration = 4000
            });

            IsBusy = false;

            return;
        }

        var response =
            await ClientsService.SearchPhoneAsync(obj.DeviceInfo, BusinessActions.AddNewDevice, CbPages.AddCall);

        if (response)
        {
            ShowNotification(new NotificationMessage
            {
                Severity = NotificationSeverity.Error,
                Summary = "",
                Detail = "This device exists in system",
                Duration = 4000
            });

            IsBusy = false;

            return;
        }

        ClientPhoneItems.Add(new ClientPhoneModel
        {
            AreaCode = obj.AreaCode,
            CountryId = obj.CountryId,
            DeviceInfo = obj.DeviceInfo,
            DeviceType = obj.DeviceType,
            DeviceTypeId = obj.DeviceTypeId,
            Email = obj.Email,
            Phone = obj.Phone,
            IsDefault = false,
            CanBeRemoved = true,
            CountryCode = obj.CountryCode,
            IsNew = true,
            Id = 0
        });

        if (RefreshCallback.HasDelegate)
        {
            await RefreshCallback.InvokeAsync();
        }

        IsBusy = false;

        ShowNotification(new NotificationMessage
        {
            Severity = NotificationSeverity.Success,
            Summary = "",
            Detail = "Device has been added successfully",
            Duration = 5000
        });

        _newDevice.AreaCode = _newDevice.Phone = _newDevice.DeviceInfo = _newDevice.Email = null;

        //    DialogService.Close(true);
    }

    #endregion Add Devices Methods

    private void ShowNotification(NotificationMessage message)
    {
        NotificationService.Notify(message);
    }

    private void Close()
    {
        DialogService.Close(true);
    }
}