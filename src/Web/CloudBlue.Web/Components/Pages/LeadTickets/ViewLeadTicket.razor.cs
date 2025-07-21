using CloudBlue.Domain.DomainModels;
using CloudBlue.Domain.DomainModels.CallLeads;
using CloudBlue.Domain.DomainModels.Filtration.Interfaces;
using CloudBlue.Domain.Enums;
using CloudBlue.Domain.Interfaces.Services;
using CloudBlue.Web.Components.Layout;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace CloudBlue.Web.Components.Pages.LeadTickets;

public partial class ViewLeadTicket : ComponentBase
{
    private readonly Dictionary<string, object> _onImageError = new();

    private int _count;

    [Parameter]
    public long Id { set; get; }
    private List<SystemEventItem> _systemEventItems = new();
    private LeadTicketItemForList LeadItem { get; set; } = null!;

    [CascadingParameter]

    private MainLayout Layout { set; get; }

    [Inject]
    private ISystemEventsService SystemEventsService { set; get; } = null!;
    [Inject]

    private ILeadTicketsService LeadTicketsService { set; get; } = null!;
    private bool _isBusy;

    private bool IsBusy
    {
        set
        {
            _isBusy = value;
            Layout.ShowhideSpinner(_isBusy);
        }
        get => _isBusy;
    }

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        if (IsBusy)
        {
            return;
        }

        IsBusy = true;
        if (Id > 0)
        {
            await SearchForLeadTicketAsync(Id);
        }

        IsBusy = false;


    }

    private bool _showTabs;
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await Layout.SetCurrentPageTitle($"View Lead Ticket");
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    private string? _clientMobile = "";
    private string? _clientEmail = "";
    private bool _mobileDisabled = true;
    private bool _whatsAppDisabled = true;
    private bool _emailDisabled = true;
    private async Task SearchForLeadTicketAsync(long id)
    {
        var retObj =
           await LeadTicketsService.GetLeadTicketsAsync(
               new LeadTicketsFiltersModel { EntityIds = Id.ToString() });

        if (retObj.Items.Length == 0)
        {
            return;
        }

        LeadItem = retObj.Items[0];
        var device = LeadItem.ClientContactDevices.FirstOrDefault(z => z.DeviceTypeId == 1);

        if (device != null)
        {
            _clientMobile = device.LoweredDeviceInfo;
        }

        device = LeadItem.ClientContactDevices.FirstOrDefault(z => z.DeviceTypeId == 4);

        if (device != null)
        {
            _clientEmail = device.LoweredDeviceInfo;
        }

        var isMobile = false;
        try
        {

            isMobile = await Js.InvokeAsync<bool>("isMobileDevice");

        }
        catch (Exception e)
        {
            Console.WriteLine(e);


        }

        if (string.IsNullOrEmpty(_clientMobile) == false)
        {
            _whatsAppDisabled = false;
            _mobileDisabled = !isMobile;
        }
        if (string.IsNullOrEmpty(_clientEmail) == false)
        {
            _emailDisabled = false;


        }

        if (Layout.CurrentUser.UserId == LeadItem.CurrentAgentId &&
           LeadItem.LeadTicketStatusId == LeadTicketStatuses.Assigned)
        {
            await LeadTicketsService.UpdateLeadTicketViewedByAgentAsync(Id);
        }

        LeadItem.SystemEvents.ForEach(z =>
            z.UserImagePath = $"/user-images/{z.UserCompanyId}/{z.UserBranchId}/{z.UserId}/{z.UserId}___Selected.jpg");

        _onImageError.TryAdd("onerror", "this.onerror=null; this.src='/images/default_user_image.jpg';");
        _count = LeadItem.SystemEvents.Count;
        PopulateEvents(0, _count);
        _showTabs = true;

    }

    private void PopulateEvents(int skip, int take)
    {
        _systemEventItems = LeadItem.SystemEvents.Skip(skip)
            .Take(take)
            .ToList();
    }

    private async Task ContactClient(int mode)
    {
        try
        {

            if (mode == 1 && _mobileDisabled == false)
            {
                await Js.InvokeVoidAsync("callPhone", _clientMobile);

                var comment = $"{Layout.CurrentUser.FullName} initiated a call with client";
                await SystemEventsService.AddEventAsync(LeadItem.Id, EntityTypes.LeadTicket,
                    EventTypes.LeadTicketFollowUp, LeadItem.ClientId, Layout.CurrentUser.UserId, comment,
                    EventProcesses.FollowUp, (int)ContactingTypes.Call);
            }
            else if (mode == 2 && _whatsAppDisabled == false)
            {

                await Js.InvokeVoidAsync("openWhatsApp", _clientMobile, $"Hello! I’m {Layout.CurrentUser.FullName}, a real estate broker. Do you have a moment to discuss your property needs or schedule a viewing?");

            }
            else if (mode == 3 && _emailDisabled == false)
            {

                string subject = "Property Inquiry – Let's Find Your Perfect Home";
                string body = $"Hello,\n\nThis is {Layout.CurrentUser.FullName}, a licensed real estate broker. I wanted to reach out and see how I can assist you in finding the right property. Please let me know what type of property you're looking for, and I’ll be happy to share options that match your needs.\n\nLooking forward to your reply!\n\nBest regards,\n{Layout.CurrentUser.Email}";

                await Js.InvokeVoidAsync("openMail", _clientEmail, subject, body);
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e);

        }
    }
}