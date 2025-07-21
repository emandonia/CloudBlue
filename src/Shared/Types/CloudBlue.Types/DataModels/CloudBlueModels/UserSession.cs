using System;
using System.Collections.Generic;

namespace DataImporterLib.CloudBlueModels;

public partial class UserSession
{
    public long Id { get; set; }

    public DateTime CreationDate { get; set; }

    public string? DeviceServiceId { get; set; }

    public DateTime ExpireDate { get; set; }

    public long ExpireDateNumeric { get; set; }

    public bool IsExpired { get; set; }

    public string ApiKey { get; set; } = null!;

    public DateTime? SetExpiredOn { get; set; }

    public string SerializedObject { get; set; } = null!;

    public long UserId { get; set; }

    public string? LoginProvider { get; set; }

    public int BranchId { get; set; }

    public int CompanyId { get; set; }
}
