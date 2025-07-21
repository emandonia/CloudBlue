using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace NewCloudBlueMirror.Components.Pages
{
    public partial class EditDeveloper
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }
        [Inject]
        public CloudBlueService CloudBlueService { get; set; }

        [Parameter]
        public int DeveloperPk { get; set; }

        protected override async Task OnInitializedAsync()
        {
            developer = await CloudBlueService.GetDeveloperByDeveloperPk(DeveloperPk);
        }
        protected bool errorVisible;
        protected NewCloudBlueMirror.Models.CloudBlue.Developer developer;

        protected async Task FormSubmit()
        {
            try
            {
                await CloudBlueService.UpdateDeveloper(DeveloperPk, developer);
                DialogService.Close(developer);
            }
            catch (Exception ex)
            {
                hasChanges = ex is Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException;
                canEdit = !(ex is Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException);
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }


        protected bool hasChanges = false;
        protected bool canEdit = true;


        protected async Task ReloadButtonClick(MouseEventArgs args)
        {
           CloudBlueService.Reset();
            hasChanges = false;
            canEdit = true;

            developer = await CloudBlueService.GetDeveloperByDeveloperPk(DeveloperPk);
        }
    }
}