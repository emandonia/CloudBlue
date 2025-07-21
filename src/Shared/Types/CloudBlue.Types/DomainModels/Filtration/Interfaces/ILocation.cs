namespace CloudBlue.Domain.DomainModels.Filtration.Interfaces;

public interface ILocation
{
    int DistrictId { get; set; }
    int NeighborhoodId { get; set; }
    int CountryId { get; set; }
    int CityId { get; set; }
}