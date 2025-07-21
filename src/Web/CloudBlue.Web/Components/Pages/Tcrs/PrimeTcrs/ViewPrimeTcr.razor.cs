using Blazored.Toast.Services;
using CloudBlue.Domain.DomainModels;
using CloudBlue.Domain.DomainModels.PrimeTcrs;
using CloudBlue.Domain.Enums;
using CloudBlue.Domain.GenericTypes;
using CloudBlue.Domain.Interfaces.Services;
using CloudBlue.Web.Components.Layout;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;

namespace CloudBlue.Web.Components.Pages.Tcrs.PrimeTcrs;

public partial class ViewPrimeTcr : ComponentBase
{

    #region Variables

    private bool _CollapseSearch;
    private readonly Dictionary<string, object> _onImageError = new();
    private int _count;
    private bool _isBusy;
    private PrimeTcrFullItem _model = new();
    private bool _showForm;
    private List<SystemEventItem> _systemEventItems = new();
    private bool searched;
    private bool IsBusy
    {
        set
        {
            _isBusy = value;
            MainLayout.ShowhideSpinner(_isBusy);
        }
        get => _isBusy;
    }




    //private IEnumerable<LookupItem<int>> _agents = [];
    //private IEnumerable<AgentItem> _allAgents = [];
    //private IEnumerable<LookupItem<int>> _developers = [];
    //private IEnumerable<LookupItem<int>> _documentTypes = [];
    //private IEnumerable<LookupItem<int>> _extraManagers = [];

    //private IEnumerable<LookupItem<int>> _outsideBrokers = [];
    //private IEnumerable<LookupItem<int>> _projects = [];
    //private IEnumerable<LookupItem<int>> _propertyTypes = [];

    //private IEnumerable<LookupItem<int>> _usages = [];
    //private IEnumerable<LookupItem<int>> _yesNoList = [];


    #endregion
    [Inject]
    private CloudBlueSettings cloudBlueSettings { set; get; }
    private async Task GetFileStream(long primeTcrId, long attachmentId, string fileName)
    {
        string attachmentFolder = Path.Combine(cloudBlueSettings.AttachmentsPath, primeTcrId.ToString());

        var extension = Path.GetExtension(fileName);

        var fileStorageUrl = $"{attachmentFolder}\\{attachmentId}.{extension}";

        if (File.Exists(fileStorageUrl) == false)
        {
            return;
        }
        var fileStream = File.OpenRead(fileStorageUrl);
        using var streamRef = new DotNetStreamReference(fileStream);
        await Js.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
    }

    #region Dependencies



    [Parameter]
    public long Id { set; get; }
    [Inject]

    private IPrimeTcrsService PrimeTcrsService { set; get; } = null!;
    [Inject]
    private ILookUpsService LookUpsService { get; set; } = null!;

    [Inject]
    private DialogService DialogService { set; get; } = null!;

