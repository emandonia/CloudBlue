using System.Text.Json;

namespace CloudBlue.Domain.DataModels;

public class AuditLog : BaseDataModel<long>
{
    public string EntityTableName { set; get; } = null!;
    public string ChangeType { set; get; } = null!;
    public string UserName { set; get; } = null!;
    public JsonDocument? OldValues { set; get; }
    public JsonDocument? NewValues { set; get; }
    public long RecordId { set; get; }
    public int UserId { set; get; }
    public DateTime LogDate { set; get; }
}