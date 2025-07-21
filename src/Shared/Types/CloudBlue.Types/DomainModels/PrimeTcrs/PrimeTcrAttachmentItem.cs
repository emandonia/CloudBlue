namespace CloudBlue.Domain.DomainModels.PrimeTcrs;

public class PrimeTcrAttachmentItem
{
    public long Id { get; set; }

    public DateTime AttachmentDate { get; set; }

    public string OriginalFileName { get; set; } = null!;

    public string TcrAttachmentDescription { get; set; } = null!;


    public string? UploadedBy { get; set; }
    public long PrimeTcrId { get; set; }
}