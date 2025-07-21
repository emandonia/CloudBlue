using Blazored.Toast.Services;
using CloudBlue.Domain.DomainModels;
using CloudBlue.Domain.DomainModels.Filtration.Interfaces;
using CloudBlue.Domain.DomainModels.PrimeTcrs;
using CloudBlue.Domain.Enums;
using CloudBlue.Domain.GenericTypes;
using CloudBlue.Domain.Interfaces.Services;
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

namespace CloudBlue.Web.Components.Pages.Tcrs.PrimeTcrs;

public partial class ManagePrimeTcrs : ComponentBase
{
    private bool _accountingOnlyColumns;
    private bool _accountingOnlyFilters;
    private IEnumerable<LookupItem<int>> _agencies = [];
    private IEnumerable<LookupItem<int>> _agents = [];
    private IEnumerable<AgentItem> _allAgentItems = [];
    private IEnumerable<LookupItem<int>> _allUsages = [];

    private LoadDataArgs? _args;
    private IEnumerable<LookupItem<int>> _branches = [];
    private IEnumerable<LookupItem<int>> _cities = [];
    private IEnumerable<LookupItem<int>> _closingKnowSubItems = [];
    private IEnumerable<LookupItem<int>> _companies = [];
    private IEnumerable<LookupItem<int>> _countries = [];
    private IEnumerable<LookupItem<int>> _developers = [];
    private IEnumerable<LookupItem<int>> _districts = [];
    private PrimeTcrsFiltersModel _filters = new();
    private RadzenDataGrid<PrimeTcrItemForList>? _grid;

    private bool _initialLoad;
    private bool _isBusy;
    private IEnumerable<LookupItem<int>> _knowItems = [];
    private IEnumerable<LookupItem<int>> _knowSubItems = [];
    private IEnumerable<LookupItem<int>> _leadSources = [];

    private bool _lookupLoading;
    private IEnumerable<LookupItem<int>> _neighborhoods = [];

    private PrimeTcrItemForList[] _primeTcrs = [];

    private IEnumerable<LookupItem<int>> _primeTcrStatuses = [];
    private IEnumerable<LookupItem<int>> _projects = [];
    private IEnumerable<LookupItem<int>> _propertyTypes = [];
    private IEnumerable<LookupItem<int>> _salesClasses = [];
    private int _salesPersonClassId;
    private IList<PrimeTcrItemForList> _selectItems = new List<PrimeTcrItemForList>();

    private int _topManagerId;
    [Parameter]
    public int? Mode { set; get; }
    private IEnumerable<LookupItem<int>> _topManagers = [];

    private int _totalNumber;

    private IEnumerable<LookupItem<int>> _usages = [];
    private IEnumerable<LookupItem<int>> _verificationStatuses = [];
    private IEnumerable<LookupItem<int>> _yesNoOptions = [];

    [CascadingParameter]
    public MainLayout Layout { get; set; }
    [Inject]
    private ILookUpsService LookUpsService { get; set; } = null!;
    [Inject]

    private IPrimeTcrsService PrimeTcrsService { get; set; } = null!;

    [Inject]
    protected DialogService DialogService { get; set; }

