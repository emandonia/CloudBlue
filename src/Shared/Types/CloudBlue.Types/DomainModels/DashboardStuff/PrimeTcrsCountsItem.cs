using CloudBlue.Domain.DataModels;

namespace CloudBlue.Domain.DomainModels.DashboardStuff;
public class PrimeTcrsCountsItem : BaseDataModel<int>
{

    public long ContractedDealsCount { set; get; }
    public long ReservedDealsForOneMonthCount { set; get; }
    public long ReOpenedDealsCount { set; get; }

}
