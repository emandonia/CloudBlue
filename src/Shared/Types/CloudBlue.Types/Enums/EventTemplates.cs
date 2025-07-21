namespace CloudBlue.Domain.Enums;

public enum EventTemplates
{
    CallCreate = 1,
    CallAssignToBranch = 2,
    LeadTicketCreate = 3,
    LeadTicketAssignToBranch = 4,
    LeadTicketAssignToAgent = 5,
    LeadTicketReAssignToAgent = 6,
    LeadTicketReAssignToBranch = 7,
    LeadTicketMoveToCompany = 8,
    LeadTicketArchive = 9,
    LeadTicketUnArchive = 10,
    LeadTicketSetVoid = 11,
    LeadTicketReject = 12,
    PrimeTcrCreate = 13,
    PrimeTcrSetReserved = 14,
    PrimeTcrSetContracted = 15,
    PrimeTcrUpdated = 16,
    LeadTicketConvertedToPrimeTcr = 17,
    LeadTicketConvertedToResaleTcr = 18,
    PrimeTcrsCanSetReviewing = 19,
    PrimeTcrsCanSetConflict = 20,
    PrimeTcrsCanSetPostpone = 21,
    PrimeTcrsCanSetReopen = 22,
    PrimeTcrsCanUpdateReservationDate = 23,
    PrimeTcrsCanUpdateConfirmationDate = 24,
    PrimeTcrsCanDelete = 25,
    PrimeTcrsCanSetHalfCommissionPaid = 26,
    PrimeTcrsCanSetInvoiced = 27,
    PrimeTcrsSetConfirmedContracted = 28,

}