using CloudBlue.Domain.DomainModels;
using CloudBlue.Domain.DomainModels.CallLeads;
using CloudBlue.Domain.DomainModels.Filtration.Interfaces;
using CloudBlue.Domain.GenericTypes;
using CloudBlue.Domain.Interfaces.Services;
using CloudBlue.Domain.Utilities;
using CloudBlue.Web.Components.Layout;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace CloudBlue.Web.Components.Pages.Calls;

public partial class ManageCalls : ComponentBase
{
    private LoadDataArgs _args;
    private CallsFiltersModel _callsFilters = new();
    private ClientPhoneItem ClientContactDevice { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        await LoadData(new LoadDataArgs());
        await PopulateLookups();
    }

    private void KnowSourceChanged()
    {
        var knowItem = _knowItems.FirstOrDefault(z => z.ItemId == _callsFilters.KnowSourceId);
        _callsFilters.KnowSubSourceId = 0;

        if (knowItem != null)
        {
            _knowSubItems = knowItem.SubLookUps;
        }
    }

    private async Task PopulateLookups()
    {
        //if (IsBusy)
        //{
        //    return;
        //}

        //IsBusy = true;
        _countries = await LookUpsService.GetCountriesAsync();
        _callTypes = await LookUpsService.GetCallTypesAsync();
        _clientCategories = await LookUpsService.GetClientCategoriesAsync();
        _callStatuses = await LookUpsService.GetCallStatusesAsync();
        _callRecipients = await LookUpsService.GetCallRecipientsAsync();
        _companies = await LookUpsService.GetCompaniesAsync();
        _knowItems = await LookUpsService.GetKnowItemsAsync();
        _leadSources = await LookUpsService.GetLeadSourcesAsync();
        await CompanyChanged();

        //  IsBusy = false;
    }

    private async Task Search(MouseEventArgs obj)
    {
        if (_args == null)
        {
            _args = new LoadDataArgs { Skip = 0 };
        }

        await LoadData(_args);
    }

    private async Task LoadData(LoadDataArgs args)
    {
        _args = args;

        if (IsBusy)
        {
            return;
        }

        IsBusy = true;
        var country = _countries.FirstOrDefault(z => z.ItemId == ClientContactDevice.CountryId);

        if (country != null)
        {
            ClientContactDevice.PhoneCountryCode = country.ExtraId;
        }

        _callsFilters.ClientContactDevice = UtilityFunctions.GetCompositePhoneForSearch(
            ClientContactDevice.PhoneCountryCode, ClientContactDevice.PhoneAreaCode, ClientContactDevice.Phone);

        if (args.Top is > 0)
        {
            _callsFilters.PageSize = args.Top.Value;
        }

        _callsFilters.PageIndex = (args.Skip ?? 0) / _callsFilters.PageSize;

        if (string.IsNullOrEmpty(args.OrderBy) == false && args.OrderBy.Length > 0 &&
           args.OrderBy.IndexOf(" ", StringComparison.Ordinal) > 0 && args.OrderBy.Split(" ")
               .Length == 2)
        {
            var arr = args.OrderBy.Split(" ");
            _callsFilters.SortField = arr[0];
            _callsFilters.SortDirection = arr[1];
        }

        var retObj = await CallsService.GetCallsAsync(_callsFilters);
        _totalNumber = retObj.TotalCount;

        foreach (var item in retObj.Items)
        {
            item.CallTypeBadgeStyle = (BadgeStyle)Enum.Parse(typeof(BadgeStyle), item.CallTypeBackgroundColor!, true);

            item.CallStatusBadgeStyle =
                (BadgeStyle)Enum.Parse(typeof(BadgeStyle), item.CallStatusBackgroundColor!, true);
        }

        _calls = retObj.Items;
        IsBusy = false;
    }

    private void AddButtonClick()
    {
        Layout.NavigateTo("calls-add");
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await Layout.SetCurrentPageTitle("Manage Calls");
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task OpenCall(long id)
    {
        var callItem = _calls.FirstOrDefault(z => z.Id == id);

        if (callItem == null)
        {
            return;
        }

        await DialogService.OpenAsync<CallCardPage>($"Call {id} - {callItem.CallStatus}",
            new Dictionary<string, object> { { "CallItem", callItem } },
            new DialogOptions { Width = "950px", Height = "650px" });
    }

    private async Task ResetFilters()
    {
        _callsFilters = new CallsFiltersModel();
        ClientContactDevice = new ClientPhoneItem();
        _args.Skip = 0;
        _args.Filter = null;
        await LoadData(_args);
    }

    #region Local variables

    private int _totalNumber;
    private bool _isBusy;

    private bool _messageVisible;
    private string _title = string.Empty;
    private string _cssClass = string.Empty;
    private MarkupString? _message;
    private IEnumerable<LookupItem<int>> _callTypes = [];
    private readonly IEnumerable<LookupItem<int>> _intlNoOptions =
    [
        new("All", 0, "", 0), new("Yes", 1, "", 0), new("No", 2, "", 0)
    ];

    private IEnumerable<LookupItem<int>> _callStatuses = [];
    private IEnumerable<LookupItem<int>> _callRecipients = [];
    private IEnumerable<LookupItem<int>> _clientCategories = [];
    private IEnumerable<LookupItem<int>> _branches = [];
    private IEnumerable<LookupItem<int>> _knowSubItems = [];
    private IEnumerable<LookupItem<int>> _countries = [];

    private async Task CompanyChanged()
    {
        if (_companies.Count() == 1)
        {
            _callsFilters.CompanyId = _companies.First()
                .ItemId;
        }

        var company = _companies.FirstOrDefault(z => z.ItemId == _callsFilters.CompanyId);

        if (company != null)
        {
            _branches = company.SubLookUps;

            if (_branches.Count() == 1)
            {
                _callsFilters.BranchId = _branches.First()
                    .ItemId;
            }
        }
    }

    private bool IsBusy
    {
        set
        {
            _isBusy = value;
            Layout.ShowhideSpinner(_isBusy);
        }
        get => _isBusy;
    }

    private IEnumerable<LookupItem<int>> _leadSources = [];
    private IEnumerable<LookupItem<int>> _knowItems = [];
    private IEnumerable<LookupItem<int>> _companies = [];

    private CallItemForList[] _calls = [];
    private readonly List<CallItemForList> _selectItems = [];
    private RadzenDataGrid<CallItemForList>? _grid;

    #endregion Local variables

    #region Dependencies

    [Inject]
    private ILookUpsService LookUpsService { get; set; } = null!;

    [Inject]
    protected DialogService DialogService { get; set; }

    [Inject]
    public ICallsService CallsService { get; set; }

    [CascadingParameter]
    public MainLayout Layout { get; set; }

    #endregion Dependencies
}