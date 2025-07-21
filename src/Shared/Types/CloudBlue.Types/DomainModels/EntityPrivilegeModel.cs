using System.ComponentModel.DataAnnotations;

namespace CloudBlue.Domain.DomainModels;
public class EntityPrivilegeModel
{
    [Required(ErrorMessage = "Privilege is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid Privilege")]

    public int PrivilegeId { get; set; }


    [Required(ErrorMessage = "Entity is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid entity")]

    public int EntityId { get; set; }

    public string? EntityName { get; set; }


    [Required(ErrorMessage = "Scope is required")]

    [AllowedValues([-1, 1, 2, 3, 4, 5, 6], ErrorMessage = "Invalid scope")]
    public int PrivilegeScopeId { get; set; }

    [Required(ErrorMessage = "Entity type is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid entity type")]

    public int PrivilegeEntityTypeId { get; set; }
    [Required(ErrorMessage = "Privilege category is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid Privilege category")]

    public int PrivilegeCategoryId { get; set; }
}
