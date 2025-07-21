using System.ComponentModel.DataAnnotations;

namespace CloudBlue.Domain.DomainModels.CallLeads;

public class LeadSourceInfoModel
{
    [Required(ErrorMessage = "Lead source is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid lead source")]
    public int LeadSourceId { set; get; }

    [Required(ErrorMessage = "Know source is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid know source")]
    public int KnowSourceId { set; get; }

    public int KnowSubSourceId { set; get; }
    public int MarketingAgencyId { get; set; }

    public string? DigitalFormName { set; get; }
    public string? LeadSourceExtra { get; set; }
}