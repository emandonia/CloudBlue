using System.ComponentModel.DataAnnotations;

namespace CloudBlue.Domain.DomainModels.PrimeTcrs;

public class CreatePrimeTcrModel
{
    [Required(ErrorMessage = "Lead ticket id is required")]
    [Range(1, long.MaxValue, ErrorMessage = "Invalid Id")]

    public long LeadTicketId { get; set; }

    [Required(ErrorMessage = "Developer is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid Developer")]
    public int DeveloperId { get; set; }

    [Required(ErrorMessage = "Answer is required")]
    [Range(1, 2, ErrorMessage = "Invalid answer")]
    public int HasDocumentId { get; set; }

    [Required(ErrorMessage = "Project is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid Project")]
    public int ProjectId { get; set; }
    public string? Phase { get; set; }

    [Required(ErrorMessage = "Property Type is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid property type")]

    public int PropertyTypeId { get; set; }
    [Required(ErrorMessage = "Sales Volume is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid sales volume")]

    public int SalesVolume { get; set; }

    [Required(ErrorMessage = "Unit Number is required")]
    public string? UnitNumber { get; set; }

    public decimal LandArea { get; set; }
    public decimal BuildUpArea { get; set; }
    [Required(ErrorMessage = "Expected contract date is required")]

    public DateTime? ContractExpectedDate { get; set; }
    public string? Remarks { get; set; }

    public int OutsideBrokerId { get; set; }

    public int FirstAgentId { get; set; }

    [Required(ErrorMessage = "Percentage is required")]
    [Range(0, 100, ErrorMessage = "Invalid percentage")]

    public int FirstAgentSharePercentage { get; set; }

    public int ExtraManagerId { get; set; }

    public int SecondAgentId { get; set; }

    [Range(0, 100, ErrorMessage = "Invalid percentage")]

    public int SecondAgentSharePercentage { get; set; }

    public int ThirdAgentId { get; set; }

    [Range(0, 100, ErrorMessage = "Invalid percentage")]

    public int ThirdAgentSharePercentage { get; set; }

    public DateTime? DocumentDate { get; set; }

    public int DocumentTypeId { get; set; }

    public bool HaveDocument { get; set; }
    public bool IsRecContracted { get; set; }
}