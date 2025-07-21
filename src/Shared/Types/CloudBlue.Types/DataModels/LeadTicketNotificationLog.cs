namespace CloudBlue.Domain.DataModels;
public class LeadTicketNotificationLog : BaseDataModel<long>
{
    public DateTime SendingDate { get; set; }

    public int SendingStatus { get; set; }

    public string? SentMessage { get; set; }

    public long LeadTicketId { get; set; }

    public string? SendingResponse { get; set; }

    public string? SentMessageId { get; set; }

    public long AgentId { get; set; }

    public long ClientId { get; set; }


    public long SendingDateNumeric { get; set; }

    public string? RecipientPhone { get; set; }

    public string? RecipientEmail { get; set; }
    public string? SourceSystem { get; set; }

    public bool IsEmail { get; set; }
}
