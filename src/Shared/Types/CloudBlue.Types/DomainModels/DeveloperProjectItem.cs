namespace CloudBlue.Domain.DomainModels;
public class DeveloperProjectItem
{
    public string? ProjectName { set; get; }
    public int ProjectId { set; get; }
    public string? DeveloperName { set; get; }
    public Int128 DeveloperId { set; get; }
}
