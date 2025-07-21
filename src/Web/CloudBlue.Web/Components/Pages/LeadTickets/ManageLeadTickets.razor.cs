using Blazored.Toast.Services;
using CloudBlue.Domain.DomainModels;
using CloudBlue.Domain.DomainModels.CallLeads;
using CloudBlue.Domain.DomainModels.Filtration.Interfaces;
using CloudBlue.Domain.Enums;
using CloudBlue.Domain.GenericTypes;
using CloudBlue.Domain.Interfaces.Services;
using CloudBlue.Domain.Utilities;
using CloudBlue.Web.Components.Layout;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using System.Globalization;

namespace CloudBlue.Web.Components.Pages.LeadTickets;

public partial class ManageLeadTickets : ComponentBase
{
    private bool _initialLoad;
    private bool opened = false;
    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        if (_initialLoad == false)
        {
            _initialLoad = true;
            await LoadData(new LoadDataArgs());
            await PopulateLookups();
        }
    }
    [Parameter]
    public long? Mode { set; get; }
    private async Task ResetFilters()
    {
        _leadsFilters = new LeadTicketsFiltersModel();
        ClientContactDevice = new ClientPhoneItem();
        _args.Skip = 0;
        _args.Filter = null;
        await LoadData(_args);
    }

    private async Task RefreshDataGrid()
    {
        await LoadData(_args);
    }

    #region Actions

    private async Task ApplySingleAction(RadzenSplitButtonItem? item)
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
        ContactingTypes contactingType;
        var idx = itemValue.IndexOf("_", StringComparison.Ordinal);

        if (idx == -1)
        {
            return;
        }

        var actionId = 0;
        var arr = itemValue.Split('_');
        var result = int.TryParse(arr[1], out actionId);

        if (result == false || actionId == 0 || Enum.TryParse(actionId.ToString(), out action) == false)
        {
            return;
        }

        var leadId = 0L;
        result = long.TryParse(arr[0], out leadId);
        var lead = _leads.FirstOrDefault(z => z.Id == leadId);

        if (lead == null)
        {
            return;
        }

        _selectItems.Clear();
        _selectItems.Add(lead);
        var isFeedback = actionId == (int)SystemPrivileges.LeadTicketsAddFeedback;
        var isReminder = arr.Length > 2 && arr[2] == "2";
        await ApplyAction(action, isFeedback, isReminder);
    }

    private async Task ApplyAction(SystemPrivileges action, bool isFeedback, bool isReminder)
    {

        if (action == SystemPrivileges.LeadTicketsExport)
        {
            //
            await ExportItemsAsync();
            return;
        }




        if (_selectItems.Count == 0)
        {
            Layout.ShowMessage(ToastLevel.Error, "You have to select at least one item");

            return;
        }

        if (action == SystemPrivileges.LeadTicketsMoveToCompany || action.ToString()
               .ToLower()
               .IndexOf("assign", StringComparison.Ordinal) > -1)
        {
            await ShowAssignBox(action);
        }
        else if (action == SystemPrivileges.LeadTicketsAddEvent || action == SystemPrivileges.LeadTicketsAddFeedback)
        {
            await ShowAddEventBox(action, isFeedback, isReminder);
        }
        else
        {
            await ShowActionBox(action);
        }

        _selectItems = new List<LeadTicketItemForList>();

        await LoadData(_args);
    }

    private async Task ExportItemsAsync()
    {
        if (_isBusy)
        {
            return;
        }

        _isBusy = true;
        Layout.ShowhideSpinner(true);
        var country = _countries.FirstOrDefault(z => z.ItemId == ClientContactDevice.CountryId);

        if (country != null)
        {
            ClientContactDevice.PhoneCountryCode = country.ExtraId;
        }

        _leadsFilters.ClientContactDevice = UtilityFunctions.GetCompositePhoneForSearch(
            ClientContactDevice.PhoneCountryCode, ClientContactDevice.PhoneAreaCode, ClientContactDevice.Phone);

        if (_leadsFilters.AssigningTypeId > 0)
        {
            _leadsFilters.AssigningType = (AssigningTypes)_leadsFilters.AssigningTypeId;
        }


        _leadsFilters.PageIndex = 0;
        _leadsFilters.PageSize = 10000;

        if (string.IsNullOrEmpty(_grid.Query.OrderBy) == false && _grid.Query.OrderBy.Length > 0 &&
           _grid.Query.OrderBy.IndexOf(" ", StringComparison.Ordinal) > 0 && _grid.Query.OrderBy.Split(" ")
               .Length == 2)
        {
            var arr = _grid.Query.OrderBy.Split(" ");
            _leadsFilters.SortField = arr[0];
            _leadsFilters.SortDirection = arr[1];
            _leadsFilters.ExtraFilters = _grid.Query.Filter;
        }

        if (_leadsFilters.ExtremeHoursFromList < 1000000)
        {
            _leadsFilters.ExtremeHours = _leadsFilters.ExtremeHoursFromList;
        }
        if (Mode != null && Mode > 0)
        {
            if (Mode >= 1000)
            {
                _leadsFilters.EntityIds = Mode.ToString();
            }
            else
            {



                _leadsFilters.ModeId = decimal.ToInt32(Mode.Value);
            }
        }
        _leadsFilters.ExportMode = true;
        var retObj = await LeadTicketsService.GetLeadTicketsAsync(_leadsFilters);


        _leadsFilters.ExportMode = false;

        var leads = retObj.Items;

        var properties = leads.First().GetType().GetProperties();

        var columns = _grid.ColumnsCollection.Where(z => string.IsNullOrEmpty(z.Title) == false && string.IsNullOrEmpty(z.Property) == false).Select(z => new ReportPropertyTitleItem { Property = z.Property, Title = z.Title }).ToList();

        columns.Add(new ReportPropertyTitleItem { Title = "Know Sub Source", Property = "KnowSubSource" });
        columns.Add(new ReportPropertyTitleItem { Title = "Country", Property = "Country" });
        columns.Add(new ReportPropertyTitleItem { Title = "City", Property = "City" });
        columns.Add(new ReportPropertyTitleItem { Title = "District", Property = "District" });
        columns.Add(new ReportPropertyTitleItem { Title = "Project", Property = "Project" });


        var stream = new MemoryStream();

        using (var document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
        {
            var workbookPart = document.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();

            var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet();

            var workbookStylesPart = workbookPart.AddNewPart<WorkbookStylesPart>();
            GenerateWorkbookStylesPartContent(workbookStylesPart);

            var sheets = workbookPart.Workbook.AppendChild(new Sheets());
            var sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Lead_Tickets_Export" };
            sheets.Append(sheet);

            workbookPart.Workbook.Save();

            var sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

            var headerRow = new Row();

            foreach (var column in columns)
            {
                headerRow.Append(new Cell()
                {
                    CellValue = new CellValue(column.Title),
                    DataType = new EnumValue<CellValues>(CellValues.String)
                });
            }

            sheetData.AppendChild(headerRow);

            foreach (var item in leads)
            {
                var row = new Row();

                foreach (var column in columns)
                {
                    var property = properties.FirstOrDefault(z => z.Name == column.Property);

                    if (property == null)
                    {
                        continue;
                    }
                    var value = property.GetValue(item);
                    var stringValue = $"{value}".Trim();

                    var cell = new Cell();

                    var underlyingType = property.PropertyType;

                    var typeCode = Type.GetTypeCode(underlyingType);

                    if (typeCode == TypeCode.DateTime)
                    {
                        if (!string.IsNullOrWhiteSpace(stringValue))
                        {
                            cell.CellValue = new CellValue() { Text = ((DateTime)value).ToOADate().ToString(System.Globalization.CultureInfo.InvariantCulture) };
                            cell.DataType = new EnumValue<CellValues>(CellValues.Number);
                            cell.StyleIndex = (UInt32Value)1U;
                        }
                    }
                    else if (typeCode == TypeCode.Boolean)
                    {
                        cell.CellValue = new CellValue(stringValue.ToLowerInvariant());
                        cell.DataType = new EnumValue<CellValues>(CellValues.Boolean);
                    }
                    else if (typeCode == TypeCode.Decimal || typeCode == TypeCode.Int32 || typeCode == TypeCode.Int64)
                    {
                        if (value != null)
                        {
                            stringValue = Convert.ToString(value, CultureInfo.InvariantCulture);
                        }
                        cell.CellValue = new CellValue(stringValue);
                        cell.DataType = new EnumValue<CellValues>(CellValues.Number);
                    }
                    else
                    {
                        cell.CellValue = new CellValue(stringValue);
                        cell.DataType = new EnumValue<CellValues>(CellValues.String);
                    }

                    row.Append(cell);
                }

                sheetData.AppendChild(row);
            }


            workbookPart.Workbook.Save();
        }

        if (stream?.Length > 0)
        {
            stream.Seek(0, SeekOrigin.Begin);
        }

        var fileName = $"Lead_Tickets_Export_{DateTime.UtcNow.ToString("yyyy_MM_dd")}.xlsx";
        using var streamRef = new DotNetStreamReference(stream);
        await JSRuntime.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);



        _isBusy = false;
        Layout.ShowhideSpinner(_lookupLoading);


    }

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



        await ApplyAction(action, true, false);
    }

    private async Task ShowActionBox(SystemPrivileges action)
    {
        var title = "";
        var width = "700px";
        var height = "515px";
        var privilege = Layout.CurrentUser.Privileges.FirstOrDefault(z => z.Privilege == action);

        if (privilege == null)
        {
            return;
        }

        var subTitle = string.Empty;

        switch (action)
        {
            case SystemPrivileges.LeadTicketsReject:
                title = "Reject Lead Ticket";
                subTitle = "Reject Reason";
                height = "545px";

                break;

            case SystemPrivileges.LeadTicketsSetVoid:
                title = "Void Lead Ticket";
                subTitle = "Void Reason";
                height = "545px";

                break;

            case SystemPrivileges.LeadTicketsArchive:
                title = "Archive Lead Ticket";
                subTitle = "Archive Comment";

                break;

            case SystemPrivileges.LeadTicketsUnArchive:
                title = "Un-Archive Lead Ticket";
                subTitle = "Un-Archive Comment";

                break;

            case SystemPrivileges.LeadTicketsConvertToBuyerRequest:
                title = "Convert To Buyer Request";

                break;

            case SystemPrivileges.LeadTicketsConvertToSellerRequest:
                title = "Convert To Seller Request";

                break;

            case SystemPrivileges.LeadTicketsConvertToPrimeTcr:
                title = "Convert To Prime Tcr";

                break;

            case SystemPrivileges.LeadTicketsCreateDuplicate:
                title = "Create Duplicate";

                break;

            default:
                return;
        }

        await DialogService.OpenAsync<ActionsPopUp>(title,
            new Dictionary<string, object>
            {
                { "LeadItems", _selectItems },
                { "Action", action },
                { "Title", title },
                { "SubTitle", subTitle },
                { "RefreshCallback", EventCallback.Factory.Create(this, RefreshDataGrid) }
            }, new DialogOptions { Width = width, Height = height });
    }

    private async Task ShowAssignBox(SystemPrivileges action)
    {
        var title = string.Empty;
        var width = "750px";
        var height = "515px";
        var subTitle = string.Empty;

        switch (action)
        {
            case SystemPrivileges.LeadTicketsAssignToAgent:
                title = "Assign To Agent";
                subTitle = "Agent";

                break;

            case SystemPrivileges.LeadTicketsAssignToBranch:
                title = "Assign To Branch";
                subTitle = "Branch";

                break;

            case SystemPrivileges.LeadTicketsReAssignToAgent:
                title = "Re-Assign To Agent";
                subTitle = "Agent";

                break;

            case SystemPrivileges.LeadTicketsReAssignToBranch:
                title = "Re-Assign To Branch";
                subTitle = "Branch";

                break;

            case SystemPrivileges.LeadTicketsMoveToCompany:
                title = "Move To Company";
                subTitle = "Company";

                break;
        }

        await DialogService.OpenAsync<AssigningPopUp>(title,
            new Dictionary<string, object>
            {
                { "LeadItems", _selectItems },
                { "Action", action },
                { "Title", title },
                { "SubTitle", subTitle },
                { "RefreshCallback", EventCallback.Factory.Create(this, RefreshDataGrid) }
            }, new DialogOptions { Width = width, Height = height });
    }

    private async Task ShowAddEventBox(SystemPrivileges action, bool isFeedback, bool isReminder)
    {
        var privilege = Layout.CurrentUser.Privileges.FirstOrDefault(z => z.Privilege == action);

        if (privilege == null)
        {
            return;
        }

        var width = "750px";
        var height = "500px";
        var title = "Add Activity Event";

        if (isFeedback)
        {
            title = "Add FeedBack";
        }
        else if (isReminder)
        {
            title = "Add Reminder";
        }

        await DialogService.OpenAsync<ActivityEventsPopUp>(title,
            new Dictionary<string, object>
            {
                { "LeadItems", _selectItems },
                { "Action", action },
                { "Title", title },
                { "IsFeedback", isFeedback },
                { "IsReminder", isReminder },
                { "RefreshCallback", EventCallback.Factory.Create(this, RefreshDataGrid) }
            }, new DialogOptions { Width = width, Height = height });
    }

    #endregion Actions

    #region Private methods and event Handlers

    private void SalesTypeChanged()
    {
        _usages = [];
        _propertyTypes = [];
        _services = [];
        _leadsFilters.UsageId = _leadsFilters.ServiceId = _leadsFilters.PropertyTypeId = 0;

        if (_leadsFilters.SalesTypeId == 0)
        {
            return;
        }

        var salesType = (SalesTypes)_leadsFilters.SalesTypeId;

        if (salesType == SalesTypes.Prime)
        {
            _usages = _allUsages.Where(z => z.ExtraId == "Prime")
                .ToArray();

            _services = _allServices.Where(z => z.ExtraId == "Prime")
                .ToArray();
        }
        else
        {
            _usages = _allUsages.ToArray();
            _services = _allServices.ToArray();
        }
    }

    private async Task PopulateAgents()
    {
        if (_lookupLoading)
        {
            return;
        }

        _lookupLoading = true;
        Layout.ShowhideSpinner(true);

        if (_companies.Count() == 1)
        {
            _leadsFilters.CompanyId = _companies.First()
                .ItemId;
        }

        if (_allAgentItems.Any() == false)
        {
            _allAgentItems = await LookUpsService.GetAgentsAsync(false);
        }

        if (_allAgentItems.Count() == 1)
        {
            _leadsFilters.AgentIdsList = _leadsFilters.AgentIdsList =
            [
                _allAgentItems.First()
                    .AgentId
            ];

            _isBusy = _lookupLoading = false;

            return;
        }

        _topManagers = _allAgentItems.Where(z => z.SalesPersonClass == SalesPersonClasses.TopManager)
            .Select(z => new LookupItem<int>(z.AgentName, z.AgentId, z.SalesPersonClass.ToString(), 0))
            .OrderBy(z => z.ItemName)
            .ToArray();

        //if (_topManagers.Count() == 1)
        //{
        //_leadsFilters.TopManagerId = _topManagers.First()
        //    .ItemId;
        //}
        SalesPersonClasses? salesClass = null;

        if (_leadsFilters.SalesPersonClassId > 0)
        {
            salesClass = (SalesPersonClasses)_leadsFilters.SalesPersonClassId;
        }

        _agents = _allAgentItems.Where(z =>
                (_leadsFilters.CompanyId == 0 || z.CompanyId == _leadsFilters.CompanyId) &&
                (salesClass == null || z.SalesPersonClass == salesClass) &&
                (_leadsFilters.BranchId == 0 || z.BranchId == _leadsFilters.BranchId) &&
                (_leadsFilters.TopManagerId == 0 || z.TopMostManagerId == _leadsFilters.TopManagerId))
            .Select(z => new LookupItem<int>(z.AgentName, z.AgentId, z.SalesPersonClass.ToString(), 0))
            .OrderBy(z => z.ItemName)
            .ToArray();

        if (_agents.Count() == 1)
        {
            _leadsFilters.AgentIdsList =
            [
                _agents.First()
                    .ItemId
            ];
        }

        _isBusy = _lookupLoading = false;
        Layout.ShowhideSpinner(false);
    }

    private async Task PopulateLookups()
    {
        if (_lookupLoading)
        {
            return;
        }

        Layout.ShowhideSpinner(true);
        _lookupLoading = true;
        _salesClasses = await LookUpsService.GetSalesPersonClasses(false);
        _countries = await LookUpsService.GetCountriesAsync();
        _gracePeriods = await LookUpsService.GetGracePeriodsAsync();
        _clientCategories = await LookUpsService.GetClientCategoriesAsync();
        _companies = await LookUpsService.GetCompaniesAsync();
        _knowItems = (await LookUpsService.GetKnowItemsAsync()).ToArray();
        _leadSources = (await LookUpsService.GetLeadSourcesAsync()).ToArray();
        _allUsages = await LookUpsService.GetUsageAsync();
        _internationalNoOptions = await LookUpsService.GetYesNoList();
        _assigningTypes = await LookUpsService.GetAssigningTypes();
        _salesTypes = await LookUpsService.GetSalesTypesAsync();
        _allAgentItems = await LookUpsService.GetAgentsAsync(false);
        _leadStatuses = await LookUpsService.GetLeadTicketStatusesAsync();
        _allServices = await LookUpsService.GetServicesAsync();
        _currencies = await LookUpsService.GetCurrenciesAsync();
        _agencies = await LookUpsService.GetMarketingAgenciesAsync();
        var feedbackItems = await LookUpsService.GetContactingTypesAsync();

        _feedbackItems = feedbackItems.Where(z => z.IsFeedBack || z.NeedsDate)
            .OrderBy(z => z.ContactingType)
            .Select(t => new LookupItem<int>(t.ContactingTypeName, t.Id, "", 0))
            .ToArray();

        _pendingActivitiesList = feedbackItems.Where(z => z.NeedsDate)
            .OrderBy(z => z.ContactingType)
            .Select(t => new LookupItem<int>(t.ContactingTypeName, t.Id, "", 0))
            .ToArray();

        Layout.ShowhideSpinner(_isBusy);
        _lookupLoading = false;
        await PopulateAgents();
        Layout.ShowhideSpinner(false);
    }

    private async Task Search(MouseEventArgs obj)
    {
        _args.Skip = 0;
        await LoadData(_args);
    }

    private async Task LoadData(LoadDataArgs args)
    {
        _args = args;

        if (_isBusy)
        {
            return;
        }

        _isBusy = true;
        Layout.ShowhideSpinner(true);
        var country = _countries.FirstOrDefault(z => z.ItemId == ClientContactDevice.CountryId);

        if (country != null)
        {
            ClientContactDevice.PhoneCountryCode = country.ExtraId;
        }

        _leadsFilters.ClientContactDevice = UtilityFunctions.GetCompositePhoneForSearch(
            ClientContactDevice.PhoneCountryCode, ClientContactDevice.PhoneAreaCode, ClientContactDevice.Phone);

        if (_leadsFilters.AssigningTypeId > 0)
        {
            _leadsFilters.AssigningType = (AssigningTypes)_leadsFilters.AssigningTypeId;
        }

        if (args.Top is > 0)
        {
            _leadsFilters.PageSize = args.Top.Value;
        }

        _leadsFilters.PageIndex = (args.Skip ?? 0) / _leadsFilters.PageSize;

        if (string.IsNullOrEmpty(args.OrderBy) == false && args.OrderBy.Length > 0 &&
           args.OrderBy.IndexOf(" ", StringComparison.Ordinal) > 0 && args.OrderBy.Split(" ")
               .Length == 2)
        {
            var arr = args.OrderBy.Split(" ");
            _leadsFilters.SortField = arr[0];
            _leadsFilters.SortDirection = arr[1];
            _leadsFilters.ExtraFilters = args.Filter;
        }

        if (_leadsFilters.ExtremeHoursFromList < 1000000)
        {
            _leadsFilters.ExtremeHours = _leadsFilters.ExtremeHoursFromList;
        }
        if (Mode != null && Mode > 0)
        {
            if (Mode >= 1000)
            {
                _leadsFilters.EntityIds = Mode.ToString();
            }
            else
            {



                _leadsFilters.ModeId = decimal.ToInt32(Mode.Value);
            }
        }
        var retObj = await LeadTicketsService.GetLeadTicketsAsync(_leadsFilters);
        _totalNumber = retObj.TotalCount;

        foreach (var item in retObj.Items)
        {
            item.SalesTypeBadgeStyle = (BadgeStyle)Enum.Parse(typeof(BadgeStyle), item.SalesTypeBackgroundColor!, true);
            item.ServiceBadgeStyle = (BadgeStyle)Enum.Parse(typeof(BadgeStyle), item.ServiceBackgroundColor!, true);
            item.StatusBadgeStyle = (BadgeStyle)Enum.Parse(typeof(BadgeStyle), item.StatusBackgroundColor!, true);
            item.UsageBadgeStyle = (BadgeStyle)Enum.Parse(typeof(BadgeStyle), item.UsageBackgroundColor!, true);
        }

        _leads = retObj.Items;

        if (Mode != null && Mode > 1000 && opened == false)
        {
            await OpenLeadTicket(Mode.Value, false);
            opened = true;
        }

        _isBusy = false;
        Layout.ShowhideSpinner(_lookupLoading);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await Layout.SetCurrentPageTitle("Manage Lead Tickets");
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task OpenLeadTicket(long id, bool mode)
    {

        if (mode)
        {
            Layout.NavigateTo($"/lead-tickets/view/{id}", true);


            return;
        }



        var leadItem = _leads.FirstOrDefault(z => z.Id == id);

        if (leadItem == null)
        {
            return;
        }

        await DialogService.OpenAsync<PeekLeadTicket>(
            $"Lead Ticket {id} - {leadItem.CurrentAgent} - {leadItem.LeadTicketStatus}",
            new Dictionary<string, object>
            {
                { "LeadItem", leadItem }, { "RefreshCallback", EventCallback.Factory.Create(this, RefreshDataGrid) }
            }, new DialogOptions { Width = "800px", Height = "600px" });
    }

    private void SelectItem(bool args, LeadTicketItemForList data)
    {
        if (args)
        {
            _selectItems.Add(data);
        }
        else
        {
            _selectItems.Remove(data);
        }
    }

    private void SelectAllItems(bool? args)
    {
        if (args == true)
        {
            Array.ForEach(_leads, z => _selectItems.Add(z));
        }
        else
        {
            if (_selectItems.Count == _leads.Length)
            {
                _selectItems.Clear();
            }
        }
    }

    private void UsageChanged()
    {
        var item = _usages.FirstOrDefault(z => z.ItemId == _leadsFilters.UsageId);
        _leadsFilters.PropertyTypeId = 0;

        if (item != null)
        {
            _propertyTypes = item.SubLookUps;
        }
    }

    private void CountryChanged()
    {
        var item = _countries.FirstOrDefault(z => z.ItemId == _leadsFilters.CountryId);
        _leadsFilters.CityId = _leadsFilters.DistrictId = _leadsFilters.NeighborhoodId = 0;

        if (item != null)
        {
            _cities = item.SubLookUps;
        }
    }

    private void ShowTooltip(ElementReference elementReference, string valExtraId)
    {
        TooltipService.Open(elementReference, valExtraId);
    }

    private void StatusChanged()
    {
        _disableExtremeHours = true;
        _leadsFilters.ExtremeHours = 0;

        if (_leadsFilters.EntityStatusIds == null || _leadsFilters.EntityStatusIds.Count() != 1 ||
           _allowedStatusesId.Contains(_leadsFilters.EntityStatusIds.First()) == false)
        {
            return;
        }

        _disableExtremeHours = false;
    }

    private void CityChanged()
    {
        var item = _cities.FirstOrDefault(z => z.ItemId == _leadsFilters.CityId);
        _leadsFilters.DistrictId = _leadsFilters.NeighborhoodId = 0;

        if (item != null)
        {
            _districts = item.SubLookUps;
        }
    }

    private void DistrictChanged()
    {
        var item = _districts.FirstOrDefault(z => z.ItemId == _leadsFilters.DistrictId);
        _leadsFilters.NeighborhoodId = 0;

        if (item != null)
        {
            _neighborhoods = item.SubLookUps;
        }
    }

    private void KnowSourceChanged()
    {
        var knowItem = _knowItems.FirstOrDefault(z => z.ItemId == _leadsFilters.KnowSourceId);
        _leadsFilters.KnowSubSourceId = 0;

        if (knowItem != null)
        {
            _knowSubItems = knowItem.SubLookUps.ToArray();
        }
    }

    #endregion Private methods and event Handlers
    private static void GenerateWorkbookStylesPartContent(WorkbookStylesPart workbookStylesPart1)
    {
        Stylesheet stylesheet1 = new Stylesheet() { MCAttributes = new MarkupCompatibilityAttributes() { Ignorable = "x14ac x16r2 xr" } };
        stylesheet1.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        stylesheet1.AddNamespaceDeclaration("x14ac", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac");
        stylesheet1.AddNamespaceDeclaration("x16r2", "http://schemas.microsoft.com/office/spreadsheetml/2015/02/main");
        stylesheet1.AddNamespaceDeclaration("xr", "http://schemas.microsoft.com/office/spreadsheetml/2014/revision");

        Fonts fonts1 = new Fonts() { Count = (UInt32Value)1U, KnownFonts = true };

        Font font1 = new Font();
        FontSize fontSize1 = new FontSize() { Val = 11D };
        Color color1 = new Color() { Theme = (UInt32Value)1U };
        FontName fontName1 = new FontName() { Val = "Calibri" };
        FontFamilyNumbering fontFamilyNumbering1 = new FontFamilyNumbering() { Val = 2 };
        FontScheme fontScheme1 = new FontScheme() { Val = FontSchemeValues.Minor };

        font1.Append(fontSize1);
        font1.Append(color1);
        font1.Append(fontName1);
        font1.Append(fontFamilyNumbering1);
        font1.Append(fontScheme1);

        fonts1.Append(font1);

        Fills fills1 = new Fills() { Count = (UInt32Value)2U };

        Fill fill1 = new Fill();
        PatternFill patternFill1 = new PatternFill() { PatternType = PatternValues.None };

        fill1.Append(patternFill1);

        Fill fill2 = new Fill();
        PatternFill patternFill2 = new PatternFill() { PatternType = PatternValues.Gray125 };

        fill2.Append(patternFill2);

        fills1.Append(fill1);
        fills1.Append(fill2);

        Borders borders1 = new Borders() { Count = (UInt32Value)1U };

        Border border1 = new Border();
        LeftBorder leftBorder1 = new LeftBorder();
        RightBorder rightBorder1 = new RightBorder();
        TopBorder topBorder1 = new TopBorder();
        BottomBorder bottomBorder1 = new BottomBorder();
        DiagonalBorder diagonalBorder1 = new DiagonalBorder();

        border1.Append(leftBorder1);
        border1.Append(rightBorder1);
        border1.Append(topBorder1);
        border1.Append(bottomBorder1);
        border1.Append(diagonalBorder1);

        borders1.Append(border1);

        CellStyleFormats cellStyleFormats1 = new CellStyleFormats() { Count = (UInt32Value)1U };
        CellFormat cellFormat1 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U };

        cellStyleFormats1.Append(cellFormat1);

        CellFormats cellFormats1 = new CellFormats() { Count = (UInt32Value)2U };
        CellFormat cellFormat2 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U, FormatId = (UInt32Value)0U };
        CellFormat cellFormat3 = new CellFormat() { NumberFormatId = (UInt32Value)14U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U, FormatId = (UInt32Value)0U, ApplyNumberFormat = true };

        cellFormats1.Append(cellFormat2);
        cellFormats1.Append(cellFormat3);

        CellStyles cellStyles1 = new CellStyles() { Count = (UInt32Value)1U };
        CellStyle cellStyle1 = new CellStyle() { Name = "Normal", FormatId = (UInt32Value)0U, BuiltinId = (UInt32Value)0U };

        cellStyles1.Append(cellStyle1);
        DifferentialFormats differentialFormats1 = new DifferentialFormats() { Count = (UInt32Value)0U };
        TableStyles tableStyles1 = new TableStyles() { Count = (UInt32Value)0U, DefaultTableStyle = "TableStyleMedium2", DefaultPivotStyle = "PivotStyleLight16" };

        StylesheetExtensionList stylesheetExtensionList1 = new StylesheetExtensionList();

        StylesheetExtension stylesheetExtension1 = new StylesheetExtension() { Uri = "{EB79DEF2-80B8-43e5-95BD-54CBDDF9020C}" };
        stylesheetExtension1.AddNamespaceDeclaration("x14", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/main");

        StylesheetExtension stylesheetExtension2 = new StylesheetExtension() { Uri = "{9260A510-F301-46a8-8635-F512D64BE5F5}" };
        stylesheetExtension2.AddNamespaceDeclaration("x15", "http://schemas.microsoft.com/office/spreadsheetml/2010/11/main");

        OpenXmlUnknownElement openXmlUnknownElement4 = workbookStylesPart1.CreateUnknownElement("<x15:timelineStyles defaultTimelineStyle=\"TimeSlicerStyleLight1\" xmlns:x15=\"http://schemas.microsoft.com/office/spreadsheetml/2010/11/main\" />");

        stylesheetExtension2.Append(openXmlUnknownElement4);

        stylesheetExtensionList1.Append(stylesheetExtension1);
        stylesheetExtensionList1.Append(stylesheetExtension2);

        stylesheet1.Append(fonts1);
        stylesheet1.Append(fills1);
        stylesheet1.Append(borders1);
        stylesheet1.Append(cellStyleFormats1);
        stylesheet1.Append(cellFormats1);
        stylesheet1.Append(cellStyles1);
        stylesheet1.Append(differentialFormats1);
        stylesheet1.Append(tableStyles1);
        stylesheet1.Append(stylesheetExtensionList1);

        workbookStylesPart1.Stylesheet = stylesheet1;
    }

    #region Local variables

    private readonly int[] _allowedStatusesId =
    [
        (int)LeadTicketStatuses.InProgress, (int)LeadTicketStatuses.Assigned
    ];

    private LeadTicketsFiltersModel _leadsFilters = new();
    private LoadDataArgs _args;

    private bool _lookupLoading;

    private ClientPhoneItem ClientContactDevice { get; set; } = new();

    private int _totalNumber;
    private bool _isBusy;
    private bool _disableExtremeHours = true;
    private IEnumerable<LookupItem<int>> _currencies = [];

    private IEnumerable<LookupItem<int>> _allServices = [];
    private IEnumerable<LookupItem<int>> _services = [];
    private IEnumerable<LookupItem<int>> _clientCategories = [];
    private IEnumerable<LookupItem<int>> _feedbackItems = [];

    private IEnumerable<LookupItem<int>> _topManagers = [];
    private IEnumerable<ContactingTypeItem> ContactingTypeItems { set; get; } = [];
    private IEnumerable<LookupItem<int>> _leadStatuses = [];
    private IEnumerable<LookupItem<int>> _salesClasses = [];
    private IEnumerable<LookupItem<int>> _companies = [];
    private IEnumerable<LookupItem<int>> _knowSubItems = [];
    private IEnumerable<LookupItem<int>> _leadSources = [];
    private IEnumerable<LookupItem<int>> _knowItems = [];
    private IEnumerable<LookupItem<int>> _countries = [];
    private IEnumerable<LookupItem<int>> _branches = [];
    private IEnumerable<AgentItem> _allAgentItems = [];

    private IEnumerable<LookupItem<int>> _salesTypes = [];
    private IEnumerable<LookupItem<int>> _usages = [];
    private IEnumerable<LookupItem<int>> _allUsages = [];
    private IEnumerable<LookupItem<int>> _propertyTypes = [];

    private IEnumerable<LookupItem<int>> _assigningTypes = [];
    private IEnumerable<LookupItem<int>> _internationalNoOptions = [];
    private IEnumerable<LookupItem<int>> _gracePeriods = [];
    private IEnumerable<LookupItem<int>> _cities = [];
    private IEnumerable<LookupItem<int>> _agents = [];
    private IEnumerable<LookupItem<int>> _agencies = [];
    private IEnumerable<LookupItem<int>> _districts = [];
    private IEnumerable<LookupItem<int>> _neighborhoods = [];
    private readonly IEnumerable<LookupItem<int>> _archivingList = [new("Yes", 1, "", 0), new("No", 2, "", 0)];

    private IEnumerable<LookupItem<int>> _pendingActivitiesList = [];

    private LeadTicketItemForList[] _leads = [];
    private IList<LeadTicketItemForList> _selectItems = new List<LeadTicketItemForList>();
    private RadzenDataGrid<LeadTicketItemForList>? _grid;

    #endregion Local variables

    #region Dependencies

    [Inject]
    private TooltipService TooltipService { set; get; } = null!;

    [Inject]
    private ILookUpsService LookUpsService { get; set; } = null!;

    [Inject]
    protected DialogService DialogService { get; set; }

    [Inject]
    public ILeadTicketsService LeadTicketsService { get; set; }

    [CascadingParameter]
    public MainLayout Layout { get; set; }

    #endregion Dependencies
}