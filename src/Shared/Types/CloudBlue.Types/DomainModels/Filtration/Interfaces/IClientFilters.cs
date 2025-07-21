namespace CloudBlue.Domain.DomainModels.Filtration.Interfaces;

public interface IClientFilters
{
    string? ClientName { set; get; }
    string? ClientNameArabic { set; get; }
    string? ClientContactDevice { set; get; }
    int ClientCategoryId { set; get; }
    int InternationalOnly { set; get; }
}