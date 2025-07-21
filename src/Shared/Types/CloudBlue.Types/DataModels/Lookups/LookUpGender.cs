namespace CloudBlue.Domain.DataModels.Lookups;

public sealed class LookUpGender : BaseDataModel<int>
{
    public string Gender { get; set; } = null!;
}