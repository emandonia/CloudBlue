using CloudBlue.Domain.DomainModels;
using CloudBlue.Domain.DomainModels.CallLeads;
using CloudBlue.Domain.Enums;
using CloudBlue.Domain.GenericTypes;
using CloudBlue.Domain.Interfaces.Services;
using CloudBlue.Web.Services;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;

namespace CloudBlue.Web.Components.Pages.LeadTickets;

public partial class ActionsPopUp : ComponentBase
{
    private readonly LeadTicketActionModel _model = new();
    private RadzenDataGrid<EntityActionResult>? _grid;
    private bool _isResponse;

    private bool _redirect;

    private List<EntityActionResult> _results = new();
    private bool _showComment;
    private bool _showQuestion;
    private bool _showReason;
    private string _url = "";
    [Parameter]
    public EventCallback RefreshCallback { get; set; }

    [Parameter]
    public string SubTitle { set; get; } = string.Empty;

    private IEnumerable<LookupItem<int>> ReasonItems { set; get; } = [];
    [Inject]
    private NavigationManager NavManager { set; get; } = null!;

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

    private void Close()
    {
        DialogService.Close(true);
    }

    protected override async Task OnParametersSetAsync()
    {
        SpinnerService.Show();
        ReasonItems = await LookUpsService.GetVoidReasonsAsync();
        SetParams();
        await base.OnParametersSetAsync();
        SpinnerService.Hide();
    }

    private void SetParams()
    {
        _model.Action = Action;

        _model.ItemsIds = LeadItems.Select(z => z.Id)
            .ToList();

        switch (Action)
        {
            case SystemPrivileges.LeadTicketsReject:
                _showQuestion = _showReason = _showComment = true;
                _redirect = false;

                break;

            case SystemPrivileges.LeadTicketsSetVoid:
                _showQuestion = _showReason = _showComment = true;
                _redirect = false;

                break;

            case SystemPrivileges.LeadTicketsArchive:
                _showReason = _redirect = false;
                _showQuestion = true;

                break;

            case SystemPrivileges.LeadTicketsUnArchive:
                _showReason = _redirect = false;
                _showQuestion = true;

                break;

            case SystemPrivileges.LeadTicketsConvertToBuyerRequest:
                _showReason = _showQuestion = _showComment = false;
                _url = $"/buyer-requests/create/{_model.ItemsIds.First()}";
                _redirect = true;

                break;

            case SystemPrivileges.LeadTicketsConvertToSellerRequest:
                _showReason = _showQuestion = _showComment = false;
                _url = $"/seller-requests/create/{_model.ItemsIds.First()}";
                _redirect = true;

                break;

            case SystemPrivileges.LeadTicketsConvertToPrimeTcr:
                _showReason = _showQuestion = _showComment = false;
                _url = $"/prime-tcrs/create/{_model.ItemsIds.First()}";
                _redirect = true;

                break;

            case SystemPrivileges.LeadTicketsCreateDuplicate:
                _showReason = _redirect = _showComment = false;
                _showQuestion = true;

                break;

            default:
                return;
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
    }

    private async Task ApplyActionAsync(LeadTicketActionModel arg)
    {
        if (_redirect)
        {
            NavManager.NavigateTo(_url, false);

            return;
        }

        SpinnerService.Show();

        arg.Reason = ReasonItems.FirstOrDefault(z => z.ItemId == arg.ReasonId)
            ?.ItemName;

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