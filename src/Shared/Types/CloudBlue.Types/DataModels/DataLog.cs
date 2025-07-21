using CloudBlue.Domain.Enums;

namespace CloudBlue.Domain.DataModels;

public class DataLog
{
    public BusinessActions Action { set; get; }
    public long Id { get; set; }

    public int UserId { get; set; }

    public string? PageName { get; set; }

    public string? ActionStr { get; set; }

    public string? MetaData { get; set; }

    public DateTime ActionDate { get; set; }

    public long ActionDateNumeric { get; set; }

    public string? UserName { get; set; }

    public bool IsNew { get; set; }

    public bool Impersonated { get; set; }

    public int OriginalUserId { get; set; }
}