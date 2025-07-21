using CloudBlue.Domain.DomainModels;
using CloudBlue.Domain.DomainModels.PrimeTcrs;
using CloudBlue.Domain.Enums;
using CloudBlue.Domain.GenericTypes;
using CloudBlue.Domain.Interfaces.Services;
using CloudBlue.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;
using Radzen.Blazor;

namespace CloudBlue.Web.Components.Pages.Tcrs.PrimeTcrs;

public partial class PrimeTcrActionsPopUp : ComponentBase
{
    private readonly PrimeTcrEntityActionModel _model = new();
    private RadzenDataGrid<EntityActionResult>? _grid;
    private List<EntityActionResult> _results = new();
    private IEnumerable<LookupItem<int>> _firstDropDownItems = [];
    private IEnumerable<LookupItem<int>> _secondDropDownItems = [];


    private string? _firstDropDownTitle;
    private string? _secondDropDownTitle;
    private string? _commentTitle;
    private string? _dateTitle;
    private string? _numericTitle;
    private string? _stringTitle;

    private bool _showSingleDropDown;
    private bool _showQuestion;
    private bool _showStringValue;
    private bool _showNumeric;
    private bool _showUploadFile;
    private bool _showTwoDropDown;

    private bool _isResponse;

    private bool _showComment;
    private bool _showDatePicker;




    [Parameter]
    public string? QuestionMessage { set; get; }

    [Parameter]
    public List<PrimeTcrItemForList> PrimeTcrItems { get; set; } = null!;

    [Inject]
    protected SpinnerService SpinnerService { get; set; } = null!;

    [Parameter]
    public SystemPrivileges Action { set; get; }

    [Parameter]
    public string? Title { set; get; }

    [Inject]
    private IPrimeTcrsActionsService ActionsService { set; get; } = null!;
    [Inject]
    private ILookUpsService LookUpsService { set; get; } = null!;

    private void Close()
    {
        DialogService.Close(true);
    }

    private void FirstDropDownChange()
    {
        var knowItem = _firstDropDownItems.FirstOrDefault(z => z.ItemId == _model.KnowSourceId);
        _model.KnowSubSourceId = 0;

        if (knowItem != null)
        {
            _secondDropDownItems = knowItem.SubLookUps;
        }
    }

    private string _showFileError = "none";
    protected override async Task OnParametersSetAsync()
    {
        SpinnerService.Show();
        SetParams();
        await base.OnParametersSetAsync();
        SpinnerService.Hide();
    }

