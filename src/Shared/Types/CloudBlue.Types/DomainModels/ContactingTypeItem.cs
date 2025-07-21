using CloudBlue.Domain.Enums;

namespace CloudBlue.Domain.DomainModels;

public class ContactingTypeItem
{
    public int Id { set; get; }
    public ContactingTypes ContactingType { set; get; }
    public bool IsFeedBack { set; get; }
    public bool IsEssential { set; get; }
    public string ContactingTypeName { get; set; } = null!;
    public bool NeedsDate { get; set; }
}