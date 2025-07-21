using CloudBlue.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CloudBlue.Domain.DomainModels;

public class PrimeTcrEntityActionModel
{
    public SystemPrivileges Action { get; set; }
    public List<long> ItemsIds { get; set; } = new();
    [Required(ErrorMessage = "Know source is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid know source")]

    public int KnowSourceId { get; set; }
    public Stream? FileStream { get; set; }
    public int KnowSubSourceId { get; set; }

    [Required(ErrorMessage = "Item is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid Item")]

    public int SelectedItemId { get; set; }

    [Required(ErrorMessage = "Comment is required")]

    public string? Comment { get; set; }
    [Required(ErrorMessage = "Input is required")]
    public string? StringValue { get; set; }
    [Required(ErrorMessage = "Value is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid Item")]
    public decimal NumericValue { get; set; }



    [Required(ErrorMessage = "Date is required")]
    public DateTime? GenericDate { get; set; }

    public bool SkipTemplate { get; set; }
    public int EventProcess { get; set; }
    public string? EventComment { get; set; }
    public string? FileName { get; set; }
}