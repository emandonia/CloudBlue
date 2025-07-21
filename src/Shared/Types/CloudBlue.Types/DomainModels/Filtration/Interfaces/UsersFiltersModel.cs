namespace CloudBlue.Domain.DomainModels.Filtration.Interfaces;

public class UsersFiltersModel : SearchPager
{
    public string? Email { get; set; }
    public string? Fullname { get; set; }
    public string? Username { get; set; }
    public int Id { get; set; }

    public int CompanyId { get; set; }
    public int BranchId { get; set; }
    public int DepartmentId { get; set; }
    public int PositionId { get; set; }

    public int TopManagerId { get; set; }
    public int DirectManagerId { get; set; }

    public int ApprovedStatus { get; set; }
    public int LockedStatus { get; set; }
    public string? DeviceInfo { get; set; }
}