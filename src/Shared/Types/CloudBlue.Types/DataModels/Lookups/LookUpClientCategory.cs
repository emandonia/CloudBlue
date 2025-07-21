namespace CloudBlue.Domain.DataModels.Lookups;

public sealed class LookUpClientCategory : BaseDataModel<int>
{
    public string ClientCategory { get; set; } = null!;
}