using CloudBlue.Domain.DomainModels;
using CloudBlue.Domain.DomainModels.Users.UsersManagement;
using CloudBlue.Domain.Enums;
using CloudBlue.Domain.GenericTypes;
using CloudBlue.Domain.Interfaces.Services;
using CloudBlue.Domain.Utilities;
using CloudBlue.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace CloudBlue.Web.Components.Pages.Admin.UserAccounts.Users;

public partial class CreateUser : ComponentBase
{
    private const int SalesDeptId = 1;
    private IEnumerable<AgentItem> _allManagers = [];
    private IEnumerable<LookupItem<int>> _allUserPositions = [];
    private IEnumerable<LookupItem<int>> _branches = [];
    private IEnumerable<LookupItem<int>> _companies = [];

    private IEnumerable<LookupItem<int>> _departments = [];

    private bool _isBusy;

    private bool _isSalesUser;
    private IEnumerable<LookupItem<int>> _managers = [];

    private readonly CreateUserModel _model = new();
    private bool _submitDisabled;
    private IEnumerable<LookupItem<int>> _userPositions = [];
    private LookupItem<int>[] _yesNoList = [];

    [Inject]
    public SpinnerService spinnerService { set; get; }
    [Inject]
    protected DialogService DialogService { get; set; }

    [Inject]
    protected NotificationService NotificationService { get; set; }
    [Inject]
    public IUsersService UsersService { get; set; }

    [Inject]
    public ILookUpsService LookUpsService { get; set; }

    private bool IsBusy
    {
        set
        {
            _isBusy = value;

            if (_isBusy)
            {
                spinnerService.Show();
            }
            else
            {
                spinnerService.Hide();
            }
        }
        get => _isBusy;
    }

    protected async Task FormSubmit()
    {
        if (IsBusy)
        {
            return;
        }

        IsBusy = _submitDisabled = true;
        var result = await UsersService.CreateUserAsync(_model);

        if (result)
        {
            NotificationService.Notify(new NotificationMessage
            {
                Severity = NotificationSeverity.Success,
                Summary = "",
                Detail = "User has been add successfully",
                Duration = 3000
            });
        }
        else
        {
            NotificationService.Notify(new NotificationMessage
            {
                Severity = NotificationSeverity.Error,
                Summary = "",
                Detail = UtilityFunctions.ConstructMessage(UsersService.LastErrors),
                Duration = 3000
            });

            _submitDisabled = false;
        }

        IsBusy = false;
    }

    protected async Task CancelButtonClick(MouseEventArgs args)
    {
        DialogService.Close();
    }

    protected override async Task OnInitializedAsync()
    {
        await PopulateLookups();
        await base.OnInitializedAsync();
    }

    private async Task PopulateLookups()
    {
        if (IsBusy)
        {
            return;
        }

        IsBusy = true;
        _yesNoList = await LookUpsService.GetYesNoList();
        _departments = await LookUpsService.GetDepartmentsAsync();
        _allUserPositions = await LookUpsService.GetUserPositionsAsync();
        _companies = await LookUpsService.GetCompaniesAsync();

        _allManagers =
            await LookUpsService.GetSalesPersonsAsync([SalesPersonClasses.TopManager, SalesPersonClasses.Manager], true);

        _userPositions = _allUserPositions;
        IsBusy = false;
    }

    private void DepartmentChanged()
    {
        _model.PositionId = 0;
        _userPositions = [];

        _userPositions = _allUserPositions.Where(z => z.ParentItemId == _model.DepartmentId)
            .ToArray();

        _isSalesUser = _model.DepartmentId == SalesDeptId;
        PopulateTopManagers();
    }

    private void PopulateTopManagers()
    {
        _managers = [];
        _model.DirectManagerId = 0;

        if (_model.DepartmentId != SalesDeptId)
        {
            return;
        }

        _managers = _allManagers.Where(z =>
                (_model.CompanyId == 0 || z.CompanyId == _model.CompanyId) &&
                (_model.BranchId == 0 || z.BranchId == _model.BranchId))
            .Select(z => new LookupItem<int>(z.AgentName, z.AgentId, "", 0))
            .ToArray();
    }

    private void CompanyChanged()
    {
        _branches = [];
        _model.BranchId = 0;
        var companyItem = _companies.FirstOrDefault(z => z.ItemId == _model.CompanyId);

        if (companyItem == null)
        {
            return;
        }

        _branches = companyItem.SubLookUps;
        PopulateTopManagers();
    }
}