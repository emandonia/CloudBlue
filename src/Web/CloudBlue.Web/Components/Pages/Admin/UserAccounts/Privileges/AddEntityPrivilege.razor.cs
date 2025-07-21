using CloudBlue.Domain.DomainModels;
using CloudBlue.Domain.DomainModels.Users;
using CloudBlue.Domain.Enums;
using CloudBlue.Domain.GenericTypes;
using CloudBlue.Domain.Interfaces.Services;
using CloudBlue.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace CloudBlue.Web.Components.Pages.Admin.UserAccounts.Privileges;

public partial class AddEntityPrivilege
{

    [Inject]
    protected DialogService DialogService { get; set; }

    [Inject]
    public ILookUpsService LookUpsService { get; set; }
    [Inject]
    public IPrivilegesService PrivilegesService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _allPrivileges = await LookUpsService.GetPrivilegesAsync();
        _departments = await LookUpsService.GetDepartmentsAsync();
        _userPositions = await LookUpsService.GetUserPositionsAsync();
        _privilegeCategories = await LookUpsService.GetPrivilegeCategoriesAsync();
        _entityTypes = await LookUpsService.GetEntityTypesAsync();
        _privilegeScopes = await LookUpsService.GetPrivilegeScopesAsync();
        _allUsers = await LookUpsService.GetActiveUsersAsync();

        _users = _allUsers.Select(z => new LookupItem<int>($"{z.FullName} ({z.UserId})", z.UserId, "", 0)).AsQueryable();



    }
    private EntityPrivilegeModel model = new();

    private IEnumerable<LookupItem<int>> _allPrivileges = [];




    private IEnumerable<LookupItem<int>> _privilegeCategories = [];
    private IEnumerable<LookupItem<int>> _privilegeScopes = [];
    private IEnumerable<LookupItem<int>> _privileges = [];
    private IEnumerable<LookupItem<int>> _entityTypes = [];
    private IEnumerable<LookupItem<int>> _userPositions = [];
    private IEnumerable<LookupItem<int>> _departments = [];
    private IQueryable<LookupItem<int>> _privilegeEntities;
    private IEnumerable<ActiveUserItem> _allUsers = [];
    private IQueryable<LookupItem<int>> _users;

    private async Task FormSubmit()
    {
        if (IsBusy)
        {
            return;
        }

        IsBusy = true;


        var entityName = string.Empty;

        if (model.PrivilegeEntityTypeId == (int)PrivilegeEntityTypes.Department)
        {
            var item = _departments.FirstOrDefault(z => z.ItemId == model.EntityId);

            if (item != null)
            {
                entityName = item.ItemName;
            }
        }
        else if (model.PrivilegeEntityTypeId == (int)PrivilegeEntityTypes.Position)
        {
            var item = _userPositions.FirstOrDefault(z => z.ItemId == model.EntityId);

            if (item != null)
            {
                entityName = item.ItemName;
            }
        }
        else if (model.PrivilegeEntityTypeId == (int)PrivilegeEntityTypes.User)
        {
            var item = _users.FirstOrDefault(z => z.ItemId == model.EntityId);

            if (item != null)
            {
                entityName = item.ItemName;
            }
        }
        model.EntityName = entityName;

        var resut = await PrivilegesService.CreateEntityPrivilegeAsync(model);
        if (resut == false)
        {
            NotificationService.Notify(new NotificationMessage
            {
                Severity = NotificationSeverity.Error,
                Summary = "",
                Detail = "This privilege exists in system",
                Duration = 3000
            });

            IsBusy = false;

            return;
        }
        IsBusy = false;
        model = new EntityPrivilegeModel();
        NotificationService.Notify(new NotificationMessage
        {
            Severity = NotificationSeverity.Success,
            Summary = "",
            Detail = "Privilege has been added successfully",
            Duration = 5000
        });


    }

    private bool _isBusy;
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

    [Inject]
    private NotificationService NotificationService { set; get; } = null!;

    [Inject]
    protected SpinnerService SpinnerService { get; set; } = null!;

    private async Task CancelButtonClick(MouseEventArgs args)
    {
        DialogService.Close(null);
    }

    private bool _showEntityList;

    private void PrivilegeCategoryChanged()
    {
        _privileges = [];
        model.PrivilegeId = 0;
        if (model.PrivilegeCategoryId == 0)
        {
            return;
        }

        _privileges = _allPrivileges.Where(z => z.ParentItemId == model.PrivilegeCategoryId)
            .ToList();
    }

    private string _entityLabel = string.Empty;

    private void EntityTypeChanged()
    {
        _showEntityList = false;
        _entityLabel = string.Empty;
        model.EntityId = 0;
        _privilegeEntities = null;

        if (model.PrivilegeEntityTypeId == 0)
            return;

        _showEntityList = true;

        if (model.PrivilegeEntityTypeId == (int)PrivilegeEntityTypes.User)
        {
            _entityLabel = "Users";
            _privilegeEntities = _users;
        }
        else if (model.PrivilegeEntityTypeId == (int)PrivilegeEntityTypes.Position)
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


}