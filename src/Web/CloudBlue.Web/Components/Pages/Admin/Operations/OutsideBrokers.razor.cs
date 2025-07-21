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
    public partial class OutsideBrokers
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

        protected IEnumerable<NewCloudBlueMirror.Models.CloudBlue.OutsideBroker> outsideBrokers;

        protected RadzenDataGrid<NewCloudBlueMirror.Models.CloudBlue.OutsideBroker> grid0;

        protected string search = "";

        protected async Task Search(ChangeEventArgs args)
        {
            search = $"{args.Value}";

            await grid0.GoToPage(0);

            outsideBrokers = await CloudBlueService.GetOutsideBrokers(new Query { Filter = $@"i => i.OutsideBrokerName.Contains(@0) || i.UserName.Contains(@0) || i.PasswordSalt.Contains(@0) || i.PasswordHash.Contains(@0)", FilterParameters = new object[] { search } });
        }
        protected override async Task OnInitializedAsync()
        {
            outsideBrokers = await CloudBlueService.GetOutsideBrokers(new Query { Filter = $@"i => i.OutsideBrokerName.Contains(@0) || i.UserName.Contains(@0) || i.PasswordSalt.Contains(@0) || i.PasswordHash.Contains(@0)", FilterParameters = new object[] { search } });
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddOutsideBroker>("Add OutsideBroker", null);
            await grid0.Reload();
        }

        protected async Task EditRow(DataGridRowMouseEventArgs<NewCloudBlueMirror.Models.CloudBlue.OutsideBroker> args)
        {
            await DialogService.OpenAsync<EditOutsideBroker>("Edit OutsideBroker", new Dictionary<string, object> { {"OutsideBrokerPk", args.Data.OutsideBrokerPk} });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, NewCloudBlueMirror.Models.CloudBlue.OutsideBroker outsideBroker)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await CloudBlueService.DeleteOutsideBroker(outsideBroker.OutsideBrokerPk);

                    if (deleteResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error",
                    Detail = $"Unable to delete OutsideBroker"
                });
            }
        }

        protected async Task ExportClick(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await CloudBlueService.ExportOutsideBrokersToCSV(new Query
                {
                    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
                    OrderBy = $"{grid0.Query.OrderBy}",
                    Expand = "",
                    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
                }, "OutsideBrokers");
            }

            if (args == null || args.Value == "xlsx")
            {
                await CloudBlueService.ExportOutsideBrokersToExcel(new Query
                {
                    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
                    OrderBy = $"{grid0.Query.OrderBy}",
                    Expand = "",
                    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
                }, "OutsideBrokers");
            }
        }
    }
}