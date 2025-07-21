namespace CloudBlue.Domain.Enums;

public enum SystemPrivileges
{
    #region Calls

    CallsAdd = 1,
    CallsEdit = 2,
    CallsManage = 3,
    CallsExport = 4,
    CallsHandle = 5,
    CallsAssignToBranch = 6,
    CallsAssignToAgent = 7,
    CallsReAssignToBranch = 8,
    CallsCancel = 9,
    CallsViewEvents = 10,
    CallsAddEvent = 11,
    CallsNotifyAgent = 12,
    CallsMoveToCompany = 13,
    CallsArchive = 14,
    CallsUnArchive = 15,

    #endregion

    #region Lead Tickets

    LeadTicketsAdd = 41,
    LeadTicketsEdit = 42,
    LeadTicketsManage = 43,
    LeadTicketsExport = 44,
    LeadTicketsAssignToAgent = 45,
    LeadTicketsReAssignToAgent = 46,
    LeadTicketsAssignToBranch = 47,
    LeadTicketsReAssignToBranch = 48,
    LeadTicketsSetOld = 49,
    LeadTicketsViewEvents = 50,
    LeadTicketsAddEvent = 51,
    LeadTicketsMoveToCompany = 52,
    LeadTicketsArchive = 53,
    LeadTicketsUnArchive = 54,
    LeadTicketsConvertToPrimeTcr = 55,
    LeadTicketsConvertToBuyerRequest = 56,
    LeadTicketsConvertToSellerRequest = 57,
    LeadTicketsCreateDuplicate = 58, //Closed Leads With Existing Agents or Closed Leads With Resigned Agents

    LeadTicketsReject = 59, //Deactivate
    LeadTicketsSetVoid = 60,
    LeadTicketsView = 63,
    LeadTicketsByPassExistingRules = 64,

    LeadTicketsAddFeedback = 61,

    //LeadTicketsReassignAllToAgent =62,

    #endregion

    #region Listing Lookup

    AgentsList = 1001,
    BranchesList = 2001,
    CompaniesList = 2002,

    #endregion

    #region Resale Requests

    ResaleBuyerRequestsAdd = 3001,
    ResaleBuyerRequestsManage = 3002,
    ResaleSellerRequestsAdd = 3501,
    ResaleSellerRequestsManage = 3502,

    #endregion

    #region Tcrs

    ResaleTcrsAdd = 4001,
    ResaleTcrsManage = 4002,
    PrimeTcrsAdd = 5001,
    PrimeTcrsManage = 5002,
    PrimeTcrsSetContracted = 5003,
    PrimeTcrsSetReserved = 5004,
    PrimeTcrsView = 5005,
    PrimeTcrsVerify = 5006,
    PrimeTcrsChangeMarketingChannel = 5007,
    PrimeTcrsAddDocumentDate = 5008, //if null
    PrimeTcrsSetResolved = 5009,
    PrimeTcrsAddEvent = 5010,
    PrimeTcrsAddAttachments = 5011,
    PrimeTcrsAddExtraManager = 5012,
    PrimeTcrsUpdateUnitType = 5013,
    PrimeTcrsUpdateSalesVolume = 5014,
    PrimeTcrsUpdateUnitNumber = 5015,
    PrimeTcrsCanDownloadAttachments = 5016,
    PrimeTcrsExport = 5017,
    #endregion
    #region accounting Tcrs
    PrimeTcrsAccountingReports = 7001,
    PrimeTcrsAccountingFilters = 7002,
    PrimeTcrsAccountingColumns = 7003,
    PrimeTcrsCanAccessCommissionsTab = 7004,
    PrimeTcrsCanAccessConfigsTab = 7005,
    PrimeTcrsCanAccessPaymentsTab = 7006,
    PrimeTcrsCanSetReviewing = 7031,
    PrimeTcrsCanSetConflict = 7032,
    PrimeTcrsCanSetPostpone = 7033,
    PrimeTcrsCanSetReopen = 7034,
    PrimeTcrsCanUpdateCreationDate = 7035,
    PrimeTcrsCanUpdateConfirmationDate = 7036,
    PrimeTcrsCanDelete = 7037,
    PrimeTcrsCanSetHalfCommissionCollected = 7038,
    PrimeTcrsCanSetInvoiced = 7039,
    PrimeTcrsSetConfirmedContracted = 7040,
    PrimeTcrsCanUpdateConfigsAndCommissions = 7041, // company revenue percentage and outside broker
    PrimeTcrsBulkSetContracted = 7042,
    PrimeTcrsCanReloadCommissions = 7043,
    PrimeTcrsCanAddPayments = 7044,
    PrimeTcrsSetHalfConfirmedContracted = 7045,
    PrimeTcrsSetConfirmedReserved = 7046,
    PrimeTcrsUpdateConfirmedReservedDate = 7047,
    PrimeTcrsUpdateHalfConfirmedContracted = 7048,
    PrimeTcrsCanSetCanceledByDeveloper = 7049,
    //PrimeTcrsCanSet = 7050,

    #endregion

    #region System Adminstration

    ManageLookups = 11000,
    ManagePrivileges = 12000,
    ManageUsers = 10000

    #endregion

    ,

}

//CheckContact