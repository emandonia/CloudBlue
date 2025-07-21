using CloudBlue.Domain.Enums;

namespace CloudBlue.Domain.DomainModels.CallLeads;

public interface IEntityBaseItem
{
    long Id { get; set; }
    SystemPrivileges[] AllowedActions { set; get; }

    string? BranchName { get; set; }

    DateTime CreationDate { get; set; }

    string? CreatedBy { get; set; }

    string? CompanyName { get; set; }
}