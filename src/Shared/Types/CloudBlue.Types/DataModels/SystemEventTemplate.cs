using CloudBlue.Domain.Enums;

namespace CloudBlue.Domain.DataModels;

public class SystemEventTemplate : BaseDataModel<int>
{
    public EventTemplates EventTemplateId { set; get; }
    public int EventProcessId { set; get; }
    public string? EventTemplate { set; get; }
    public string? TemplateName { set; get; }
    public int EventTypeId { get; set; }
    public int EntityTypeId { get; set; }
    public string? AnchorsJson { get; set; }
}