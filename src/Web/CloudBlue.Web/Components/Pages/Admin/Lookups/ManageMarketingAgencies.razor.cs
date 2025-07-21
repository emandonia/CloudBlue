using Blazored.Toast.Services;
using CloudBlue.Domain.DomainModels;
using CloudBlue.Domain.DomainModels.Users.UsersManagement;
using CloudBlue.Domain.GenericTypes;
using CloudBlue.Domain.Interfaces.Services;
using CloudBlue.Web.Components.Layout;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using System.Linq.Dynamic.Core;


namespace CloudBlue.Web.Components.Pages.Admin.Lookups;

public partial class ManageMarketingAgencies : ComponentBase
{

    private LoadDataArgs _args;

    private RadzenDataGrid<UserItemForList> _grid0;

    private bool _isBusy;
    private int _totalNumber;

    [Inject]
    private ILookUpsService LookUpsService { get; set; } = null!;
    [Inject]
    private ILookUpsManager LookUpsManager { get; set; } = null!;

    [CascadingParameter]
    public MainLayout Layout { get; set; } = null!;


    private bool IsBusy
    {
        set
        {
            _isBusy = value;
            Layout.ShowhideSpinner(_isBusy);
        }
        get => _isBusy;
    }

    private IEnumerable<LookupItem<int>> _items = [];
    private IEnumerable<LookupItem<int>> _allItems = [];

    protected override async Task OnInitializedAsync()
    {
        await PopulateLookups();
        LoadData(new LoadDataArgs());

    }

    private CreateLookupModel _model = new();
    private async Task PopulateLookups()
    {
        if (IsBusy)
        {
            return;
        }

        IsBusy = true;
        _allItems = await LookUpsService.GetMarketingAgenciesAsync();

        IsBusy = false;
    }



    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await Layout.SetCurrentPageTitle("Manage Marketing Agencies");
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    private void LoadData(LoadDataArgs args)
    {
        _args = args;

        if (IsBusy)
        {
            return;
        }

        IsBusy = true;
        var pageSize = 10;
        if (args.Top is > 0)
        {
            pageSize = args.Top.Value;
        }
        var orderBy = "ItemName Asc";

        if (string.IsNullOrEmpty(args.OrderBy) == false)
        {
            orderBy = args.OrderBy;
        }

        var pageIndex = ((args.Skip ?? 0) / pageSize);



        _totalNumber = _allItems.Count();
        _items = _allItems.AsQueryable().OrderBy(orderBy).Skip(pageSize * pageIndex).Take(pageSize).ToArray();
        IsBusy = false;
    }

    private async Task FormSubmit(CreateLookupModel arg)
    {
        if (IsBusy)
        {
            return;
        }

        IsBusy = true;
        var result = await LookUpsManager.CreateMarketingAgencyAsync(arg);

        if (result)
        {
            Layout.ShowMessage(ToastLevel.Success, "Item has been added successfully");
            IsBusy = false;
            LookUpsService.RefreshMode = true;
            await PopulateLookups();
            LookUpsService.RefreshMode = false;
            LoadData(_args);
        }
        else
        {
            Layout.ShowMessage(ToastLevel.Error, LookUpsManager.LastErrors);

        }
        IsBusy = false;

    }
}