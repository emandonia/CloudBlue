using Blazored.Toast.Services;
using CloudBlue.Domain.DomainModels.Filtration.Interfaces;
using CloudBlue.Domain.DomainModels.Users;
using CloudBlue.Domain.Enums;
using CloudBlue.Domain.GenericTypes;
using CloudBlue.Domain.Interfaces.Services;
using CloudBlue.Web.Components.Layout;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace CloudBlue.Web.Components.Pages.Admin.UserAccounts.Privileges;

public partial class ManageEntityPrivileges : ComponentBase
{
    private IEnumerable<LookupItem<int>> _allPrivileges = [];
    private IEnumerable<ActiveUserItem> _allUsers = [];

    private LoadDataArgs _args;
    private IEnumerable<LookupItem<int>> _departments = [];

    private string _entityLabel = string.Empty;
    private IEnumerable<LookupItem<int>> _entityTypes = [];

    private EntityPrivilegesFiltersModel _filters = new();

    private bool _isBusy;
    private EntityPrivilegeItemForList[] _items = [];

    private IEnumerable<LookupItem<int>> _privilegeCategories = [];
    private IQueryable<LookupItem<int>> _privilegeEntities;
    private IEnumerable<LookupItem<int>> _privileges = [];
    private IEnumerable<LookupItem<int>> _privilegeScopes = [];

    private bool _showEntityList;
    private int _totalNumber;
    private IEnumerable<LookupItem<int>> _userPositions = [];
    private IQueryable<LookupItem<int>> _users;
    protected RadzenDataGrid<EntityPrivilegeItemForList> grid0;

    [Inject]
    private ILookUpsService LookUpsService { get; set; } = null!;

    [Inject]
    private DialogService DialogService { set; get; } = null!;

    [CascadingParameter]
    public MainLayout Layout { get; set; }

    [Inject]
    public IPrivilegesService PrivilegesService { get; set; } = null!;

    private bool IsBusy
    {
        set
        {
        _isBusy = value;
        Layout.ShowhideSpinner(_isBusy);
        }
        get => _isBusy;
    }

    private async Task Search(MouseEventArgs obj)
    {
    if (_args == null)
    {
    _args = new LoadDataArgs { Skip = 0 };
    }

    await LoadData(_args);
    }

    private async Task ResetFilters()
    {
    _filters = new EntityPrivilegesFiltersModel();
    _args.Skip = 0;
    _args.Filter = null;
    await LoadData(_args);
    }

    protected override async Task OnInitializedAsync()
    {
    await LoadData(new LoadDataArgs());
    await PopulateLookups();
    }

    private async Task PopulateLookups()
    {
    if (IsBusy)
    {
    return;
    }

    IsBusy = true;
    _privilegeCategories = await LookUpsService.GetPrivilegeCategoriesAsync();
    _privilegeScopes = await LookUpsService.GetPrivilegeScopesAsync();
    _entityTypes = await LookUpsService.GetEntityTypesAsync();
    _userPositions = await LookUpsService.GetUserPositionsAsync();
    _departments = await LookUpsService.GetDepartmentsAsync();
    _allPrivileges = await LookUpsService.GetPrivilegesAsync();
    _allUsers = await LookUpsService.GetActiveUsersAsync();

    _users = _allUsers.Select(z => new LookupItem<int>($"{z.FullName} ({z.UserId})", z.UserId, "", 0))
        .AsQueryable();

    IsBusy = false;
    }

    protected async Task AddButtonClick(MouseEventArgs args)
    {
    await DialogService.OpenAsync<AddEntityPrivilege>("Add Entity Privilege");
    await LoadData(_args);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
    if (firstRender)
    {
    await Layout.SetCurrentPageTitle("Manage Privileges");
    }

    await base.OnAfterRenderAsync(firstRender);
    }

    private async Task LoadData(LoadDataArgs args)
    {
    _args = args;

    if (IsBusy)
    {
    return;
    }

    IsBusy = true;
    _filters.ExtraFilters = args.Filter;

    if (args.Top is > 0)
    {
    _filters.PageSize = args.Top.Value;
    }

    _filters.PageIndex = args.Skip ?? 0 / _filters.PageSize;

    if (string.IsNullOrEmpty(args.OrderBy) == false && args.OrderBy.Length > 0 &&
       args.OrderBy.IndexOf(" ", StringComparison.Ordinal) > 0 && args.OrderBy.Split(" ")
           .Length == 2)
    {
    var arr = args.OrderBy.Split(" ");
    _filters.SortField = arr[0];
    _filters.SortDirection = arr[1];
    }

    var retObj = await PrivilegesService.GetEntityPrivilegesAsync(_filters);
    _totalNumber = retObj.TotalCount;
    _items = retObj.Items;
    IsBusy = false;
    }

    private void EntityTypeChanged()
    {
    _showEntityList = false;
    _entityLabel = string.Empty;
    _filters.EntityId = 0;
    _privilegeEntities = null;

    if (_filters.EntityTypeId == 0)
    {
    return;
    }

    _showEntityList = true;

    if (_filters.EntityTypeId == (int)PrivilegeEntityTypes.User)
    {
    _entityLabel = "Users";
    _privilegeEntities = _users;
    }
    else if (_filters.EntityTypeId == (int)PrivilegeEntityTypes.Position)
    {
    _entityLabel = "User Positions";
    _privilegeEntities = _userPositions.AsQueryable();
    }
    else
    {
    _entityLabel = "Department";
    _privilegeEntities = _departments.AsQueryable();
    }
    }

    private void PrivilegeCategoryChanged()
    {
    _filters.PrivilegeId = 0;

    if (_filters.PrivilegeCategoryId == 0)
    {
    return;
    }

    _privileges = _allPrivileges.Where(z => z.ParentItemId == _filters.PrivilegeCategoryId)
        .ToArray();
    }

    private async Task RemoveItem(long id)
    {
    if (IsBusy)
    {
    return;
    }

    var result = await DialogService.Confirm("Are you sure you want to delete privilege?", "Delete Privilege",
        new ConfirmOptions { OkButtonText = "Yes", CancelButtonText = "No" });

    if (result != true)
    {
    return;
    }

    IsBusy = true;
    var res = await PrivilegesService.DeletePrivilegeAsync(id);
    IsBusy = false;

    if (res)
    {
    Layout.ShowMessage(ToastLevel.Success, "Privilege has been deleted successfully");
    await LoadData(_args);

    return;
    }

    Layout.ShowMessage(ToastLevel.Error, "An error has been occurred while deleting privilege");
    }
}