    private async Task SetParams()
    {
        _model.Action = Action;
        _showQuestion = string.IsNullOrEmpty(QuestionMessage) == false;
        _model.ItemsIds = PrimeTcrItems.Select(z => z.Id)
            .ToList();

        _results = PrimeTcrItems.Select(z => new EntityActionResult
        {
            ActionResult = z.AllowedActions.Contains(Action),
            ItemId = z.Id,
            Message = z.AllowedActions.Contains(Action)
                    ? "This Action looks applicable for this item"
                    : "This Action do NOT look applicable for this item"
        })
        .ToList();

        _showSingleDropDown = _showStringValue = _showNumeric = _showTwoDropDown = _isResponse = _showComment = _showDatePicker = false;
        _showFileError = "none";
        //19

        switch (Action)
        {
            case SystemPrivileges.PrimeTcrsCanReloadCommissions:
                break;
            case SystemPrivileges.PrimeTcrsCanSetInvoiced:
                break;

            case SystemPrivileges.PrimeTcrsCanSetReviewing:

                break;
            case SystemPrivileges.PrimeTcrsCanDelete:

                break;

            case SystemPrivileges.PrimeTcrsSetContracted:

                break;
            case SystemPrivileges.PrimeTcrsSetResolved:

                break;
            case SystemPrivileges.PrimeTcrsCanSetPostpone:
                _dateTitle = "Postpone Date";
                _showDatePicker = true;
                _showComment = true;
                _commentTitle = "Feedback";

                break;
            case SystemPrivileges.PrimeTcrsCanSetReopen:
                _showComment = true;
                _commentTitle = "Feedback";
                break;
            case SystemPrivileges.PrimeTcrsCanSetConflict:
                _showComment = true;
                _commentTitle = "Feedback";
                break;
            case SystemPrivileges.PrimeTcrsCanSetHalfCommissionCollected:
                _dateTitle = "Collection Date";
                _showDatePicker = true;
                break;

            case SystemPrivileges.PrimeTcrsCanUpdateCreationDate:
                _dateTitle = "creation Date";
                _showDatePicker = true;
                break;
            case SystemPrivileges.PrimeTcrsUpdateConfirmedReservedDate:
                _dateTitle = "Reservation Confirmation Date";
                _showDatePicker = true;
                break;

                break;
            case SystemPrivileges.PrimeTcrsChangeMarketingChannel:
                _firstDropDownTitle = "Marketing Channel";
                _secondDropDownTitle = "Marketing Sub Channel";
                _firstDropDownItems = await LookUpsService.GetKnowItemsAsync();
                _showTwoDropDown = true;
                break;


            case SystemPrivileges.PrimeTcrsAddExtraManager:
                _firstDropDownTitle = "Manager";
                await GetManagersList();
                _showSingleDropDown = true;
                break;
            case SystemPrivileges.PrimeTcrsUpdateUnitType:
                _firstDropDownTitle = "Unit Type";
                await GetUnitTypeList();
                _showSingleDropDown = true;
                break;
            case SystemPrivileges.PrimeTcrsUpdateUnitNumber:
                _stringTitle = "Unit Number";
                _showStringValue = true;
                break;

            case SystemPrivileges.PrimeTcrsUpdateSalesVolume:
                _numericTitle = "Sales Volume";
                _showNumeric = true;
                break;
            case SystemPrivileges.PrimeTcrsCanUpdateConfirmationDate:
                _dateTitle = "Contract Confirmation Date";
                _showDatePicker = true;
                break;


            case SystemPrivileges.PrimeTcrsSetConfirmedContracted:
                _dateTitle = "Contract Confirmation Date";
                _showDatePicker = true;
                break;

            case SystemPrivileges.PrimeTcrsSetHalfConfirmedContracted:
                _dateTitle = "Half Contract Confirmation Date";
                _showDatePicker = true;
                break;
            case SystemPrivileges.PrimeTcrsAddDocumentDate:
                _showDatePicker = true;
                _showSingleDropDown = true;
                _dateTitle = "Document Date";
                _firstDropDownTitle = "Document Type";
                _firstDropDownItems = await LookUpsService.GetDocumentTypesList();
                break;

            case SystemPrivileges.PrimeTcrsSetConfirmedReserved:
                _dateTitle = "Reservation Confirmation Date";
                _showDatePicker = true;
                break;

            case SystemPrivileges.PrimeTcrsUpdateHalfConfirmedContracted:
                _dateTitle = "Half Contract Confirmation Date";
                _showDatePicker = true;
                break;
            case SystemPrivileges.PrimeTcrsCanSetCanceledByDeveloper:
                _showComment = true;
                _commentTitle = "Feedback";
                break;
            case SystemPrivileges.PrimeTcrsAddEvent:
                _showDatePicker = _showSingleDropDown = _showComment;
                _dateTitle = "Event Date";
                _firstDropDownTitle = "Contacting Method";
                _commentTitle = "Comment";
                var items = await LookUpsService.GetContactingTypesAsync();
                _firstDropDownItems = items.Select(z => new LookupItem<int>(z.ContactingTypeName, z.Id, "", 0));

                break;
            case SystemPrivileges.PrimeTcrsAddAttachments:
                _showComment = _showUploadFile = true;
                break;
        }








    }

    private async Task GetUnitTypeList()
    {
        var usages = await LookUpsService.GetUsageAsync(PrimeTcrItems.First().UsageId);

        _firstDropDownItems = usages.First()
            .SubLookUps; //
    }

    private async Task GetManagersList()
    {
        var allAgents = await LookUpsService.GetAgentsAsync(true);
        //  _documentTypes = await LookUpsService.GetDocumentTypesList();


        _firstDropDownItems = allAgents.OrderBy(z => z.AgentName)
            .Where(z => z.SalesPersonClass != SalesPersonClasses.Agent && z.SalesPersonClass != SalesPersonClasses.Rec)
            .Select(z => new LookupItem<int>(z.AgentName, z.AgentId, z.SalesPersonClass.ToString(), 0))
            .ToArray();

    }
    private IBrowserFile? _file;

    private void HandleFileSelected(InputFileChangeEventArgs args)
    {
        _file = args.File;
    }

    private async Task ApplyActionAsync(PrimeTcrEntityActionModel arg)
    {
        _showFileError = "none";
        SpinnerService.Show();
        if (_model.Action == SystemPrivileges.PrimeTcrsAddAttachments)
        {
            if (_file == null)
            {
                _showFileError = "inline";
                SpinnerService.Hide();
                return;
            }

            _model.FileStream = _file.OpenReadStream(10 * 1024 * 1024); // Max 10MB
            _model.FileName = _file.Name;
        }


        if (_model.Action == SystemPrivileges.PrimeTcrsAddExtraManager)
        {
            var agent = _firstDropDownItems.FirstOrDefault(z => z.ItemId == _model.SelectedItemId);

            if (agent != null)
            {
                _model.StringValue = agent.ItemName;
            }
        }

        _results = await ActionsService.ApplyActionAsync(arg);
        _isResponse = true;

        if (_grid != null)
        {
            await _grid.Reload();
        }

        SpinnerService.Hide();
    }
}