    [CascadingParameter]
    public MainLayout MainLayout { get; set; }
    #endregion
    #region Actions
    private async Task ApplyBulkAction(RadzenSplitButtonItem? item)
    {
        if (item == null || item.Value == null)
        {
            return;
        }

        var itemValue = item.Value;

        if (string.IsNullOrEmpty(itemValue))
        {
            return;
        }

        SystemPrivileges action;
        var tempInt = 0;
        var result = int.TryParse(itemValue, out tempInt);

        if (result == false || tempInt == 0 || Enum.TryParse(tempInt.ToString(), out action) == false)
        {
            return;
        }

        var title = "";
        var questionMessage = "";


        switch (action)
        {
            case SystemPrivileges.PrimeTcrsCanSetReviewing:
                title = "Set Reviewing";
                questionMessage = "Are you sure you want to set selected item to Reviewing?";

                break;
            case SystemPrivileges.PrimeTcrsCanDelete:
                title = "Set Deleted";
                questionMessage = "Are you sure you want to delete selected item?";

                break;

            case SystemPrivileges.PrimeTcrsSetContracted:
                questionMessage = "Are you sure you want to set selected item to Contracted?";

                title = "Set Contracted";
                break;
            case SystemPrivileges.PrimeTcrsSetResolved:
                questionMessage = "Are you sure you want to set selected item to Resolved?";

                title = "Set Resolved";
                break;
            case SystemPrivileges.PrimeTcrsCanSetPostpone:
                questionMessage = "Are you sure you want to set selected item to Postpone?";

                title = "Set Postpone";
                break;
            case SystemPrivileges.PrimeTcrsCanSetReopen:
                questionMessage = "Are you sure you want to set selected items to Re_open?";
                //        
                title = "Set Re-Open";
                break;
            case SystemPrivileges.PrimeTcrsCanSetConflict:
                title = "Set Conflict";
                questionMessage = "Are you sure you want to set selected items to Conflict?";

                break;
            case SystemPrivileges.PrimeTcrsCanSetHalfCommissionCollected:
                title = "Set Half Commission Collected";
                questionMessage = "Are you sure you want to set selected items to Half Commission Collected?";

                break;
            case SystemPrivileges.PrimeTcrsSetConfirmedReserved:
                questionMessage = "Are you sure you want to set selected items to Confirmed Reserved?";
                title = "Set Confirmed Reserved";
                break;
            case SystemPrivileges.PrimeTcrsCanReloadCommissions:
                questionMessage = "Are you sure you want to reload commissions for selected item?";

                title = "Reload Commissions";

                break;
            case SystemPrivileges.PrimeTcrsCanUpdateCreationDate:
                questionMessage = "Are you sure you want to update creation date for selected item?";

                title = "Update Creation Date";
                break;

            case SystemPrivileges.PrimeTcrsChangeMarketingChannel:
                questionMessage = "Are you sure you want to change marketing channel for selected item?";
                title = "Change Marketing Channel";
                break;

            case SystemPrivileges.PrimeTcrsAddExtraManager:
                title = "Add Extra Manager";
                questionMessage = "Are you sure you want to add extra manager for selected item?";

                break;
            case SystemPrivileges.PrimeTcrsUpdateUnitType:
                questionMessage = "Are you sure you want to update unit type for selected item?";

                title = "Update Unit Type";
                break;
            case SystemPrivileges.PrimeTcrsUpdateUnitNumber:
                questionMessage = "Are you sure you want to update unit number for selected item?";

                title = "Update Unit Number";
                break;

            case SystemPrivileges.PrimeTcrsUpdateSalesVolume:
                questionMessage = "Are you sure you want to update sales volume for selected item?";
                title = "Update Sales Volume";
                break;


            case SystemPrivileges.PrimeTcrsCanUpdateConfirmationDate:
                title = "Update Confirmation Date";
                questionMessage = "Are you sure you want to update contract confirmation date for selected item?";
                break;
            case SystemPrivileges.PrimeTcrsUpdateConfirmedReservedDate:
                title = "Update Confirmation Date";
                questionMessage = "Are you sure you want to update reservation confirmation date for selected item?";

                break;

            case SystemPrivileges.PrimeTcrsCanSetInvoiced:
                questionMessage = "Are you sure you want to set selected items to Invoiced?";
                title = "Set Invoiced";
                break;
            case SystemPrivileges.PrimeTcrsSetConfirmedContracted:
                questionMessage = "Are you sure you want to set selected items to Confirmed Contracted?";
                title = "Set Confirmed Contracted";
                break;
            case SystemPrivileges.PrimeTcrsSetHalfConfirmedContracted:
                questionMessage = "Are you sure you want to set selected items to Half Confirmed Contracted?";
                title = "Set Half Confirmed Contracted";
                break;

            case SystemPrivileges.PrimeTcrsUpdateHalfConfirmedContracted:
                questionMessage = "Are you sure you want to update selected items Half Confirmed Contracted date?";
                title = "Set Half Confirmed Contracted";
                break;
            case SystemPrivileges.PrimeTcrsCanSetCanceledByDeveloper:
                questionMessage = "Are you sure you want to set selected items to Canceled By Developer?";
                //        
                title = "Set Canceled By Developer";
                break;

            case SystemPrivileges.PrimeTcrsAddDocumentDate:
                questionMessage = "Are you sure you want to add document information?";
                //        
                title = "Add Document Information";
                break;
            case SystemPrivileges.PrimeTcrsAddEvent:
                questionMessage = "";
                //        
                title = "Add Event";

                break;
            case SystemPrivileges.PrimeTcrsAddAttachments:
                questionMessage = "";
                //        
                title = "Attach Document";
                break;

            case SystemPrivileges.PrimeTcrsCanAddPayments:
                await AddPayment();
                return;

                break;
            case SystemPrivileges.PrimeTcrsVerify:
                await VerifyTcr();
                return;
                break;


            case SystemPrivileges.PrimeTcrsCanUpdateConfigsAndCommissions:
                await SaveConfigsAndCommissions();
                return;
                break;

        }


        var width = "700px";
        var height = "515px";
        var selectItems = new List<PrimeTcrItemForList>{new PrimeTcrItemForList
        {
            AllowedActions = _model.AllowedActions,
            Id = _model.PrimeTcrId,
            UsageId = _model.UsageId

        }};

        await DialogService.OpenAsync<PrimeTcrActionsPopUp>(title,
            new Dictionary<string, object>
            {
                { "PrimeTcrItems", selectItems },
                { "Action", action },
                { "Title", title },
                { "QuestionMessage", questionMessage },

            }, new DialogOptions { Width = width, Height = height });

        searched = false;
        await SearchForTcrAsync(_model.PrimeTcrId);
        // StateHasChanged();


    }

