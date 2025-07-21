namespace CloudBlue.Domain.DataModels.CbUsers;

public class UserSession : BaseDataModel<long>
{
    public DateTime CreationDate { get; set; }

    public string? DeviceServiceId { get; set; }

    public DateTime ExpireDate { get; set; }

    public long ExpireDateNumeric { get; set; }

    public string ApiKey { get; set; } = null!;

    public DateTime? SetExpiredOn { get; set; }

    public string SerializedObject { get; set; } = null!;

    public int UserId { get; set; }

    public string? LoginProvider { get; set; }

    public int BranchId { get; set; }

    public int CompanyId { get; set; }

    public bool IsExpired { get; set; }
}