using Blazored.Toast.Services;
using CloudBlue.Domain.DomainModels;
using CloudBlue.Domain.DomainModels.CallLeads;
using CloudBlue.Domain.DomainModels.PrimeTcrs;
using CloudBlue.Domain.Enums;
using CloudBlue.Domain.GenericTypes;
using CloudBlue.Domain.Interfaces.Services;
using CloudBlue.Web.Components.Layout;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace CloudBlue.Web.Components.Pages.Tcrs.PrimeTcrs;

public partial class CreatePrimeTcr : ComponentBase
{
    private IEnumerable<LookupItem<int>> _agents = [];
    private IEnumerable<AgentItem> _allAgents = [];

    private IEnumerable<LookupItem<int>> _developers = [];
    private IEnumerable<LookupItem<int>> _documentTypes = [];
    private IEnumerable<LookupItem<int>> _extraManagers = [];
    private bool _isBusy;
    private LeadTicketInfoItemForTcr _leadItem = new();

    private CreatePrimeTcrModel _model = new();
    private IEnumerable<LookupItem<int>> _outsideBrokers = [];
    private IEnumerable<LookupItem<int>> _projects = [];
    private IEnumerable<LookupItem<int>> _propertyTypes = [];

    private bool _showForm;

    // private IEnumerable<LookupItem<int>> _usages = [];
    private IEnumerable<LookupItem<int>> _yesNoList = [];

    private bool searched;
    [Parameter]
    public long Id { set; get; }
    [Inject]
    private IPrimeTcrsService PrimeTcrsService { set; get; } = null!;
    [Inject]
    private ILookUpsService LookUpsService { get; set; } = null!;

    [Inject]
    private ILeadTicketsService LeadTicketsService { set; get; }
    [Inject]
    private DialogService DialogService { set; get; } = null!;

    [CascadingParameter]
    public MainLayout MainLayout { get; set; }

    private bool IsBusy
    {
        set
        {
            _isBusy = value;
            MainLayout.ShowhideSpinner(_isBusy);
        }
        get => _isBusy;
    }

    private async Task PopulateLookups()
    {
        _outsideBrokers = await LookUpsService.GetOutsideBrokersAsync();
        _developers = await LookUpsService.GetDevelopersAsync();
        var usages = await LookUpsService.GetUsageAsync(_leadItem.UsageId);

        _propertyTypes = usages.First()
            .SubLookUps; //

        //.Select(z => new LookupItem<int>(z.ItemName, z.ItemId, z.ExtraId, z.ParentItemId)).ToArray();
        _allAgents = await LookUpsService.GetAgentsAsync(true);
        _yesNoList = await LookUpsService.GetYesNoList("Select One");
        _documentTypes = await LookUpsService.GetDocumentTypesList();

        _agents = _allAgents.OrderBy(z => z.AgentName)
            .Select(z => new LookupItem<int>(z.AgentName, z.AgentId, z.SalesPersonClass.ToString(), 0))
            .ToArray();

        _extraManagers = _allAgents.OrderBy(z => z.AgentName)
            .Where(z => z.SalesPersonClass != SalesPersonClasses.Agent && z.SalesPersonClass != SalesPersonClasses.Rec)
            .Select(z => new LookupItem<int>(z.AgentName, z.AgentId, z.SalesPersonClass.ToString(), 0))
            .ToArray();
    }

    private void OnButtonClick(int buttonIdentifier)
    {
        _model.IsRecContracted = buttonIdentifier == 2;
    }

    protected override async Task OnParametersSetAsync()
    {
        if (Id > 0)
        {
            await SearchForLeadAsync(Id);
        }

        await base.OnParametersSetAsync();
    }

