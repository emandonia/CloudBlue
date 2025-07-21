using System.Text.Json;

namespace CloudBlue.Domain.DomainModels;
public class LeadTicketInfoForEmail
{
    public string? ClientName { get; set; }
    public string? ClientPhone { get; set; }
    public string? ServiceName { get; set; }
    public string? PropertyTypeName { get; set; }
    public int AgentId { get; set; }
    public int PropertyTypeId { get; set; }
    public int ServiceId { get; set; }
    public JsonDocument? ContactDevicesJsonb { get; set; }
    public long ClientId { get; set; }
    public long Id { get; set; }
    public string? AgentMobile { get; set; }
    public string? AgentName { get; set; }
    public string? District { get; set; }
}
