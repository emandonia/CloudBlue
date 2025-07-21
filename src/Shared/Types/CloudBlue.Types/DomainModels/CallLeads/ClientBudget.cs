using System.ComponentModel.DataAnnotations;

namespace CloudBlue.Domain.DomainModels.CallLeads;

public class ClientBudget
{
    [Range(0.0, double.MaxValue, ErrorMessage = "Invalid budget Value")]
    public decimal BudgetFrom { set; get; }

    [Range(0.0, double.MaxValue, ErrorMessage = "Invalid budget value")]
    public decimal BudgetTo { set; get; }

    public int CurrencyId { set; get; }
}