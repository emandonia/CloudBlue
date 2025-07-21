using CloudBlue.Domain.Enums;

namespace CloudBlue.Domain.DomainModels.Filtration.Interfaces;

public class LeadTicketsFiltersModel : SearchPager, IClientFilters, IEntityFilters, ILeadSourceFilters
{

    #region For Binding
    public int ExtremeHoursFromList { get; set; }
    public IEnumerable<int> AgentIdsList { get; set; } = new List<int>();
    public bool AgentsRecursive { get; set; }
    public int AssigningTypeId { set; get; }
    public int SalesPersonClassId { get; set; }

    #endregion


    #region Applicable Filters
    public bool LastFeedbackOnly { get; set; }
    public IEnumerable<int> FeedbackIds { get; set; } = new List<int>();

    public int ArchivedId { get; set; } = 2;
    public int PendingActivityId { get; set; }

    /// <summary>
    /// ///////
    /// </summary>
    public DateTime? EntityAssignDateFrom { get; set; }
    public DateTime? EntityAssignDateTo { get; set; }
    public IEnumerable<int> EntityStatusIds { get; set; } = new List<int>();


    public DateTime? AgentResignDateFrom { get; set; }
    public DateTime? AgentResignDateTo { get; set; }




    public DateTime? AgentLastActivityDateFrom { get; set; }
    public DateTime? AgentLastActivityDateTo { get; set; }
    public List<int> AgentsIds { get; set; } = new();
    public AssigningTypes? AssigningType { set; get; }

    public int CurrencyId { get; set; }
    public decimal BudgetFrom { get; set; }
    public decimal BudgetTo { get; set; }


    #endregion





    #region Client Filters

    public string? ClientName { get; set; }
    public string? ClientNameArabic { get; set; }

    // "(0020) 012 2121402"
    public string? ClientContactDevice { get; set; }

    public int ClientCategoryId { get; set; }
    public int InternationalOnly { get; set; }
    public string? CountryCode { get; set; }

    #endregion Client Filters

    #region Source Filters

    public int LeadSourceId { get; set; }
    public int KnowSourceId { get; set; }
    public int KnowSubSourceId { get; set; }
    public int AgencyId { get; set; }
    public string? FormName { set; get; }

    #endregion Source Filters

    #region Basic Filters

    public string? EntityIds { get; set; }
    public DateTime? EntityCreationDateFrom { get; set; }
    public DateTime? EntityCreationDateTo { get; set; }

    public int CreatedById { get; set; }
    public int ModeId { get; set; }

    public int UsageId { get; set; }
    public int PropertyTypeId { get; set; }

    public int SalesTypeId { get; set; }
    public int ServiceId { get; set; }

    public int CountryId { get; set; }
    public int CityId { get; set; }
    public int DistrictId { get; set; }
    public int NeighborhoodId { get; set; }

    public int CompanyId { get; set; }
    public int BranchId { get; set; }
    public int ExtremeHours { get; set; }
    public int TopManagerId { get; set; }
    public bool ReverseAssignDateComparison { get; set; }
    public List<int> ManagersIds { get; set; } = new();
    public int DirectManagerId { get; set; }

    #endregion Basic Filters
}