    [Inject]
    private TooltipService TooltipService { set; get; } = null!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await Layout.SetCurrentPageTitle("Manage Lead Prime Tcrs");
        }

        await base.OnAfterRenderAsync(firstRender);
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
            _filters.CompanyId = _companies.First()
                .ItemId;
        }

        if (_allAgentItems.Any() == false)
        {
            _allAgentItems = await LookUpsService.GetAgentsAsync(false);
        }

        if (_filters.CompanyId > 0)
        {
            _branches = _companies.First(z => z.ItemId == _filters.CompanyId)
                .SubLookUps;
        }
        else
        {
            _filters.BranchId = 0;
            _filters.AgentsRecursive = false;
            _filters.AgentId = 0;
        }
        if (_allAgentItems.Count() == 1)
        {
            _filters.AgentId = _allAgentItems.First()
                .AgentId;


        }

        _topManagers = _allAgentItems.Where(z =>
                z.SalesPersonClass == SalesPersonClasses.TopManager &&
                (_filters.CompanyId == 0 || z.CompanyId == _filters.CompanyId) &&
                (_filters.BranchId == 0 || z.BranchId == _filters.BranchId))
            .Select(z => new LookupItem<int>(z.AgentName, z.AgentId, z.SalesPersonClass.ToString(), 0))
            .OrderBy(z => z.ItemName)
            .ToArray();

        SalesPersonClasses? salesClass = null;

        if (_salesPersonClassId > 0)
        {
            salesClass = (SalesPersonClasses)_salesPersonClassId;
        }

        _agents = _allAgentItems.Where(z =>
                (_filters.CompanyId == 0 || z.CompanyId == _filters.CompanyId) &&
                (salesClass == null || z.SalesPersonClass == salesClass) &&
                (_filters.BranchId == 0 || z.BranchId == _filters.BranchId) &&
                (_topManagerId == 0 || z.TopMostManagerId == _topManagerId))
            .Select(z => new LookupItem<int>(z.AgentName, z.AgentId, z.SalesPersonClass.ToString(), 0))
            .OrderBy(z => z.ItemName)
            .ToArray();

        if (_agents.Count() == 1)
        {
            _filters.AgentId = _agents.First()
                .ItemId;
        }

        _isBusy = _lookupLoading = false;
        Layout.ShowhideSpinner(false);
    }

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        if (_initialLoad == false)
        {
            _initialLoad = true;
            await LoadData(new LoadDataArgs());
            await PopulateLookups();
        }

        _accountingOnlyColumns =
            Layout.CurrentUser.Privileges.Any(z => z.Privilege == SystemPrivileges.PrimeTcrsAccountingColumns);

        _accountingOnlyFilters =
            Layout.CurrentUser.Privileges.Any(z => z.Privilege == SystemPrivileges.PrimeTcrsAccountingFilters);
    }

    private async Task PopulateLookups()
    {
        if (_lookupLoading)
        {
            return;
        }

        Layout.ShowhideSpinner(true);
        _lookupLoading = true;
        _countries = await LookUpsService.GetCountriesAsync();
        _yesNoOptions = await LookUpsService.GetYesNoList();
        _salesClasses = await LookUpsService.GetSalesPersonClasses(false);
        _developers = await LookUpsService.GetDevelopersAsync();
        _verificationStatuses = await LookUpsService.GetVerificationStatusesAsync();
        _companies = await LookUpsService.GetCompaniesAsync();
        _knowItems = (await LookUpsService.GetKnowItemsAsync()).ToArray();
        _leadSources = (await LookUpsService.GetLeadSourcesAsync()).ToArray();
        _allUsages = await LookUpsService.GetUsageAsync();

        _usages = _allUsages.Where(z => z.ExtraId == "Prime")
            .ToArray();

        _allAgentItems = await LookUpsService.GetAgentsAsync(false);
        _primeTcrStatuses = await LookUpsService.GetPrimeTcrStatusesAsync();
        _agencies = await LookUpsService.GetMarketingAgenciesAsync();
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

    private async Task LoadData(LoadDataArgs? args)
    {
        _args = args;

        if (_isBusy)
        {
            return;
        }

        _isBusy = true;
        Layout.ShowhideSpinner(true);

        if (args.Top is > 0)
        {
            _filters.PageSize = args.Top.Value;
        }

        _filters.PageIndex = (args.Skip ?? 0) / _filters.PageSize;

        if (string.IsNullOrEmpty(args.OrderBy) == false && args.OrderBy.Length > 0 &&
           args.OrderBy.IndexOf(" ", StringComparison.Ordinal) > 0 && args.OrderBy.Split(" ")
               .Length == 2)
        {
            var arr = args.OrderBy.Split(" ");
            _filters.SortField = arr[0];
            _filters.SortDirection = arr[1];
            _filters.ExtraFilters = args.Filter;
        }

        if (Mode != null && Mode > 0)
        {
            _filters.ModeId = Mode.Value;
        }

        ;
        var retObj = await PrimeTcrsService.GetPrimeTcrsAsync(_filters);
        _totalNumber = retObj.TotalCount;

        foreach (var item in retObj.Items)
        {
            item.StatusBadgeStyle =
                (BadgeStyle)Enum.Parse(typeof(BadgeStyle), item.PrimeTcrStatusBackgroundColor!, true);
        }

        _primeTcrs = retObj.Items;
        _isBusy = false;
        Layout.ShowhideSpinner(_lookupLoading);
    }

    private void KnowSourceChanged()
    {
        var knowItem = _knowItems.FirstOrDefault(z => z.ItemId == _filters.KnowSourceId);
        _filters.KnowSubSourceId = 0;

        if (knowItem != null)
        {
            _knowSubItems = knowItem.SubLookUps.ToArray();
        }
    }

    private void ClosingKnowChanged()
    {
        var knowItem = _knowItems.FirstOrDefault(z => z.ItemId == _filters.ClosingChannelId);
        _filters.ClosingSubChannelId = 0;

        if (knowItem != null)
        {
            _closingKnowSubItems = knowItem.SubLookUps.ToArray();
        }
    }

    private void CountryChanged()
    {
        var item = _countries.FirstOrDefault(z => z.ItemId == _filters.CountryId);
        _filters.CityId = _filters.DistrictId = _filters.NeighborhoodId = 0;

        if (item != null)
        {
            _cities = item.SubLookUps;
        }
    }

    private void CityChanged()
    {
        var item = _cities.FirstOrDefault(z => z.ItemId == _filters.CityId);
        _filters.DistrictId = _filters.NeighborhoodId = 0;

        if (item != null)
        {
            _districts = item.SubLookUps;
        }
    }

    private void DistrictChanged()
    {
        var item = _districts.FirstOrDefault(z => z.ItemId == _filters.DistrictId);
        _filters.NeighborhoodId = 0;

        if (item != null)
        {
            _neighborhoods = item.SubLookUps;
        }
    }

    private void UsageChanged()
    {
        var item = _usages.FirstOrDefault(z => z.ItemId == _filters.UsageId);
        _filters.PropertyTypeId = 0;

        if (item != null)
        {
            _propertyTypes = item.SubLookUps;
        }
    }

    private void DeveloperChanged()
    {
        var item = _developers.FirstOrDefault(z => z.ItemId == _filters.DeveloperId);
        _filters.ProjectId = 0;

        if (item != null)
        {
            _projects = item.SubLookUps;
        }
    }

    private async Task ResetFilters()
    {
        _filters = new PrimeTcrsFiltersModel();
        _args.Skip = 0;
        _args.Filter = null;
        await LoadData(_args);
    }

    private void SelectItem(bool args, PrimeTcrItemForList data)
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
            Array.ForEach(_primeTcrs, z => _selectItems.Add(z));
        }
        else
        {
            if (_selectItems.Count == _primeTcrs.Length)
            {
                _selectItems.Clear();
            }
        }
    }

    private void ShowTooltip(ElementReference elementReference, string valExtraId)
    {
        TooltipService.Open(elementReference, valExtraId);
    }

    private void OpenPrimeTcr(long id)
    {
        Layout.NavigateTo($"/prime-tcrs/view/{id}", true);
    }

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



        if (action == SystemPrivileges.PrimeTcrsExport)
        {
            await ExportItemsAsync();
            return;
        }
        if (_selectItems.Count == 0)
        {
            Layout.ShowMessage(ToastLevel.Error, "You have to select at least one item");

            return;
        }


        var title = "";

        if (action == SystemPrivileges.PrimeTcrsCanSetReviewing)
        {
            title = "Set Reviewing";
        }
        else
        {
            title = "Set Rec Contracted";
        }

        //var confirm = await DialogService.Confirm("Confirm", msg);
        //if (confirm != true)
        //    return;


        var width = "700px";
        var height = "515px";


        await DialogService.OpenAsync<PrimeTcrActionsPopUp>(title,
            new Dictionary<string, object>
            {
                { "PrimeTcrItems", _selectItems },
                { "Action", action },
                { "Title", title },


            }, new DialogOptions { Width = width, Height = height });

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



        _filters.PageIndex = 0;
        _filters.PageSize = 10000;

        if (string.IsNullOrEmpty(_grid.Query.OrderBy) == false && _grid.Query.OrderBy.Length > 0 &&
           _grid.Query.OrderBy.IndexOf(" ", StringComparison.Ordinal) > 0 && _grid.Query.OrderBy.Split(" ")
               .Length == 2)
        {
            var arr = _grid.Query.OrderBy.Split(" ");
            _filters.SortField = arr[0];
            _filters.SortDirection = arr[1];
            _filters.ExtraFilters = _grid.Query.Filter;
        }

        if (Mode != null && Mode > 0)
        {
            _filters.ModeId = Mode.Value;
        }
        _filters.ExportMode = true;
        var retObj = await PrimeTcrsService.GetPrimeTcrsAsync(_filters);


        _filters.ExportMode = false;

        var leads = retObj.Items;

        var properties = leads.First().GetType().GetProperties();

        var columns = _grid.ColumnsCollection.Where(z => string.IsNullOrEmpty(z.Title) == false && string.IsNullOrEmpty(z.Property) == false).Select(z => new ReportPropertyTitleItem { Property = z.Property, Title = z.Title }).ToList();

        columns.Add(new ReportPropertyTitleItem { Title = "Build-Up Area", Property = "BuildUpArea" });
        columns.Add(new ReportPropertyTitleItem { Title = "Converted From Referral", Property = "ConvertedFromReferral" });
        columns.Add(new ReportPropertyTitleItem { Title = "District", Property = "LeadDistrict" });
        columns.Add(new ReportPropertyTitleItem { Title = "Document Date", Property = "DocumentDate" });
        columns.Add(new ReportPropertyTitleItem { Title = "Document Type", Property = "DocumentType" });
        columns.Add(new ReportPropertyTitleItem { Title = "Last Developer Reviewing Date", Property = "LastDeveloperReviewingDate" });
        columns.Add(new ReportPropertyTitleItem { Title = "Last Resolve Date", Property = "LastResolveDate" });
        columns.Add(new ReportPropertyTitleItem { Title = "Lead Project", Property = "LeadProject" });
        columns.Add(new ReportPropertyTitleItem { Title = "Reopen reason", Property = "LastReopenReason" });
        columns.Add(new ReportPropertyTitleItem { Title = "1st Agent", Property = "FirstAgent" });
        columns.Add(new ReportPropertyTitleItem { Title = "1st Agent Id", Property = "FirstAgentId" });
        columns.Add(new ReportPropertyTitleItem { Title = "1st Agent Percentage", Property = "FirstAgentPercentage" });
        columns.Add(new ReportPropertyTitleItem { Title = "1st Agent Position", Property = "FirstAgentPosition" });
        columns.Add(new ReportPropertyTitleItem { Title = "1st Agent M1", Property = "FirstAgentManagerOne" });
        columns.Add(new ReportPropertyTitleItem { Title = "1st Agent M2", Property = "FirstAgentManagerTwo" });
        columns.Add(new ReportPropertyTitleItem { Title = "1st Agent M3", Property = "FirstAgentManagerThree" });
        columns.Add(new ReportPropertyTitleItem { Title = "1st Agent M4", Property = "FirstAgentManagerFour" });
        columns.Add(new ReportPropertyTitleItem { Title = "1st Agent M5", Property = "FirstAgentManagerFive" });

        columns.Add(new ReportPropertyTitleItem { Title = "2nd Agent", Property = "SecondAgent" });
        columns.Add(new ReportPropertyTitleItem { Title = "2nd Agent Id", Property = "SecondAgentId" });
        columns.Add(new ReportPropertyTitleItem { Title = "2nd Agent Percentage", Property = "SecondAgentPercentage" });
        columns.Add(new ReportPropertyTitleItem { Title = "2nd Agent Position", Property = "SecondAgentPosition" });
        columns.Add(new ReportPropertyTitleItem { Title = "2nd Agent M1", Property = "SecondAgentManagerOne" });
        columns.Add(new ReportPropertyTitleItem { Title = "2nd Agent M2", Property = "SecondAgentManagerTwo" });
        columns.Add(new ReportPropertyTitleItem { Title = "2nd Agent M3", Property = "SecondAgentManagerThree" });
        columns.Add(new ReportPropertyTitleItem { Title = "2nd Agent M4", Property = "SecondAgentManagerFour" });
        columns.Add(new ReportPropertyTitleItem { Title = "2nd Agent M5", Property = "SecondAgentManagerFive" });

        columns.Add(new ReportPropertyTitleItem { Title = "3rd Agent", Property = "ThirdAgent" });
        columns.Add(new ReportPropertyTitleItem { Title = "3rd Agent Id", Property = "ThirdAgentId" });
        columns.Add(new ReportPropertyTitleItem { Title = "3rd Agent Percentage", Property = "ThirdAgentPercentage" });
        columns.Add(new ReportPropertyTitleItem { Title = "3rd Agent Position", Property = "ThirdAgentPosition" });
        columns.Add(new ReportPropertyTitleItem { Title = "3rd Agent M1", Property = "ThirdAgentManagerOne" });
        columns.Add(new ReportPropertyTitleItem { Title = "3rd Agent M2", Property = "ThirdAgentManagerTwo" });
        columns.Add(new ReportPropertyTitleItem { Title = "3rd Agent M3", Property = "ThirdAgentManagerThree" });
        columns.Add(new ReportPropertyTitleItem { Title = "3rd Agent M4", Property = "ThirdAgentManagerFour" });
        columns.Add(new ReportPropertyTitleItem { Title = "3rd Agent M5", Property = "ThirdAgentManagerFive" });



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
            var sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Prime_Tcrs_Export" };
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

            var newHeaders =
                "1st Agent,1st Agent Id,1st Agent Percentage,1st Agent Position,1st Agent M1,1st Agent M2,1st Agent M3,1st Agent M4,1st Agent M5, 2nd Agent,2nd Agent Id,2nd Agent Percentage,2nd Agent Position,2nd Agent M1,2nd Agent M2,2nd Agent M3,2nd Agent M4,2nd Agent M5, 3rd Agent,3rd Agent Id,3rd Agent Percentage,3rd Agent Position,3rd Agent M1,3rd Agent M2,3rd Agent M3,3rd Agent M4,3rd Agent M5".Split(",");

            foreach (var column in newHeaders)
            {
                headerRow.Append(new Cell()
                {
                    CellValue = new CellValue(column),
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

        var fileName = $"Prime_Tcrs_Export_{DateTime.UtcNow.ToString("yyyy_MM_dd")}.xlsx";
        using var streamRef = new DotNetStreamReference(stream);
        await JSRuntime.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);



        _isBusy = false;
        Layout.ShowhideSpinner(_lookupLoading);


    }

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

    #endregion



}