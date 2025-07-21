namespace CloudBlue.Domain.Enums;

public enum EventProcesses
{
    AddNewLeadTicket = 1,
    AddNewCall = 2,
    EditLeadTicket = 3,
    EditCall = 4,
    AssignCallToBranch = 5,

    AssignLeadTicketToBranch = 6,
    ReAssignCallToBranch = 7,
    ReAssignLeadTicketToBranch = 8,
    AssignLeadTicketToAgent = 9,
    ReAssignLeadTicketToAgent = 10,
    ChangeLeadTicketStatus = 11,
    ChangeCallStatus = 12,
    FollowUp = 13,
    CancelCall = 14,
    ChangeProspectStatus = 15,

    ConvertLeadTicketToProspect = 16,
    ConvertLeadTicketToRequest = 17,
    AddNewProspect = 18,
    EditProspect = 19,
    AddNewRequest = 20,
    EditRequest = 21,
    ChangeRequestStatus = 22,

    ReAssignProspectToAgent = 23,
    ReAssignRequestToAgent = 24,
    ReAssignProspectToBranch = 25,
    ReAssignRequestToBranch = 26,

    ArchiveCall = 27,
    ArchiveLeadTicket = 28,
    ArchiveProspect = 29,
    ArchiveRequest = 30,

    UnArchiveCall = 31,
    UnArchiveLeadTicket = 32,
    UnArchiveProspect = 33,
    UnArchiveRequest = 34,

    AddNewReferral = 35,
    AssignReferralToBranch = 36,
    ReAssignReferralToBranch = 37,
    EditReferral = 38,
    ChangeReferralStatus = 39,
    CancelReferral = 40,
    ArchiveReferral = 41,
    UnArchiveReferral = 42,
    AssignReferralToAgent = 43,
    CreateTcr = 44,
    UpdateTcr = 45,
    ReserveTcr = 46,
    CloseTcr = 47,
    ApproveReserveTcr = 48,
    ApproveCloseTcr = 49,
    ReopenTcr = 50,

    ConfirmReserveTcr = 51,
    ConfirmCloseTcr = 52,

    CommissionPaidToAgent = 54,
    CommissionPaidToTeamLeader = 55,
    CommissionPaidToSalesManager = 56,
    CommissionPaidToOutsideBroker = 57,

    CanceledByDeveloper = 58,
    ReviewingByDeveloper = 59,
    CancelTcr = 60,
    AddBroker = 61,
    EditBroker = 62,

    AddDeveloper = 63,
    EditDeveloper = 64,

    AddProject = 65,
    EditProject = 66,

    PostponeTcr = 67,
    ConflictTcr = 68,


    RevenueCollected = 53,
    UpdateTcrCommission = 69,
    UpdateReloadCommissions = 70,

    ResolvedByRec = 71,
    DeleteTcr = 72,
    NotifyAgentForAlreadyExist = 73,
    HandleCall = 74,
    MoveCallToCompany = 75,
    VerifyTcr = 76,
    PaymentCollected = 77,
    ChangeTcrMarketingChannel = 78,
    MoveLeadToCompany = 79,
    UpdateTcrContractingDate = 80,
    SetHalfCommissionPaid = 81,
    LinkReferral = 82,
    AddPermissionRequest = 83,
    ApprovePermissionRequest = 84,
    DenyPermissionRequest = 85,
    ReviveOldLeadTicket = 86,

    SetInvoiced = 87,
    UpdateResaleCollectedCommission = 88,
    HalfConfirmCloseTcr = 89,
    UpdatePropertyImages = 90,
    UpdateTcrProjectConfigs = 91,
    ViewLeadTicket = 92
}