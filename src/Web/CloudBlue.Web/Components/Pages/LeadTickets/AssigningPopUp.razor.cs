using CloudBlue.Domain.DomainModels;
using CloudBlue.Domain.DomainModels.CallLeads;
using CloudBlue.Domain.DomainModels.Users.UserSessions;
using CloudBlue.Domain.Enums;
using CloudBlue.Domain.GenericTypes;
using CloudBlue.Domain.Interfaces.Services;
using CloudBlue.Web.Services;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace CloudBlue.Web.Components.Pages.LeadTickets;

public partial class AssigningPopUp : ComponentBase
{
    private readonly LeadTicketActionModel _model = new();
    private int _agentsWidth = 4;

    private IEnumerable<LookupItem<int>> _branches = [];
    private int _branchesWidth = 4;
    private int _companiesWidth = 4;

    private RadzenDataGrid<EntityActionResult>? _grid;
    private bool _isResponse;

    private List<EntityActionResult> _results = new();
    [Parameter]
    public EventCallback RefreshCallback { get; set; }

    [Parameter]
    public string SubTitle { set; get; } = string.Empty;

    [Inject]
    private ILeadTicketsActionsService ActionsService { set; get; } = null!;

    [Inject]
    private ILookUpsService LookUpsService { set; get; } = null!;

    [Inject]
    private LoggedInUserInfo CurrentUser { set; get; } = null!;

    [Inject]
    private TooltipService TooltipService { get; set; } = null!;

    [Parameter]
    public SystemPrivileges Action { set; get; }

    [Parameter]
    public string? Title { set; get; }

    [Parameter]
    public IEnumerable<LeadTicketItemForList> LeadItems { get; set; } = null!;

    private IEnumerable<LookupItem<int>> Agents { set; get; } = [];
    private IEnumerable<LookupItem<int>> Companies { get; set; } = [];

    private IEnumerable<LookupItem<int>> Branches { get; set; } = [];

    private IEnumerable<AgentItem> AllAgentItems { set; get; } = [];

    private bool ShowCompanies { get; set; }

    private bool ShowBranches { get; set; }

    private bool ShowAgents { get; set; }
    [Inject]
    protected SpinnerService SpinnerService { get; set; } = null!;

    private void PopulateAgents()
    {
        Agents = AllAgentItems.Where(z => _model.BranchId == 0 || z.BranchId == _model.BranchId)
            .Select(z => new LookupItem<int>(z.AgentName, z.AgentId, z.SalesPersonClass.ToString(), 0))
            .ToList();

        if (Agents.Count() == 1)
        {
            _model.AgentId = Agents.First().ItemId;
        }
    }

    private void Close()
    {
        DialogService.Close(true);
    }

    private void BranchChanged()
    {
        if (Branches.Count() == 1)
        {
            _model.BranchId = Branches.First().ItemId;
        }

        PopulateAgents();
    }

    private void CompanyChanged()
    {
        if (Companies.Count() == 1)
        {
            _model.CompanyId = Companies.First().ItemId;
        }
        var company = Companies.FirstOrDefault(z => z.ItemId == _model.CompanyId);

        if (company != null)
        {
            _branches = company.SubLookUps;
            BranchChanged();
        }
    }

    protected override async Task OnParametersSetAsync()
    {
        SpinnerService.Show();
        Companies = await LookUpsService.GetCompaniesAsync();
        AllAgentItems = await LookUpsService.GetAgentsAsync(true);
        SetParams();
        await base.OnParametersSetAsync();
        SpinnerService.Hide();
    }

    private void ShowTooltip(ElementReference elementReference, string valExtraId)
    {
        TooltipService.Open(elementReference, valExtraId);
    }

    private void SetParams()
    {
        _model.Action = Action;

        _model.ItemsIds = LeadItems.Select(z => z.Id)
            .ToList();

        var privilege = CurrentUser.Privileges.FirstOrDefault(z => z.Privilege == Action);

        if (privilege == null)
        {
            return;
        }

        ShowAgents = ShowBranches = ShowCompanies = false;

        if (Action == SystemPrivileges.LeadTicketsAssignToAgent || Action == SystemPrivileges.LeadTicketsReAssignToAgent)
        {
            ShowAgents = true;
            ShowCompanies = privilege.PrivilegeScope == PrivilegeScopes.Global;

            ShowBranches = privilege.PrivilegeScope == PrivilegeScopes.Company ||
                           privilege.PrivilegeScope == PrivilegeScopes.Global;
        }

        if (Action == SystemPrivileges.LeadTicketsAssignToBranch ||
           Action == SystemPrivileges.LeadTicketsReAssignToBranch)
        {
            ShowBranches = true;
            ShowCompanies = privilege.PrivilegeScope == PrivilegeScopes.Global;
            _agentsWidth = 0;
        }
        else if (Action == SystemPrivileges.LeadTicketsMoveToCompany)
        {
            ShowCompanies = true;
        }

        _companiesWidth = 6;
        _branchesWidth = 6;
        _agentsWidth = 6;

        if (ShowCompanies && ShowBranches && ShowAgents)
        {
            _companiesWidth = 4;
            _branchesWidth = 4;
            _agentsWidth = 4;
        }

        _results = LeadItems.Select(z => new EntityActionResult
        {
            ActionResult = z.AllowedActions.Contains(Action),
            ItemId = z.Id,
            Message = z.AllowedActions.Contains(Action)
                    ? "This Action looks applicable for this item"
                    : "This Action do NOT look applicable for this item"
        })
            .ToList();

        if (ShowCompanies == false)
        {
            _model.CompanyId = CurrentUser.CompanyId;
        }
        if (ShowBranches == false)
        {
            _model.BranchId = CurrentUser.BranchId;
        }
        CompanyChanged();
        BranchChanged();
    }

    private async Task ApplyAction(LeadTicketActionModel arg)
    {
        SpinnerService.Show();

        if (Action == SystemPrivileges.LeadTicketsAssignToAgent || Action == SystemPrivileges.LeadTicketsReAssignToAgent)
        {
            arg.AgentName = AllAgentItems.FirstOrDefault(z => z.AgentId == arg.AgentId)
                ?.AgentName;
        }
        else if (Action == SystemPrivileges.LeadTicketsAssignToBranch ||
                Action == SystemPrivileges.LeadTicketsReAssignToBranch)
        {
            arg.BranchName = Branches.FirstOrDefault(z => z.ItemId == arg.BranchId)
                ?.ItemName;
        }
        else
        {
            arg.CompanyName = Companies.FirstOrDefault(z => z.ItemId == arg.CompanyId)
                ?.ItemName;
        }

        _results = await ActionsService.ApplyActionAsync(arg);
        _isResponse = true;

        if (_grid != null)
        {
            await _grid.Reload();
        }

        if (RefreshCallback.HasDelegate && _results.Any(z => z.ActionResult))
        {
            await RefreshCallback.InvokeAsync();
        }

        SpinnerService.Hide();
    }
}