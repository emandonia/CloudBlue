using System.ComponentModel.DataAnnotations.Schema;

namespace CloudBlue.Domain.DataModels.PrimeTcrs;


[Table("PrimeTcrAttachments")]
public partial class PrimeTcrAttachment : BaseDataModel<long>
{

    public int UploadedById { get; set; }

    public DateTime AttachmentDate { get; set; }

    public string OriginalFileName { get; set; } = null!;

    public string TcrAttachmentDescription { get; set; } = null!;

    public long PrimeTcrId { get; set; }

    public string? UploadedBy { get; set; }

    public virtual PrimeTcr PrimeTcr { get; set; } = null!;
}
