using CloudBlue.Domain.DomainModels;
using CloudBlue.Domain.DomainModels.CallLeads;
using Microsoft.AspNetCore.Components;

namespace CloudBlue.Web.Components.Pages.Calls;

public partial class CallCardPage : ComponentBase
{
    private readonly Dictionary<string, object> _onImageError = new();
    private int _count;

    private List<SystemEventItem> _systemEventItems = new();
    [Parameter]
    public CallItemForList CallItem { get; set; } = null!;

    [Parameter]
    public bool ShowClose { get; set; } = true;

    //App_Themes/Blue/Images/userimage.jpg
    protected override async Task OnParametersSetAsync()
    {
        Array.ForEach(CallItem.SystemEvents,
            z => z.UserImagePath =
                $"/user-images/{z.UserCompanyId}/{z.UserBranchId}/{z.UserId}/{z.UserId}___Selected.jpg");

        _onImageError.TryAdd("onerror", "this.onerror=null; this.src='/images/default_user_image.jpg';");
        _count = CallItem.SystemEvents.Length;
        PopulateEvents(0, _count);
        StateHasChanged();
        await base.OnParametersSetAsync();
    }

    private void PopulateEvents(int skip, int take)
    {
        _systemEventItems = CallItem.SystemEvents.Skip(skip)
            .Take(take)
            .ToList();
    }
}