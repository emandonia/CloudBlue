using CloudBlue.Domain.BaseTypes;
using CloudBlue.Domain.DataModels.Lookups;
using CloudBlue.Domain.DomainModels;
using CloudBlue.Domain.DomainModels.Users.UserSessions;
using CloudBlue.Domain.Enums;
using CloudBlue.Domain.GenericTypes;
using CloudBlue.Domain.Interfaces.DbContext;
using CloudBlue.Domain.Interfaces.Services;

namespace CloudBlue.BusinessServices.App;

public class LookUpsManager(ILookUpsDataContext db, ILookUpsService lookUpsService, LoggedInUserInfo loggedInUserInfo)
: BaseService, ILookUpsManager
{
    protected override void PopulateInitialData()
    {

    }

    protected override UserPrivilegeItem? CheckPrivilege(SystemPrivileges privilege)
    {
        return loggedInUserInfo.Privileges.FirstOrDefault(z =>
           z.Privilege == privilege && z.PrivilegeScope != PrivilegeScopes.Denied);
    }

    public async Task<bool> CreateMarketingAgencyAsync(CreateLookupModel model)
    {
        if (CheckPrivilege(SystemPrivileges.ManageLookups) == null)
        {
            LastErrors.Add(Errors.YouDoNotHavePrivilegeToDoThisAction);
            return false;

        }

        if (string.IsNullOrEmpty(model.LookupName))
        {
            LastErrors.Add(Errors.RequiredDataAreMissing);
            return false;

        }
        if (db.MarketingAgencies.Any(z => z.Agency.ToLower() == model.LookupName.ToLower()))
        {
            LastErrors.Add(Errors.ItemAlreadyExist);
            return false;
        }
        db.MarketingAgencies.Add(new MarketingAgency
        {
            Agency = model.LookupName.Trim()
        });

        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AddNeighborhoodAsync(LocationItem model)
    {
        if (CheckPrivilege(SystemPrivileges.ManageLookups) == null)
        {
            LastErrors.Add(Errors.YouDoNotHavePrivilegeToDoThisAction);
            return false;

        }
        if (string.IsNullOrEmpty(model.Neighborhood?.Trim()) || model.DistrictId <= 0)
        {
            LastErrors.Add(Errors.RequiredDataAreMissing);
            return false;

        }
        if (db.LookUpNeighborhoods.Any(z => z.Neighborhood.ToLower() == model.Neighborhood.Trim().ToLower() && z.DistrictId == model.DistrictId))
        {
            LastErrors.Add(Errors.ItemAlreadyExist);
            return false;
        }
        db.LookUpNeighborhoods.Add(new LookUpNeighborhood
        {
            Neighborhood = model.Neighborhood.Trim(),
            DistrictId = model.DistrictId,
            TypeId = model.NeighborhoodTypeId
        });

        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AddCityAsync(LocationItem model)
    {
        if (CheckPrivilege(SystemPrivileges.ManageLookups) == null)
        {
            LastErrors.Add(Errors.YouDoNotHavePrivilegeToDoThisAction);
            return false;

        }
        if (string.IsNullOrEmpty(model.City?.Trim()) || model.CountryId <= 0)
        {
            LastErrors.Add(Errors.RequiredDataAreMissing);
            return false;

        }
        if (db.LookUpCities.Any(z => z.City.ToLower() == model.City.Trim().ToLower() && z.CountryId == model.CountryId))
        {
            LastErrors.Add(Errors.ItemAlreadyExist);
            return false;
        }
        db.LookUpCities.Add(new LookUpCity
        {
            City = model.City.Trim(),
            CountryId = model.CountryId
        });

        await db.SaveChangesAsync();
        return true;

    }

    public async Task<bool> AddDistrictAsync(LocationItem model)
    {
        if (CheckPrivilege(SystemPrivileges.ManageLookups) == null)
        {
            LastErrors.Add(Errors.YouDoNotHavePrivilegeToDoThisAction);
            return false;

        }
        if (string.IsNullOrEmpty(model.District?.Trim()) || model.CityId <= 0)
        {
            LastErrors.Add(Errors.RequiredDataAreMissing);
            return false;

        }
        if (db.LookUpDistricts.Any(z => z.District.ToLower() == model.District.Trim().ToLower() && z.CityId == model.CityId))
        {
            LastErrors.Add(Errors.ItemAlreadyExist);
            return false;
        }
        db.LookUpDistricts.Add(new LookUpDistrict
        {
            District = model.District.Trim(),
            CityId = model.CityId
        });

        await db.SaveChangesAsync();
        return true;

    }
}

