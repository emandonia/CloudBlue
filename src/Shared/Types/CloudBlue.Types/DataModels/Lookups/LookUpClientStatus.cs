namespace CloudBlue.Domain.DataModels.Lookups;

public sealed class LookUpClientStatus : BaseDataModel<int>
{
    public string ClientStatus { get; set; } = null!;
}