    private async Task SaveConfigsAndCommissions()
    {
        if (IsBusy)
        {
            return;

        }

        IsBusy = true;
        var result = await PrimeTcrsService.UpdatePrimeTcrConfigsAsync(_model);

        if (result == false)
        {
            MainLayout.ShowMessage(ToastLevel.Error, PrimeTcrsService.LastErrors);
        }
        else
        {
            MainLayout.ShowMessage(ToastLevel.Success, "Prime Tcr has been updated successfully");
            await SearchForTcrAsync(_model.PrimeTcrId);

        }

        IsBusy = false;
    }

    private async Task AttachDocument()
    {
        throw new NotImplementedException();
    }

    private async Task VerifyTcr()
    {
        throw new NotImplementedException();
    }

    private async Task AddPayment()
    {
        throw new NotImplementedException();
    }

    private async Task AddEvent()
    {
        throw new NotImplementedException();
    }

    #endregion
    #region Methods



    #endregion

    //private async Task PopulateLookups()
    //{
    //   // _outsideBrokers = await LookUpsService.GetOutsideBrokersAsync();
    //    //_developers = await LookUpsService.GetDevelopersAsync();

    //    //.Select(z => new LookupItem<int>(z.ItemName, z.ItemId, z.ExtraId, z.ParentItemId)).ToArray();
    //    //_allAgents = await LookUpsService.GetAgentsAsync();
    //    //_yesNoList = await LookUpsService.GetYesNoList("Select One");
    //    //_documentTypes = await LookUpsService.GetDocumentTypesList();

    //    //_agents = _allAgents.OrderBy(z => z.AgentName)
    //    //    .Select(z => new LookupItem<int>(z.AgentName, z.AgentId, z.SalesPersonClass.ToString(), 0))
    //    //    .ToArray();

    //    //_extraManagers = _allAgents.OrderBy(z => z.AgentName)
    //    //    .Where(z => z.SalesPersonClass != SalesPersonClasses.Agent && z.SalesPersonClass != SalesPersonClasses.Rec)
    //    //    .Select(z => new LookupItem<int>(z.AgentName, z.AgentId, z.SalesPersonClass.ToString(), 0))
    //    //    .ToArray();
    //}

    protected override async Task OnParametersSetAsync()
    {
        if (Id > 0)
        {
            await SearchForTcrAsync(Id);
        }

        await base.OnParametersSetAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await MainLayout.SetCurrentPageTitle($"View Prime Tcr - {Id}");
        }

        await base.OnAfterRenderAsync(firstRender);
    }
    private async Task SearchForTcrAsync(long id)
    {
        if (IsBusy || searched)
        {
            return;
        }

        IsBusy = true;
        _CollapseSearch = false;
        searched = true;
        _model = new PrimeTcrFullItem();
        _showForm = false;
        var tcr = await PrimeTcrsService.GetPrimeTcrForViewAsync(id);

        if (tcr == null)
        {
            MainLayout.ShowMessage(ToastLevel.Error, PrimeTcrsService.LastErrors);
            IsBusy = false;

            return;
        }

        _showForm = true;
        _CollapseSearch = true;
        _model = tcr;

        _systemEventItems = _model.SystemEvents;

        _systemEventItems.ForEach(z =>
            z.UserImagePath = $"/user-images/{z.UserCompanyId}/{z.UserBranchId}/{z.UserId}/{z.UserId}___Selected.jpg");

        _onImageError.TryAdd("onerror", "this.onerror=null; this.src='/images/default_user_image.jpg';");
        _count = _systemEventItems.Count;

        //   await PopulateLookups();
        IsBusy = false;
    }

    private async Task SearchForTcr()
    {
        searched = false;
        await SearchForTcrAsync(_model.PrimeTcrId);
    }

    //private void AgentsChanged()
    //{
    //    if (_model.SecondAgentId == 0)
    //    {
    //        _model.SecondAgentSharePercentage = 0;
    //    }

    //    if (_model.ThirdAgentId == 0)
    //    {
    //        _model.ThirdAgentSharePercentage = 0;
    //    }
    //}

    private async Task UpdatePrimeTcrAsync(PrimeTcrFullItem arg)
    {

    }

    private async Task UpdateTree(long primeTcrId, int agentId)
    {
        if (IsBusy)
        {
            return;
        }

        var result = await DialogService.Confirm("Are you sure you want to update this agent tree?", "Update Tree",
      new ConfirmOptions { OkButtonText = "Yes", CancelButtonText = "No" });

        if (result != true)
        {
            return;
        }

        IsBusy = true;

        result = await PrimeTcrsService.UpdatePrimeTcrAgentTreeAsync(primeTcrId, agentId);

        if (result.Value)
        {
            MainLayout.ShowMessage(ToastLevel.Success, "Agent tree has updated successfully");
        }
        else
        {
            MainLayout.ShowMessage(ToastLevel.Error, PrimeTcrsService.LastErrors);
        }

        IsBusy = false;
    }
}