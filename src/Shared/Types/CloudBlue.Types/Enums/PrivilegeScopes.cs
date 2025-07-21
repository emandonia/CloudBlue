namespace CloudBlue.Domain.Enums;

public enum PrivilegeScopes
{
    Denied = -1,
    Own = 1,
    DirectTeam = 2,
    TreeTeam = 3,
    Branch = 4,
    Company = 5,
    Global = 6
}