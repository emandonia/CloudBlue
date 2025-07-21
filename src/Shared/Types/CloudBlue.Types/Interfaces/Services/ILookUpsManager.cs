using CloudBlue.Domain.DomainModels;
using CloudBlue.Domain.GenericTypes;

namespace CloudBlue.Domain.Interfaces.Services;

public interface ILookUpsManager : IBaseService
{
    Task<bool> CreateMarketingAgencyAsync(CreateLookupModel createLookupModel);
    Task<bool> AddNeighborhoodAsync(LocationItem model);
    Task<bool> AddCityAsync(LocationItem model);
    Task<bool> AddDistrictAsync(LocationItem model);
}

