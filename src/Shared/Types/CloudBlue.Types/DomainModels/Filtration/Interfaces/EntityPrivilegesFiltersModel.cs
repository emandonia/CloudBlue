namespace CloudBlue.Domain.DomainModels.Filtration.Interfaces;

public class EntityPrivilegesFiltersModel : SearchPager
{
    public int PrivilegeId { get; set; }
    public int PrivilegeCategoryId { get; set; }
    public int PrivilegeScopeId { get; set; }
    public int EntityTypeId { get; set; }
    public int EntityId { get; set; }
}