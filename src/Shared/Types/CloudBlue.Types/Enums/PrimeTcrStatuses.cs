namespace CloudBlue.Domain.Enums;

public enum PrimeTcrStatuses
{
    Reserved = 2,
    Contracted = 4,

    ConfirmedReserved = 7,
    ConfirmedContracted = 8,
    ReviewingByDeveloper = 9,
    CanceledByDeveloper = 10,
    Postponed = 11,
    ReopenDev = 12,
    Conflict = 13,
    Resolved = 14,
    Deleted = 15,
    ReopenSales = 16,

    //     PendingPermits=16

    HalfConfirmedContracted = 17
}