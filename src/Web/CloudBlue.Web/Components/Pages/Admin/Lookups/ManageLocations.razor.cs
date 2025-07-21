using Blazored.Toast.Services;
using CloudBlue.Domain.DomainModels.Users.UsersManagement;
using CloudBlue.Domain.GenericTypes;
using CloudBlue.Domain.Interfaces.Services;
using CloudBlue.Web.Components.Layout;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using System.Linq.Dynamic.Core;

namespace CloudBlue.Web.Components.Pages.Admin.Lookups;

public partial class ManageLocations : ComponentBase
{

    private LoadDataArgs _args;

    private RadzenDataGrid<UserItemForList> _grid0;
    LocationItem _model = new();
    private bool _isBusy;
    private int _totalNumber;
    private IEnumerable<LookupItem<int>> _countries = [];
    private IEnumerable<LookupItem<int>> _neighborhoodTypes = [];
    private IEnumerable<LookupItem<int>> _cities = [];
    private IEnumerable<LookupItem<int>> _districts = [];
    private IEnumerable<LocationItem> _locations = [];

    [Inject]
    private ILookUpsService LookUpsService { get; set; } = null!;
    [Inject]
    private ILookUpsManager LookUpsManager { get; set; } = null!;

    [CascadingParameter]
    public MainLayout Layout { get; set; } = null!;
    private void CountryChanged()
    {
        var item = _countries.FirstOrDefault(z => z.ItemId == _model.CountryId);

        _model.CityId =
            _model.DistrictId = 0;

        if (item != null)
        {
            _cities = item.SubLookUps;
        }
        LoadData(new LoadDataArgs());
    }

    private void CityChanged()
    {
        var item = _cities.FirstOrDefault(z => z.ItemId == _model.CityId);
        _model.DistrictId = 0;

        if (item != null)
        {
            _districts = item.SubLookUps;
        }
        LoadData(new LoadDataArgs());
    }

    private void DistrictChanged()
    {
        LoadData(new LoadDataArgs());
    }

    private bool IsBusy
    {
        set
        {
            _isBusy = value;
            Layout.ShowhideSpinner(_isBusy);
        }
        get => _isBusy;
    }

    private IEnumerable<LocationItem> _items = [];
    private IEnumerable<LocationItem> _allItems = [];

    protected override async Task OnInitializedAsync()
    {
        await PopulateLookups();
        LoadData(new LoadDataArgs());

    }

    private async Task PopulateLookups()
    {
        if (IsBusy)
        {
            return;
        }

        IsBusy = true;
        _allItems = await LookUpsService.GetLocationsAsync();
        _countries = await LookUpsService.GetCountriesAsync();
        _neighborhoodTypes = await LookUpsService.GetNeighborhoodTypesAsync();
        IsBusy = false;
    }



    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await Layout.SetCurrentPageTitle("Manage Locations");
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    private void LoadData(LoadDataArgs args)
    {
        _args = args;

        if (IsBusy)
        {
            return;
        }

        IsBusy = true;
        var pageSize = 10;
        if (args.Top is > 0)
        {
            pageSize = args.Top.Value;
        }

        var pageIndex = ((args.Skip ?? 0) / pageSize);

        var query = _allItems.AsQueryable();

        if (_model.DistrictId > 0)
        {
            query = query.Where(z => z.DistrictId == _model.DistrictId);
        }
        else if (_model.CityId > 0)
        {
            query = query.Where(z => z.CityId == _model.CityId);
        }
        else if (_model.CountryId > 0)
        {
            query = query.Where(z => z.CountryId == _model.CountryId);
        }
        var orderBy = "Neighborhood Asc";

        if (string.IsNullOrEmpty(args.OrderBy) == false)
        {
            orderBy = args.OrderBy;
        }

        _totalNumber = query.Count();
        _items = query.OrderBy(orderBy).Skip(pageSize * pageIndex).Take(pageSize).ToArray();
        IsBusy = false;
    }

    private async void FormSubmit(LocationItem locationModel)
    {

    }

    private async Task AddNeighborhood()
    {
        if (IsBusy)
        {
            return;
        }

        if (_model.DistrictId == 0)
        {
            Layout.ShowMessage(ToastLevel.Error, "Please Select District");
            return;
        }
        if (string.IsNullOrEmpty(_model.Neighborhood))
        {
            Layout.ShowMessage(ToastLevel.Error, "Please Fill in Neighborhood");

            return;
        }

        IsBusy = true;
        var result = await LookUpsManager.AddNeighborhoodAsync(_model);

        if (result)
        {
            LookUpsService.RefreshMode = true;
            IsBusy = false;
            await PopulateLookups();
            LoadData(_args);
            LookUpsService.RefreshMode = false;
            Layout.ShowMessage(ToastLevel.Success, "Neighborhood has been added successfully");
        }
        else
        {
            IsBusy = false;
            Layout.ShowMessage(ToastLevel.Error, LookUpsManager.LastErrors);
        }

    }
    private async Task AddCity()
    {
        if (IsBusy)
        {
            return;
        }

        if (_model.CountryId == 0)
        {
            Layout.ShowMessage(ToastLevel.Error, "Please Select Country");
            return;
        }
        if (string.IsNullOrEmpty(_model.City))
        {
            Layout.ShowMessage(ToastLevel.Error, "Please Fill in City");

            return;
        }

        IsBusy = true;
        var result = await LookUpsManager.AddCityAsync(_model);

        if (result)
        {
            LookUpsService.RefreshMode = true;
            IsBusy = false;
            await PopulateLookups();
            LoadData(_args);
            LookUpsService.RefreshMode = false;
            Layout.ShowMessage(ToastLevel.Success, "City has been added successfully");
        }
        else
        {
            IsBusy = false;
            Layout.ShowMessage(ToastLevel.Error, LookUpsManager.LastErrors);
        }
    }
    private async Task AddDistrict()
    {
        if (IsBusy)
        {
            return;
        }

        if (_model.CityId == 0)
        {
            Layout.ShowMessage(ToastLevel.Error, "Please Select City");
            return;
        }
        if (string.IsNullOrEmpty(_model.District))
        {
            Layout.ShowMessage(ToastLevel.Error, "Please Fill in District");

            return;
        }

        IsBusy = true;
        var result = await LookUpsManager.AddDistrictAsync(_model);

        if (result)
        {
            LookUpsService.RefreshMode = true;
            IsBusy = false;
            await PopulateLookups();
            LoadData(_args);
            LookUpsService.RefreshMode = false;
            Layout.ShowMessage(ToastLevel.Success, "District has been added successfully");
        }
        else
        {
            IsBusy = false;
            Layout.ShowMessage(ToastLevel.Error, LookUpsManager.LastErrors);
        }
    }
}