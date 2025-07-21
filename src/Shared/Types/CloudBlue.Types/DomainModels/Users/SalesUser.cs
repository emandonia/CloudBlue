namespace CloudBlue.Domain.DomainModels.Users;

public class SalesUser
{
    public decimal FlatCommissionPercentage = (decimal)0.002;
    public int UserId { set; get; }

    public decimal CommissionFlatRatio { set; get; }

    public int Level { get; set; }

    public string? AgentName { get; set; }
}