    private async Task SearchForLeadAsync(long id)
    {
        if (IsBusy || searched)
        {
            return;
        }

        IsBusy = true;
        searched = true;
        _model = new CreatePrimeTcrModel();
        _leadItem = new LeadTicketInfoItemForTcr();
        _showForm = false;
        var leadTicket = await LeadTicketsService.GetLeadTicketForPrimeTcrAsync(id);

        if (leadTicket == null)
        {
            MainLayout.ShowMessage(ToastLevel.Error, LeadTicketsService.LastErrors);
            IsBusy = false;

            return;
        }

        _showForm = true;
        _leadItem = leadTicket;
        await PopulateLookups();

        _model = new CreatePrimeTcrModel
        {
            PropertyTypeId = _leadItem.PropertyTypeId,
            LeadTicketId = _leadItem.LeadTicketId,
            FirstAgentSharePercentage = 100,
            FirstAgentId = _leadItem.AgentId
        };

        IsBusy = false;
    }

    private async Task CreatePrimeTcrAsync(CreatePrimeTcrModel arg)
    {
        if (IsBusy)
        {
            return;
        }

        IsBusy = true;
        var total = 0;

        if (_model.SecondAgentId == 0)
        {
            _model.SecondAgentSharePercentage = 0;
        }

        if (_model.ThirdAgentId == 0)
        {
            _model.ThirdAgentSharePercentage = 0;
        }

        if ((_model.FirstAgentId > 0 && _model.FirstAgentSharePercentage <= 0) ||
           (_model.SecondAgentId > 0 && _model.SecondAgentSharePercentage <= 0) ||
           (_model.ThirdAgentId > 0 && _model.ThirdAgentSharePercentage <= 0))
        {
            MainLayout.ShowMessage(ToastLevel.Error, "Agent percentage should be greater than 0");
            IsBusy = false;

            return;
        }


        total = _model.FirstAgentSharePercentage + _model.SecondAgentSharePercentage + _model.ThirdAgentSharePercentage;

        if (total != 100)
        {
            MainLayout.ShowMessage(ToastLevel.Error, "Total agents percentages should be 100");
            IsBusy = false;

            return;
        }





        var msg =
            "PLEASE MAKE SURE THE FOLLOWING FIELDS ARE CORRECT<br/><br/>- Agent(s)<br/>- Agents Percentages<br/>- Sales Volume<br/>- Unit Number <br/>- Project<br/>- Unit Type";

        var result = await DialogService.Confirm(msg, "Create TCR",
            new ConfirmOptions { OkButtonText = "Yes", CancelButtonText = "No" });

        if (result != true)
        {
            return;
        }

        msg = "By clicking YES you will not be allowed to add or modify agent(s)?";

        result = await DialogService.Confirm(msg, "Create TCR",
            new ConfirmOptions { OkButtonText = "Yes", CancelButtonText = "No" });

        if (result != true)
        {
            return;
        }

        result = await PrimeTcrsService.CreatePrimeTcrAsync(_model);

        if (result.Value)
        {
            _model = new CreatePrimeTcrModel();
            _leadItem = new LeadTicketInfoItemForTcr();
            MainLayout.ShowMessage(ToastLevel.Success, "Tcr has been created successfully");
        }
        else
        {
            MainLayout.ShowMessage(ToastLevel.Error, PrimeTcrsService.LastErrors);
        }

        IsBusy = false;
    }

    private void DeveloperChanged()
    {
        _model.ProjectId = 0;
        _model.Phase = string.Empty;

        if (_model.DeveloperId == 0)
        {
            return;
        }

        var developer = _developers.FirstOrDefault(z => z.ItemId == _model.DeveloperId);

        if (developer == null)
        {
            return;
        }

        _projects = developer.SubLookUps;
    }

    private async Task SearchLeadTicket()
    {
        searched = false;
        await SearchForLeadAsync(_model.LeadTicketId);
    }

    private void AgentsChanged()
    {
        if (_model.SecondAgentId == 0)
        {
            _model.SecondAgentSharePercentage = 0;
        }

        if (_model.ThirdAgentId == 0)
        {
            _model.ThirdAgentSharePercentage = 0;
        }
    }
}