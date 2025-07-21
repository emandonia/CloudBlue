namespace CloudBlue.Domain.DataModels.Lookups;

public sealed class LookUpClientType : BaseDataModel<int>
{
    public string ClientType { get; set; } = null!;
}