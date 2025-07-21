using CloudBlue.Domain.DomainModels;
using CloudBlue.Domain.DomainModels.CallLeads;
using CloudBlue.Domain.Enums;
using CloudBlue.Domain.GenericTypes;
using CloudBlue.Domain.Interfaces.Services;
using CloudBlue.Web.Services;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;

namespace CloudBlue.Web.Components.Pages.LeadTickets;

public partial class ActivityEventsPopUp : ComponentBase
{
    private readonly LeadTicketActionModel _model = new();

    private List<LookupItem<int>> _feedbackItems = new();
    private RadzenDataGrid<EntityActionResult>? _grid;

    private bool _isResponse;

    private List<EntityActionResult> _results = new();
    [Parameter]
    public string SubTitle { set; get; } = string.Empty;

    private IEnumerable<ContactingTypeItem> ContactingTypeItems { set; get; } = [];
    [Parameter]
    public bool IsFeedback { set; get; }

    [Parameter]
    public bool IsReminder { set; get; }

    [Parameter]
    public List<LeadTicketItemForList> LeadItems { get; set; } = null!;

    [Inject]
    protected SpinnerService SpinnerService { get; set; } = null!;

    [Inject]
    private ILookUpsService LookUpsService { set; get; } = null!;

    [Parameter]
    public SystemPrivileges Action { set; get; }

    [Parameter]
    public string? Title { set; get; }

    [Inject]
    private ILeadTicketsActionsService ActionsService { set; get; } = null!;

    [Parameter]
    public EventCallback? RefreshCallback { get; set; }

    private void Close()
    {
        DialogService.Close(true);
    }

    protected override async Task OnParametersSetAsync()
    {
        SpinnerService.Show();
        ContactingTypeItems = await LookUpsService.GetContactingTypesAsync();
        SetParams();
        await base.OnParametersSetAsync();
        SpinnerService.Hide();
    }

    private void SetParams()
    {
        _model.Action = Action;

        _model.ItemsIds = LeadItems.Select(z => z.Id)
            .ToList();

        _results = LeadItems.Select(z => new EntityActionResult
        {
            ActionResult = z.AllowedActions.Contains(Action),
            ItemId = z.Id,
            Message = z.AllowedActions.Contains(Action)
                    ? "This Action looks applicable for this item"
                    : "This Action do NOT look applicable for this item"
        })
            .ToList();

        if (IsReminder)
        {
            SubTitle = "Reminder Type";

            _feedbackItems = ContactingTypeItems.Where(z => z.NeedsDate)
                .Select(t => new LookupItem<int>(t.ContactingTypeName, t.Id, "", 0))
                .ToList();
        }
        else
        {
            SubTitle = IsFeedback ? "Feedback Type" : "Activity Type";

            _feedbackItems = ContactingTypeItems.Where(z => z.IsFeedBack == IsFeedback)
                .Select(t => new LookupItem<int>(t.ContactingTypeName, t.Id, "", 0))
                .ToList();
        }
    }

    private async Task ApplyAction(LeadTicketActionModel arg)
    {
        SpinnerService.Show();
        _results = await ActionsService.ApplyActionAsync(arg);
        _isResponse = true;

        if (_grid != null)
        {
            await _grid.Reload();
        }

        if (RefreshCallback != null && RefreshCallback.Value.HasDelegate && _results.Any(z => z.ActionResult))
        {
            await RefreshCallback.Value.InvokeAsync();
        }

        SpinnerService.Hide();
    }

    private void FeedBackTypeChanged()
    {

    }
}