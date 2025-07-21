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
    public partial class EditDevelopersProject
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
        public int DeveloperProjectPk { get; set; }

        protected override async Task OnInitializedAsync()
        {
            developersProject = await CloudBlueService.GetDevelopersProjectByDeveloperProjectPk(DeveloperProjectPk);
        }
        protected bool errorVisible;
        protected NewCloudBlueMirror.Models.CloudBlue.DevelopersProject developersProject;

        protected async Task FormSubmit()
        {
            try
            {
                await CloudBlueService.UpdateDevelopersProject(DeveloperProjectPk, developersProject);
                DialogService.Close(developersProject);
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

            developersProject = await CloudBlueService.GetDevelopersProjectByDeveloperProjectPk(DeveloperProjectPk);
        }
    }
}