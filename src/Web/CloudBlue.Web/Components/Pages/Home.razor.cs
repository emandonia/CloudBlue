using CloudBlue.Domain.DomainModels.DashboardStuff;
using CloudBlue.Domain.Interfaces.Services;
using CloudBlue.Web.Components.Layout;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace CloudBlue.Web.Components.Pages;

public partial class Home : ComponentBase
{


    [CascadingParameter]
    public required MainLayout Layout { get; set; }



    private DashboardContent _dashboardContent = new();
    [Inject]
    private IDashboardService DashboardService { get; set; } = null!;

    private readonly PeriodicTimer _timer = new PeriodicTimer(TimeSpan.FromSeconds(90));
    private readonly CancellationTokenSource _cts = new();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await Layout.SetCurrentPageTitle("Home");
        }

        await base.OnAfterRenderAsync(firstRender);
    }


    protected override async Task OnInitializedAsync()
    {
        await FetchDataAsync();

        _ = StartTimerAsync(); // Run the timer in the background
    }

    private async Task StartTimerAsync()
    {
        try
        {
            while (await _timer.WaitForNextTickAsync(_cts.Token))
            {
                await FetchDataAsync();
                // StateHasChanged(); // Refresh UI
            }
        }
        catch (OperationCanceledException)
        {
            // Timer was stopped
        }
    }

    private RadzenTree _radzenTree;
    private async Task FetchDataAsync()
    {
        _dashboardContent = await DashboardService.GetDashboardContentsAsync();

        foreach (var item in _dashboardContent.LeadTicketsNotifications)
        {

            if (item.Count == 0)
            {
                item.LinkUrl = string.Empty;
                item.StatusBadgeStyle = ProgressBarStyle.Success;
            }
            else if (item.Count > 20)
            {
                item.StatusBadgeStyle = ProgressBarStyle.Danger;


            }
            else if (item.Count > 10)
            {
                item.StatusBadgeStyle = ProgressBarStyle.Warning;


            }
            else if (item.Count <= 10)
            {
                item.StatusBadgeStyle = ProgressBarStyle.Info;
            }


        }
        foreach (var item in _dashboardContent.PrimeTcrsNotifications)
        {

            if (item.Count == 0)
            {
                item.LinkUrl = string.Empty;
                item.StatusBadgeStyle = BadgeStyle.Success;
            }
            else if (item.Count > 20)
            {
                item.StatusBadgeStyle = BadgeStyle.Danger;


            }
            else if (item.Count > 10)
            {
                item.StatusBadgeStyle = BadgeStyle.Warning;


            }
            else if (item.Count <= 10)
            {
                item.StatusBadgeStyle = BadgeStyle.Info;
            }


        }



    }
    void OnChange(string? args)
    {
        if (string.IsNullOrEmpty(args))
        {
            return;
        }
        Layout.NavigateTo(args);
    }

    public void Dispose()
    {
        _cts.Cancel();
        _timer.Dispose();
        _cts.Dispose();
    }

}