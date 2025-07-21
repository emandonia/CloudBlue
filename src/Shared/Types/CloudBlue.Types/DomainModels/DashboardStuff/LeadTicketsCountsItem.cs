using CloudBlue.Domain.DataModels;

namespace CloudBlue.Domain.DomainModels.DashboardStuff;

public class LeadTicketsCountsItem : BaseDataModel<int>
{
    public long NoAnswerLeadsCount { set; get; }
    public long FreshLeadsCount { set; get; }
    public long NewLeadsExceedTwoHoursCount { set; get; }
    public long QualifiedLeadsExceedTwoWeeksCount { set; get; }
    public long CallLaterLeadsCount { set; get; }
    public long NewLeadsReassignedCount { set; get; }

}