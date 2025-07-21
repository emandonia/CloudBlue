using Blazored.Toast.Services;
using CloudBlue.Domain.DomainModels;
using CloudBlue.Domain.DomainModels.CallLeads;
using CloudBlue.Domain.DomainModels.Filtration.Interfaces;
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

namespace CloudBlue.Web.Components.Pages.LeadTickets;

public partial class CreateLeadTicket : ComponentBase
{
    private int _topManagerId;
    bool _showAddFeedback = false;

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        await PopulateLookups();
    }

    private async Task PopulateLookups()
    {
        if (IsBusy)
        {
            return;
        }

        IsBusy = true;
        _countries = await LookUpsService.GetCountriesAsync();
        _agencies = await LookUpsService.GetMarketingAgenciesAsync();
        _clientTitles = await LookUpsService.GetClientTitlesAsync();
        _clientTypes = await LookUpsService.GetClientTypesAsync();
        _companies = await LookUpsService.GetCompaniesAsync();
        _salesClasses = await LookUpsService.GetSalesPersonClasses(true);
        _currencies = await LookUpsService.GetCurrenciesAsync();
        _deviceTypes = await LookUpsService.GetDeviceTypesAsync();
        _knowItems = await LookUpsService.GetKnowItemsAsync(true);
        _workFields = await LookUpsService.GetWorkFieldsAsync();
        _corporateCompanies = await LookUpsService.GetCorporateCompaniesAsync();
        _allUsages = await LookUpsService.GetUsageAsync();
        _allServices = await LookUpsService.GetServicesAsync();
        _salesTypes = await LookUpsService.GetSalesTypesAsync();
        _leadSources = await LookUpsService.GetLeadSourcesAsync(true);
        _genders = await LookUpsService.GetGendersAsync();
        _clientCategories = await LookUpsService.GetClientCategoriesAsync();
        IsBusy = false;
        await PopulateAgents();
    }
    private async Task ShowAddEventBox()
    {
        var action = SystemPrivileges.LeadTicketsAddFeedback;
        var privilege = MainLayout.CurrentUser.Privileges.FirstOrDefault(z => z.Privilege == action);

        if (privilege == null)
        {
            return;
        }

        var width = "750px";
        var height = "500px";
        var title = "Add Activity Event";


        title = "Add FeedBack";

        var retObj =
            await LeadTicketsService.GetLeadTicketsAsync(
                new LeadTicketsFiltersModel { EntityIds = createdLeadId.ToString() });

        if (retObj.Items.Length == 0)
        {
            return;
        }
        await DialogService.OpenAsync<ActivityEventsPopUp>(title,
            new Dictionary<string, object>
            {
                { "LeadItems", retObj.Items.ToList() },
                { "Action", action },
                { "Title", title },
                { "IsFeedback", true },
                { "IsReminder", false
                }
            }, new DialogOptions { Width = width, Height = height });
    }

    private long createdLeadId = 0;
    private async Task CreateLeadTicketAsync()
    {
        if (IsBusy)
        {
            return;
        }

        IsBusy = true;
        _leadCreateModel.ClientInfo.ClientContactDevices = _clientPhoneItems;
        var agent = _allAgentItems.FirstOrDefault(z => z.AgentId == _leadCreateModel.AgentId);

        if (agent == null)
        {
            return;
        }

        _leadCreateModel.BranchId = agent.BranchId;
        _leadCreateModel.CompanyId = agent.CompanyId;
        PopulateStringValues();
        var response = await LeadTicketsService.CreateLeadTicketAsync(_leadCreateModel);

        if (response)
        {
            createdLeadId = _leadCreateModel.LeadTicketId;
            _showAddFeedback = _leadCreateModel.AgentId > 0 && _leadCreateModel.AgentId == MainLayout.CurrentUser.UserId;
            ResetAll(true);
            MainLayout.ShowMessage(ToastLevel.Success, "Lead Ticket Created Successfully");
        }
        else
        {
            MainLayout.ShowMessage(ToastLevel.Error, LeadTicketsService.LastErrors);
        }

        IsBusy = false;
    }

    private void PopulateStringValues()
    {
        var company = _companies.FirstOrDefault(z => z.ItemId == _leadCreateModel.CompanyId);

        if (company == null)
        {
            return;
        }

        _leadCreateModel.CompanyName = company.ItemName;
        var agent = _allAgentItems.FirstOrDefault(z => z.AgentId == _leadCreateModel.AgentId);

        if (agent != null)
        {
            _leadCreateModel.AgentName = agent.AgentName;
            _leadCreateModel.BranchId = agent.BranchId;
        }

        var branch = company.SubLookUps.FirstOrDefault(z => z.ItemId == _leadCreateModel.BranchId);

        if (branch != null)
        {
            _leadCreateModel.BranchName = branch.ItemName;
        }

        var sb = new List<string>();
        var country = _countries.FirstOrDefault(z => z.ItemId == _leadCreateModel.Location.CountryId);

        if (country != null)
        {
            sb.Add(country.ItemName);
        }

        var city = _cities.FirstOrDefault(z => z.ItemId == _leadCreateModel.Location.CityId);

        if (city != null)
        {
            sb.Add(city.ItemName);
        }

        var district = _districts.FirstOrDefault(z => z.ItemId == _leadCreateModel.Location.DistrictId);

        if (district != null)
        {
            sb.Add(district.ItemName);
        }

        var neighborhood = _neighborhoods.FirstOrDefault(z => z.ItemId == _leadCreateModel.Location.NeighborhoodId);

        if (neighborhood != null)
        {
            sb.Add(neighborhood.ItemName);
            _leadCreateModel.Location.ProjectName = neighborhood.ItemName;
        }

        _leadCreateModel.LocationStr = string.Join(" - ", sb);
        _leadCreateModel.CurrentUserName = MainLayout.CurrentUser.FullName;
    }

    private void Cancel(MouseEventArgs obj)
    {
        ResetAll(true);
    }

    private void SalesTypeChanged()
    {
        _usages = [];
        _propertyTypes = [];
        _services = [];
        _leadCreateModel.ServiceId = _leadCreateModel.UsageId = _leadCreateModel.PropertyTypeId = 0;

        if (_leadCreateModel.SalesTypeId == 0)
        {
            return;
        }

        var salesType = (SalesTypes)_leadCreateModel.SalesTypeId;

        if (salesType == SalesTypes.Prime)
        {
            _usages = _allUsages.Where(z => z.ExtraId == "Prime")
                .ToArray();

            _services = _allServices.Where(z => z.ExtraId == "Prime")
                .ToArray();
        }
        else
        {
            _usages = _allUsages.ToArray();
            _services = _allServices.ToArray();
        }
    }

    private void IsCorporateChanged()
    {
        _leadCreateModel.CorporateCompanyId = 0;

        if (_leadCreateModel.IsCorporate == 1)
        {
            _corporateDisabled = false;

            return;
        }

        _corporateDisabled = true;
    }

    #region Dependencies

    [Inject]
    private ILeadTicketsService LeadTicketsService { get; set; } = null!;

    [Inject]
    private IClientsService ClientsService { get; set; } = null!;

    [Inject]
    private ILookUpsService LookUpsService { get; set; } = null!;

    [Inject]
    private DialogService DialogService { set; get; } = null!;

    [CascadingParameter]
    public MainLayout MainLayout { get; set; }

    [Inject]
    private IJSRuntime JSRuntime { set; get; }

    [Inject]
    public LoggedInUserInfo LoggedInUserInfo { set; get; }

    #endregion Dependencies

    #region Local variables

    private ClientPhoneModel _clientPhoneItem = new();

    private LeadTicketCreateModel _leadCreateModel = new();

    private bool _corporateDisabled = true;
    private bool _disabled = true;
    private bool _readOnly = true;
    private bool _disableArabicName = true;
    private bool _disableCategory = true;
    private bool _disableBirthDate = true;
    private bool _disableGender = true;
    private bool _disableWorkField = true;
    private bool _disableOccupation = true;
    private bool _disableCompanyName = true;
    private bool _messageVisible;
    private string _title = string.Empty;
    private string _cssClass = string.Empty;
    private MarkupString? _message;
    private IEnumerable<LookupItem<int>> _clientTypes = [];
    private IEnumerable<LookupItem<int>> _clientCategories = [];
    private IEnumerable<LookupItem<int>> _genders = [];
    private IEnumerable<LookupItem<int>> _topManagers = [];
    private IEnumerable<LookupItem<int>> _salesClasses = [];
    private IEnumerable<LookupItem<int>> _knowSubItems = [];
    private IEnumerable<LookupItem<int>> _clientTitles = [];
    private IEnumerable<LookupItem<int>> _countries = [];
    private IEnumerable<LookupItem<int>> _propertyTypes = [];
    private IEnumerable<LookupItem<int>> _cities = [];
    private IEnumerable<LookupItem<int>> _agents = [];
    private IEnumerable<LookupItem<int>> _agencies = [];
    private IEnumerable<LookupItem<int>> _districts = [];
    private IEnumerable<LookupItem<int>> _neighborhoods = [];

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

    private List<ClientPhoneModel> _clientPhoneItems = [];
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
    private IEnumerable<LookupItem<int>> _corporateCompanies = [];
    private readonly IEnumerable<LookupItem<int>> _yesNowList = [new("Yes", 1, ",0", 0), new("No", 2, "", 0)];

    private IEnumerable<AgentItem> _allAgentItems = [];

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
            _clientPhoneItem.AreaCode, _clientPhoneItem.Phone) ?? string.Empty;

        if (string.IsNullOrEmpty(_clientPhoneItem.DeviceInfo))
        {
            MainLayout.ShowMessage(ToastLevel.Error, "Invalid Phone");
            IsBusy = false;

            return;
        }

        var response =
            await ClientsService.SearchClientByPhoneAsync(_clientPhoneItem, BusinessActions.FindClient,
                CbPages.AddLeadTicket);

        _leadCreateModel.ClientExists = response.Exist;

        if (response.Exist == false)
        {
            _disabled = false;
            _readOnly = false;
            _leadCreateModel = new LeadTicketCreateModel();
            _leadCreateModel.ClientInfo.ClientTypeId = 1;
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
            _leadCreateModel.ClientInfo.ClientId = _leadCreateModel.ClientId = response.ClientItem.Id;
            PopulateClientData(response);
        }

        if (response.CanAddLeadTicket)
        {
            EnableControls(response.Exist);
        }

        if (_companies.Count() == 1)
        {
            _leadCreateModel.CompanyId = _companies.First()
                .ItemId;

            IsBusy = false;
            await PopulateAgents();
        }

        RefreshDevicesGrid();
        SetMessage(response);

        //display add permission request
        //display add AE Call
        IsBusy = false;
    }

    private void ResetAll(bool resetAll)
    {
        _disabled = true;
        _readOnly = true;
        _disableArabicName = true;
        _disableCategory = true;
        _disableBirthDate = true;
        _disableGender = true;
        SetMessage(null);
        _disableWorkField = true;
        _disableOccupation = true;
        _disableCompanyName = true;
        _leadCreateModel = new LeadTicketCreateModel();
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
            responseExist && string.IsNullOrEmpty(_leadCreateModel.ClientInfo.ClientArabicName) == false;

        _disableCategory = responseExist && _leadCreateModel.ClientInfo.ClientCategoryId > 0;
        _disableBirthDate = responseExist && _leadCreateModel.ClientInfo.BirthDate != null;
        _disableGender = responseExist && _leadCreateModel.ClientInfo.GenderId > 0;
        _disableWorkField = responseExist && _leadCreateModel.ClientInfo.WorkFieldId > 0;
        _disableOccupation = responseExist && string.IsNullOrEmpty(_leadCreateModel.ClientInfo.Occupation) == false;

        _disableCompanyName =
            responseExist && string.IsNullOrEmpty(_leadCreateModel.ClientInfo.ClientCompanyName) == false;
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
        _leadCreateModel.ClientInfo.ClientName = response.ClientItem.ClientName;
        _leadCreateModel.ClientInfo.ClientArabicName = response.ClientItem.ClientNameArabic;
        _leadCreateModel.ClientInfo.BirthDate = response.ClientItem.ClientBirthDate;
        _leadCreateModel.ClientInfo.ClientTitleId = response.ClientItem.ClientTitleId;
        _leadCreateModel.ClientInfo.ClientTypeId = response.ClientItem.ClientTypeId;
        _leadCreateModel.ClientInfo.ClientCompanyName = response.ClientItem.ClientCompanyName;
        _leadCreateModel.ClientInfo.WorkFieldId = response.ClientItem.ClientWorkFieldId;
        _leadCreateModel.ClientInfo.Occupation = response.ClientItem.ClientOccupation;
        _leadCreateModel.ClientInfo.GenderId = response.ClientItem.GenderId;

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
            .ToArray());
    }

    #endregion SearchClient

    #region Add Devices Methods

    private RadzenDataGrid<ClientPhoneModel> _devicesGrid;

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

    private void KnowSourceChanged()
    {
        var knowItem = _knowItems.FirstOrDefault(z => z.ItemId == _leadCreateModel.LeadSourceInfo.KnowSourceId);
        _leadCreateModel.LeadSourceInfo.KnowSubSourceId = 0;

        if (knowItem != null)
        {
            _knowSubItems = knowItem.SubLookUps;
        }
    }

    private int _salesPersonClassId;

    private bool _showAssigning = true;
    private bool _showCompanies = true;
    private bool _showTopManager = true;
    private int _companiesWidth = 3;
    private int _topManagersWidth = 3;
    private int _salesFilterWidth = 2;
    private int _agentsWidth = 4;

    private async Task PopulateAgents()
    {
        if (IsBusy)
        {
            return;
        }

        IsBusy = true;
        _showAssigning = true;
        _showCompanies = true;
        _showTopManager = true;
        _companiesWidth = 3;
        _topManagersWidth = 3;
        _salesFilterWidth = 2;
        _agentsWidth = 4;

        if (_companies.Count() == 1)
        {
            _leadCreateModel.CompanyId = _companies.First()
                .ItemId;

            _showCompanies = false;
            _topManagersWidth = 6;
        }

        if (_allAgentItems.Any() == false)
        {
            _allAgentItems = await LookUpsService.GetAgentsAsync(true);
        }

        if (_allAgentItems.Count() == 1)
        {
            _leadCreateModel.AgentId = _allAgentItems.First()
                .AgentId;

            _showAssigning = false;
            IsBusy = false;

            return;
        }

        _topManagers = _allAgentItems.Where(z => z.SalesPersonClass == SalesPersonClasses.TopManager)
            .Select(z => new LookupItem<int>(z.AgentName, z.AgentId, z.SalesPersonClass.ToString(), 0))
            .ToArray();

        if (_topManagers.Count() == 1)
        {
            if (_showCompanies)
            {
                _companiesWidth = 6;
            }

            //_topManagerId = _topManagers.First().ItemId;
        }

        _showTopManager = _topManagers.Count() > 1;
        SalesPersonClasses? salesClass = null;

        if (_salesPersonClassId > 0)
        {
            salesClass = (SalesPersonClasses)_salesPersonClassId;
        }

        _agents = _allAgentItems.Where(z =>
                (_topManagerId == 0 || z.TopMostManagerId == _topManagerId) &&
                (salesClass == null || z.SalesPersonClass == salesClass))
            .Select(z => new LookupItem<int>(z.AgentName, z.AgentId, z.SalesPersonClass.ToString(), 0))
            .ToArray();

        if (_agents.Count() == 1)
        {
            _leadCreateModel.AgentId = _agents.First()
                .ItemId;
        }
        else
        {
            _leadCreateModel.AgentId = 0;
        }

        if (!_showCompanies && !_showTopManager)
        {
            _agentsWidth = _salesFilterWidth = 6;
        }

        if (_leadCreateModel.CompanyId == 0)
        {
            _leadCreateModel.BranchId = _leadCreateModel.AgentId = _topManagerId = _salesPersonClassId = 0;
        }
        else if (_showTopManager && _topManagerId == 0)
        {
            _leadCreateModel.AgentId = _leadCreateModel.BranchId = _salesPersonClassId = 0;
        }
        else if (_salesPersonClassId == 0)
        {
            _leadCreateModel.AgentId = _leadCreateModel.BranchId = 0;
        }

        IsBusy = false;
    }

    private void UsageChanged()
    {
        var item = _usages.FirstOrDefault(z => z.ItemId == _leadCreateModel.UsageId);
        _leadCreateModel.PropertyTypeId = 0;

        if (item != null)
        {
            _propertyTypes = item.SubLookUps;
        }
    }

    private void CountryChanged()
    {
        var item = _countries.FirstOrDefault(z => z.ItemId == _leadCreateModel.Location.CountryId);

        _leadCreateModel.Location.CityId =
            _leadCreateModel.Location.DistrictId = _leadCreateModel.Location.NeighborhoodId = 0;

        if (item != null)
        {
            _cities = item.SubLookUps;
        }
    }

    private void CityChanged()
    {
        var item = _cities.FirstOrDefault(z => z.ItemId == _leadCreateModel.Location.CityId);
        _leadCreateModel.Location.DistrictId = _leadCreateModel.Location.NeighborhoodId = 0;

        if (item != null)
        {
            _districts = item.SubLookUps;
        }
    }

    private void DistrictChanged()
    {
        var item = _districts.FirstOrDefault(z => z.ItemId == _leadCreateModel.Location.DistrictId);
        _leadCreateModel.Location.NeighborhoodId = 0;

        if (item != null)
        {
            _neighborhoods = item.SubLookUps;
        }
    }

    private void ShowTooltip(ElementReference elementReference, string valExtraId)
    {
        TooltipService.Open(elementReference, valExtraId);
    }

    #endregion Events Handlers
}