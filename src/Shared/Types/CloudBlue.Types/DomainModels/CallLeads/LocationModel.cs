using System.ComponentModel.DataAnnotations;

namespace CloudBlue.Domain.DomainModels.CallLeads;

public class LocationModel
{
    [Required(ErrorMessage = "District is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid district")]
    public int DistrictId { get; set; }

    [Required(ErrorMessage = "Neighborhood is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid neighborhood")]
    public int NeighborhoodId { get; set; }

    public string? NeighborhoodOther { get; set; }
    [Required(ErrorMessage = "Country is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid country")]
    public int CountryId { get; set; }

    [Required(ErrorMessage = "City is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid city")]
    public int CityId { get; set; }
    public string? ProjectName { get; set; }
}