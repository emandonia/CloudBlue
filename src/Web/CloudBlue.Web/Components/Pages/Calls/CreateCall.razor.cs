using Blazored.Toast.Services;
using CloudBlue.Domain.DomainModels;
using CloudBlue.Domain.DomainModels.CallLeads;
using CloudBlue.Domain.DomainModels.Users.UserSessions;
using CloudBlue.Domain.Enums;
using CloudBlue.Domain.GenericTypes;
using CloudBlue.Domain.Interfaces.Services;
using CloudBlue.Domain.Utilities;
using CloudBlue.Web.Components.Layout;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;

namespace CloudBlue.Web.Components.Pages.Calls;

public partial class CreateCall : ComponentBase
{
    #region Private methods and Page Event Handlers

    private void SalesTypeChanged()
    {
        _usages = [];
        _propertyTypes = [];
        _services = [];

        _callCreateModel.LeadTicketModel.UsageId = _callCreateModel.LeadTicketModel.ServiceId =
            _callCreateModel.LeadTicketModel.PropertyTypeId = 0;
        if (_callCreateModel.LeadTicketModel.SalesTypeId == 0)
        {
            return;
        }

        var salesType = (SalesTypes)_callCreateModel.LeadTicketModel.SalesTypeId;

        if (salesType == SalesTypes.Prime)
        {
            _usages = _allUsages.Where(z => z.ExtraId == "Prime")
                .ToList();

            _services = _allServices.Where(z => z.ExtraId == "Prime")
                .ToList();
        }
        else
        {
            _usages = _allUsages.ToList();
            _services = _allServices.ToList();
        }
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        _showContent = false;
    }

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        await PopulateLookups();
        _showContent = true;
    }

    private async Task CreateCallAsync()
    {
        if (IsBusy)
        {
            return;
        }

        IsBusy = true;
        _callCreateModel.ClientInfo.ClientContactDevices = _clientPhoneItems;
        PopulateStringValues();
        var response = await CallService.CreateCallAsync(_callCreateModel);

        if (response)
        {
            ResetAll(true);
            MainLayout.ShowMessage(ToastLevel.Success, "Call Created Successfully");
        }
        else
        {
            MainLayout.ShowMessage(ToastLevel.Error, CallService.LastErrors);
        }

        IsBusy = false;
    }

    private void PopulateStringValues()
    {
        _callCreateModel.Duration = "00:00";

        _callCreateModel.DurationInSeconds = DateTime.UtcNow.Subtract(_startCall)
            .Seconds;

        var x = _callCreateModel.DurationInSeconds / 60;
        var y = _callCreateModel.DurationInSeconds % 60;
        var strX = x.ToString("00");
        var strY = y.ToString("00");
        _callCreateModel.Duration = string.Format(strX + ":" + strY);
        var company = _companies.FirstOrDefault(z => z.ItemId == _callCreateModel.CompanyId);

        if (company != null)
        {
            _callCreateModel.CompanyName = company.ItemName;
        }

        var branch = _branches.FirstOrDefault(z => z.ItemId == _callCreateModel.BranchId);

        if (branch != null)
        {
            _callCreateModel.BranchName = branch.ItemName;
        }

        if (_isBrokerage == false)
        {
            return;
        }

        var sb = new List<string>();
        var country = _countries.FirstOrDefault(z => z.ItemId == _callCreateModel.Location.CountryId);

        if (country != null)
        {
            sb.Add(country.ItemName);
        }

        var city = _cities.FirstOrDefault(z => z.ItemId == _callCreateModel.Location.CityId);

        if (city != null)
        {
            sb.Add(city.ItemName);
        }

        var district = _districts.FirstOrDefault(z => z.ItemId == _callCreateModel.Location.DistrictId);

        if (district != null)
        {
            sb.Add(district.ItemName);
        }

        var neighborhood = _neighborhoods.FirstOrDefault(z => z.ItemId == _callCreateModel.Location.NeighborhoodId);

        if (neighborhood != null)
        {
            sb.Add(neighborhood.ItemName);
            _callCreateModel.Location.ProjectName = neighborhood.ItemName;
        }

        _callCreateModel.LocationStr = string.Join(" - ", sb);
        var agent = _agents.FirstOrDefault(z => z.ItemId == _callCreateModel.LeadTicketModel.AgentId);

        if (agent != null)
        {
            _callCreateModel.AgentName = agent.ItemName;
        }

        _callCreateModel.CurrentUserName = MainLayout.CurrentUser.FullName;
    }

    private void Cancel(MouseEventArgs obj)
    {
        ResetAll(true);
    }

    #endregion Private methods and Page Event Handlers

    #region Lookups

    private IEnumerable<LookupItem<int>> _allCallTypes = [];
    private IEnumerable<LookupItem<int>> _callTypes = [];
    private IEnumerable<LookupItem<int>> _clientTypes = [];
    private IEnumerable<LookupItem<int>> _genders = [];
    private IEnumerable<LookupItem<int>> _branches = [];
    private IEnumerable<LookupItem<int>> _knowSubItems = [];
    private IEnumerable<LookupItem<int>> _clientTitles = [];
    private IEnumerable<LookupItem<int>> _countries = [];
    private IEnumerable<LookupItem<int>> _propertyTypes = [];
    private IEnumerable<LookupItem<int>> _cities = [];
    private IEnumerable<LookupItem<int>> _agents = [];
    private IEnumerable<LookupItem<int>> _districts = [];
    private IEnumerable<LookupItem<int>> _neighborhoods = [];
    private IEnumerable<LookupItem<int>> _leadSources = [];
    private IEnumerable<LookupItem<int>> _currencies = [];
    private IEnumerable<LookupItem<int>> _knowItems = [];
    private IEnumerable<LookupItem<int>> _deviceTypes = [];
    private IEnumerable<LookupItem<int>> _usages = [];
    private IEnumerable<LookupItem<int>> _allUsages = [];
    private IEnumerable<LookupItem<int>> _companies = [];
    private IEnumerable<LookupItem<int>> _salesTypes = [];
    private IEnumerable<LookupItem<int>> _services = [];
    private IEnumerable<LookupItem<int>> _allServices = [];
    private IEnumerable<LookupItem<int>> _workFields = [];
    private IEnumerable<AgentItem> _allAgentItems = [];

    private async Task PopulateLookups()
    {
        if (IsBusy)
        {
            return;
        }

        IsBusy = true;
        _countries = (await LookUpsService.GetCountriesAsync()).ToList();
        _allCallTypes = await LookUpsService.GetCallTypesAsync();
        _clientTitles = await LookUpsService.GetClientTitlesAsync();
        _clientTypes = await LookUpsService.GetClientTypesAsync();
        _companies = await LookUpsService.GetCompaniesAsync();
        _currencies = await LookUpsService.GetCurrenciesAsync();
        _deviceTypes = await LookUpsService.GetDeviceTypesAsync();
        _knowItems = await LookUpsService.GetKnowItemsAsync();
        _workFields = await LookUpsService.GetWorkFieldsAsync();
        _allUsages = await LookUpsService.GetUsageAsync();
        _allServices = await LookUpsService.GetServicesAsync();
        _salesTypes = await LookUpsService.GetSalesTypesAsync();
        _leadSources = await LookUpsService.GetLeadSourcesAsync();
        _genders = await LookUpsService.GetGendersAsync();
        IsBusy = false;
        await CompanyChanged();
    }

    #endregion Lookups

    #region Dependencies

    [Inject]
    private IJSRuntime JsRuntime { get; set; } = null!;

    [Inject]
    private ICallsService CallService { get; set; } = null!;

    [Inject]
    private IClientsService ClientsService { get; set; } = null!;

    [Inject]
    private ILookUpsService LookUpsService { get; set; } = null!;

    [Inject]
    private DialogService DialogService { set; get; } = null!;

    [CascadingParameter]
    public MainLayout MainLayout { get; set; }

    [Inject]
    public LoggedInUserInfo LoggedInUserInfo { set; get; } = null!;

    #endregion Dependencies

    #region Local variables

    private bool _showContent;

    private ClientPhoneModel _clientPhoneItem = new();

    private CallCreateModel _callCreateModel = new();

    private bool _disabled = true;
    private bool _isBrokerage;
    private bool _readOnly = true;
    private bool _disableArabicName = true;
    private bool _disableBirthDate = true;
    private bool _disableGender = true;
    private bool _disableWorkField = true;
    private bool _disableOccupation = true;
    private bool _disableCompanyName = true;
    private bool _messageVisible;
    private string _title = string.Empty;
    private string _cssClass = string.Empty;
    private MarkupString? _message;
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

    private DateTime _startCall = DateTime.UtcNow;

    private List<ClientPhoneModel> _clientPhoneItems = [];

    #endregion Local variables

    #region SearchClient

    private async Task SearchClient()
    {
        ResetAll(false);
        var item = _countries.FirstOrDefault(z => z.ItemId == _clientPhoneItem.CountryId);

        if (item == null)
        {
            return;
        }

        if (IsBusy)
        {
            return;
        }

        IsBusy = true;
        MainLayout.ShowMessage(ToastLevel.Info, "Finding Client");
        _clientPhoneItem.CountryCode = item.ExtraId;

        _clientPhoneItem.DeviceInfo = UtilityFunctions.GetCompositePhoneForSearch(_clientPhoneItem.CountryCode,
            _clientPhoneItem.AreaCode, _clientPhoneItem.Phone);

        if (string.IsNullOrEmpty(_clientPhoneItem.DeviceInfo))
        {
            MainLayout.ShowMessage(ToastLevel.Error, "Invalid Phone");
            IsBusy = false;

            return;
        }

        var response =
            await ClientsService.SearchClientByPhoneAsync(_clientPhoneItem, BusinessActions.FindClient,
                CbPages.AddCall);

        _callCreateModel.ClientExists = response.Exist;

        if (response.Exist == false)
        {
            _callCreateModel = new CallCreateModel();
            _callCreateModel.ClientInfo.ClientTypeId = 1;
            _clientPhoneItem.DeviceType = DeviceTypes.Mobile;
            _clientPhoneItem.IsNew = true;
            _clientPhoneItem.DeviceTypeId = (int)DeviceTypes.Mobile;

            _clientPhoneItems.Add(new ClientPhoneModel
            {
                AreaCode = _clientPhoneItem.AreaCode,
                CountryId = _clientPhoneItem.CountryId,
                DeviceInfo = _clientPhoneItem.DeviceInfo,
                DeviceType = _clientPhoneItem.DeviceType,
                DeviceTypeId = _clientPhoneItem.DeviceTypeId,
                Email = _clientPhoneItem.Email,
                Phone = _clientPhoneItem.Phone,
                IsDefault = true,
                CanBeRemoved = false,
                CountryCode = _clientPhoneItem.CountryCode,
                IsNew = true,
                Id = 0
            });
        }
        else
        {
            _callCreateModel.ClientInfo.ClientId = _callCreateModel.ClientId = response.ClientItem.Id;
            PopulateClientData(response);
        }

        RefreshDevicesGrid();

        if (response.CanAddLeadTicket)
        {
            _callTypes = _allCallTypes.Where(z => z.ItemId < (int)CallTypes.ALreadyExist)
                .ToList();
        }
        else
        {
            _callTypes = _allCallTypes.Where(z => z.ItemId == (int)CallTypes.ALreadyExist)
                .ToList();
        }

        SetMessage(response);
        EnableControls(response.Exist);

        if (_companies.Count() == 1)
        {
            _callCreateModel.CompanyId = _companies.First()
                .ItemId;

            _branches = _companies.First()
                .SubLookUps;

            if (_branches.Count() == 1)
            {
                _callCreateModel.BranchId = _branches.First()
                    .ItemId;
            }
        }

        IsBusy = false;
        _startCall = DateTime.UtcNow;
    }

    private void ResetAll(bool resetAll)
    {
        _disabled = true;
        _readOnly = true;
        _disableArabicName = true;
        _disableBirthDate = true;
        _disableGender = true;
        SetMessage(null);
        _disableWorkField = true;
        _disableOccupation = true;
        _disableCompanyName = true;
        _callCreateModel = new CallCreateModel();
        _clientPhoneItems = new List<ClientPhoneModel>();

        if (resetAll)
        {
            _clientPhoneItem = new ClientPhoneModel();
        }
    }

    private void EnableControls(bool responseExist)
    {
        _disabled = false;
        _readOnly = responseExist;

        _disableArabicName =
            responseExist && string.IsNullOrEmpty(_callCreateModel.ClientInfo.ClientArabicName) == false;

        _disableBirthDate = responseExist && _callCreateModel.ClientInfo.BirthDate != null;
        _disableGender = responseExist && _callCreateModel.ClientInfo.GenderId > 0;
        _disableWorkField = responseExist && _callCreateModel.ClientInfo.WorkFieldId > 0;
        _disableOccupation = responseExist && string.IsNullOrEmpty(_callCreateModel.ClientInfo.Occupation) == false;

        _disableCompanyName =
            responseExist && string.IsNullOrEmpty(_callCreateModel.ClientInfo.ClientCompanyName) == false;
    }

    private void SetMessage(SearchClientItem? response)
    {
        if (response == null)
        {
            _messageVisible = false;
            _title = _cssClass = string.Empty;
            _message = new MarkupString("");

            return;
        }

        _messageVisible = true;

        if (response.Exist)
        {
            _cssClass = "rz-background-color-warning-lighter";
            _title = "Warning";
        }
        else
        {
            _cssClass = "rz-background-color-success-lighter";
            _title = "Success";
        }

        if (string.IsNullOrEmpty(response.Message) == false)
        {
            _message = (MarkupString)response.Message;
        }
    }

    private void PopulateClientData(SearchClientItem response)
    {
        _callCreateModel.ClientInfo.ClientName = response.ClientItem.ClientName;
        _callCreateModel.ClientInfo.ClientArabicName = response.ClientItem.ClientNameArabic;
        _callCreateModel.ClientInfo.BirthDate = response.ClientItem.ClientBirthDate;
        _callCreateModel.ClientInfo.ClientTitleId = response.ClientItem.ClientTitleId;
        _callCreateModel.ClientInfo.ClientTypeId = response.ClientItem.ClientTypeId;
        _callCreateModel.ClientInfo.ClientCompanyName = response.ClientItem.ClientCompanyName;
        _callCreateModel.ClientInfo.WorkFieldId = response.ClientItem.ClientWorkFieldId;
        _callCreateModel.ClientInfo.Occupation = response.ClientItem.ClientOccupation;
        _callCreateModel.ClientInfo.GenderId = response.ClientItem.GenderId;

        _clientPhoneItems.AddRange(response.ClientItem.ClientContactDevices.Select(z => new ClientPhoneModel
        {
            DeviceInfo = z.DeviceInfo,
            Phone = z.Phone,
            CanBeRemoved = false,
            IsNew = false,
            DeviceType = (DeviceTypes)z.DeviceTypeId,
            DeviceTypeId = z.DeviceTypeId,
            AreaCode = z.PhoneAreaCode,
            CountryCode = z.PhoneCountryCode,
            CountryId = z.CountryId,
            IsDefault = z.IsDefault
        })
            .ToList());
    }

    #endregion SearchClient

    #region Add Devices Methods

    private RadzenDataGrid<ClientPhoneModel> _devicesGrid = null!;

    private async Task ShowInlineDialog()
    {
        await DialogService.OpenAsync<CreateDevice>("Add new device",
            new Dictionary<string, object>
            {
                { "ClientPhoneItems", _clientPhoneItems },
                { "Countries", _countries },
                { "DeviceTypesList", _deviceTypes },
                { "RefreshCallback", EventCallback.Factory.Create(this, RefreshDevicesGrid) }
            }, new DialogOptions { Width = "auto", Height = "300px" });
    }

    private void RefreshDevicesGrid()
    {
        _devicesGrid.Reload();
    }

    private void RemoveItem(string? deviceInfo)
    {
        var item = _clientPhoneItems.FirstOrDefault(z => z.DeviceInfo == deviceInfo);

        if (item != null && item.CanBeRemoved)
        {
            _clientPhoneItems.Remove(item);
        }

        RefreshDevicesGrid();
    }

    #endregion Add Devices Methods

    #region Events Handlers

    private async Task CompanyChanged()
    {

        if (_companies.Count() == 1)
        {
            _callCreateModel.CompanyId = _companies.First()
                .ItemId;
        }

        var company = _companies.FirstOrDefault(z => z.ItemId == _callCreateModel.CompanyId);
        _callCreateModel.BranchId = _callCreateModel.LeadTicketModel.AgentId = 0;
        if (company != null)
        {
            _branches = company.SubLookUps;
            await BranchChanged();
        }
    }

    private void KnowSourceChanged()
    {
        var knowItem = _knowItems.FirstOrDefault(z => z.ItemId == _callCreateModel.LeadSourceInfo.KnowSourceId);
        _callCreateModel.LeadSourceInfo.KnowSubSourceId = 0;
        if (knowItem != null)
        {
            _knowSubItems = knowItem.SubLookUps;
        }
    }

    private async Task CallTypeChanged()
    {
        _isBrokerage = _callCreateModel.CallTypeId == (int)CallTypes.Brokerage;

        if (_isBrokerage && _callCreateModel.BranchId > 0 && _callCreateModel.LeadTicketModel.AgentId == 0 &&
            !_agents.Any())
        {
            await PopulateAgents();
        }
    }

    private async Task PopulateAgents()
    {
        if (IsBusy)
        {
            return;
        }

        IsBusy = true;

        if (_allAgentItems.Any() == false)
        {
            _allAgentItems = await LookUpsService.GetAgentsAsync(true);
        }

        _agents = _allAgentItems.Where(z => _callCreateModel.BranchId == 0 || z.BranchId == _callCreateModel.BranchId)
            .Select(z => new LookupItem<int>(z.AgentName, z.AgentId, z.SalesPersonClass.ToString(), 0))
            .ToList();

        if (_agents.Count() == 1)
        {
            _callCreateModel.LeadTicketModel.AgentId = _agents.First()
                .ItemId;
        }
        IsBusy = false;
    }

    private void UsageChanged()
    {
        var item = _usages.FirstOrDefault(z => z.ItemId == _callCreateModel.LeadTicketModel.UsageId);
        _callCreateModel.LeadTicketModel.PropertyTypeId = 0;
        if (item != null)
        {
            _propertyTypes = item.SubLookUps;
        }
    }

    private void CountryChanged()
    {
        var item = _countries.FirstOrDefault(z => z.ItemId == _callCreateModel.Location.CountryId);
        _callCreateModel.Location.CityId = _callCreateModel.Location.DistrictId = _callCreateModel.Location.NeighborhoodId = 0;
        if (item != null)
        {
            _cities = item.SubLookUps;
        }
    }

    private void CityChanged()
    {
        var item = _cities.FirstOrDefault(z => z.ItemId == _callCreateModel.Location.CityId);
        _callCreateModel.Location.DistrictId = _callCreateModel.Location.NeighborhoodId = 0;
        if (item != null)
        {
            _districts = item.SubLookUps;
        }
    }

    private void DistrictChanged()
    {
        var item = _districts.FirstOrDefault(z => z.ItemId == _callCreateModel.Location.DistrictId);
        _callCreateModel.Location.NeighborhoodId = 0;
        if (item != null)
        {
            _neighborhoods = item.SubLookUps;
        }
    }

    private async Task BranchChanged()
    {
        if (_branches.Count() == 1)
        {
            _callCreateModel.BranchId = _branches.First()
                .ItemId;
        }
        if (_isBrokerage)
        {
            await PopulateAgents();
        }
    }

    private void ShowTooltip(ElementReference elementReference, string valExtraId)
    {
        TooltipService.Open(elementReference, valExtraId);
    }



    #endregion Events Handlers
}