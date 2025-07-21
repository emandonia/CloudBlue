namespace CloudBlue.Domain.DataModels.Lookups;

public class ConstructionDeveloper : BaseDataModel<int>
{
    public string DeveloperName { get; set; } = null!;

    public bool HalfCommission { get; set; }

    public virtual ICollection<ConstructionDeveloperProject> ConstructionDeveloperProjects { get; set; } =
        new List<ConstructionDeveloperProject>();
}