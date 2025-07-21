namespace CloudBlue.Domain.Enums;

public enum UserStatuses
{
    Active = 1,

    //pending action to activate
    Inactive = 2,

    // pending admin action to activate
    Suspended = 3,

    // multiple failed logins
    Blocked = 4,

    //soft deleted by admin or account Admin
    Deleted = 5
}