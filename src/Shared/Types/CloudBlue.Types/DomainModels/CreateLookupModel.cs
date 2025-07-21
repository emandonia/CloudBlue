using System.ComponentModel.DataAnnotations;

namespace CloudBlue.Domain.DomainModels;

public class CreateLookupModel
{
    [Required(ErrorMessage = "Lookup name is required")]
    [MaxLength(150, ErrorMessage = "Max length 150 characters")]
    [MinLength(2, ErrorMessage = "Min length 2 characters")]

    public string LookupName { set; get; } = null!;
}
