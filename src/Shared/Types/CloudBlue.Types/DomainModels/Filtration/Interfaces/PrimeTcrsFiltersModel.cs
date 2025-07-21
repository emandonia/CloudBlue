namespace CloudBlue.Domain.DomainModels.Filtration.Interfaces;

public class PrimeTcrsFiltersModel : SearchPager, IEntityFilters, ILeadSourceFilters
{
    public int ClosingChannelId { get; set; }
    public int ClosingSubChannelId { get; set; }
    public int CollectedId { get; set; }
    public int InvoicedId { get; set; }
    public int AgencyId { get; set; }
    public int UsageId { get; set; }
    public int PropertyTypeId { get; set; }
    public int DeveloperId { get; set; }
    public int ProjectId { get; set; }
    public int WasOldId { get; set; }
    public int CountryId { get; set; }
    public int DistrictId { get; set; }
    public int NeighborhoodId { get; set; }
    public int CityId { get; set; }
    public int VerificationStatusId { get; set; } = -1;
    public int IsCorporateId { get; set; }
    public int HalfCommissionCollectedStatusId { get; set; }
    public string? LeadTicketsIds { get; set; }
    public string? UnitId { get; set; }

    public bool IncludeHalfCommission { set; get; }
    public bool IncludeHalfConfirmed { set; get; }

    public int AgentId { get; set; }
    public bool AgentsRecursive { get; set; }

    public DateTime? RecContractDateFrom { get; set; }
    public DateTime? RecContractDateTo { get; set; }
    public DateTime? RecReserveDateFrom { get; set; }
    public DateTime? RecReserveDateTo { get; set; }

    public DateTime? LastResolvedDateFrom { get; set; }
    public DateTime? LastResolvedDateTo { get; set; }
    public DateTime? LastReviewedDateFrom { get; set; }
    public DateTime? LastReviewedDateTo { get; set; }

    public DateTime? ConfirmContractDateFrom { get; set; }
    public DateTime? ConfirmContractDateTo { get; set; }
    public DateTime? PostponeDateFrom { get; set; }
    public DateTime? PostponeDateTo { get; set; }
    public DateTime? LastConflictDateFrom { get; set; }
    public DateTime? LastConflictDateTo { get; set; }
    public bool UseTcrCompany { get; set; }
    public List<int> ManagersIds { get; set; } = new();
    public bool UserAgentOr { get; set; }
    public int ModeId { get; set; }
    public int CompanyId { get; set; }
    public int BranchId { get; set; }
    public string? EntityIds { get; set; }
    public IEnumerable<int> EntityStatusIds { get; set; } = [];

    public DateTime? EntityCreationDateFrom { get; set; }
    public DateTime? EntityCreationDateTo { get; set; }

    //not user filters
    public int CreatedById { get; set; }
    public int LeadSourceId { get; set; }
    public int KnowSourceId { get; set; }
    public int KnowSubSourceId { get; set; }

    //public int HalfCommissionStatusId { get; set; }
}