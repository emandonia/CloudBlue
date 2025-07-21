using Blazored.Toast.Services;
using CloudBlue.Domain.Enums;
using CloudBlue.Domain.Utilities;
using CloudBlue.Web.Helpers;
using CloudBlue.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace CloudBlue.Web.Components.Layout;

public partial class BasicLayout : LayoutComponentBase
{
    public void ShowMessage(ToastLevel level, List<Errors> errors)
    {
        if (errors.Count == 0)
        {
            return;
        }


        var message = UtilityFunctions.ConstructMessage(errors);
        ShowMessage(level, message);
    }

    public async Task SetLoggedInUserSessionAsync(string apiKey)
    {
        await LocalStorageHelper.StoreItem(LiteralsHelper.ApiKey, apiKey);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await SetCurrentPageTitle("Login");
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    public void NavigateTo(string url, bool newTab = false)
    {
        try
        {
            if (newTab)
            {
                JsRuntime.InvokeVoidAsync("open", url, "_blank");
            }
            else
            {
                NavigationManager.NavigateTo(url, true);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            throw;
        }
    }

    public async Task SetCurrentPageTitle(string title)
    {
        await JsRuntime.InvokeVoidAsync("setTitle", $"CloudBlue - {title}");
    }

    public void ShowhideSpinner(bool status)
    {
        if (status)
        {
            SpinnerService.Show();
        }
        else
        {
            SpinnerService.Hide();
        }
    }

    public void ShowMessage(ToastLevel level, string message)
    {
        ToastService.ShowToast(level, message);
    }

    public void ShowMessage(ToastLevel level, List<string> messages)
    {
        ShowMessage(level, string.Join("; ", messages));
    }

    #region Dependencies

    [Inject]
    protected IJSRuntime JsRuntime { get; set; } = null!;

    [Inject]
    protected IToastService ToastService { set; get; } = null!;

    [Inject]
    protected SpinnerService SpinnerService { get; set; } = null!;

    [Inject]
    protected NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    private ILocalStorageHelper LocalStorageHelper { set; get; } = null!;

    #endregion Dependencies
}