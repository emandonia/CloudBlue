namespace CloudBlue.Domain.DomainModels.Users.UsersManagement;

public class CreateUserFullModel : CreateUserModel
{
    public string FullNameLowered { get; set; } = null!;

    public int ParentId { get; set; }

    public bool IsParent { get; set; }

    public bool IsVirtual { get; set; }


    public bool InResaleTeam { get; set; }


    public int TopMostManagerId { get; set; }

    public string? TopMostManagerName { get; set; }

    public int HireDateNumeric { get; set; }

    public DateTime? LastPromotionDate { get; set; }

    public int LastPromotionDateNumeric { get; set; }



    public string PasswordSalt { get; set; } = null!;



    public int UserGroupId { get; set; }


    public string? DirectManagerName { get; set; }
    public int[] ManagersArray { get; set; } = [];
}