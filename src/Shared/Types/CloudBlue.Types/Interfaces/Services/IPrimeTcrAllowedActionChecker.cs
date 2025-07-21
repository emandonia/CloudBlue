using CloudBlue.Domain.DomainModels.PrimeTcrs;
using CloudBlue.Domain.Enums;

namespace CloudBlue.Domain.Interfaces.Services;

public interface IPrimeTcrAllowedActionChecker : IBaseService
{
    void PopulateAllowedActions(IEnumerable<PrimeTcrItemForList> items);
    SystemPrivileges? CanAddExtraManager(PrimeTcrItemForList item);
    SystemPrivileges? CanUpdateUnitType(PrimeTcrItemForList item);
    SystemPrivileges? CanUpdateSalesVolume(PrimeTcrItemForList item);
    SystemPrivileges? CanUpdateUnitNumber(PrimeTcrItemForList item);
    SystemPrivileges? CanSetResolved(PrimeTcrItemForList item);
    SystemPrivileges? CanUpdateConfigsAndCommissions(PrimeTcrItemForList item);
    SystemPrivileges? CanPrimeTcrsCanReloadCommissions(PrimeTcrItemForList item);
    SystemPrivileges? CanAddEvent(PrimeTcrItemForList item);
    SystemPrivileges? CanAddAttachments(PrimeTcrItemForList item);
    SystemPrivileges? CanSetReserved(PrimeTcrItemForList item);
    SystemPrivileges? CanVerify(PrimeTcrItemForList item);
    SystemPrivileges? CanChangeMarketingChannel(PrimeTcrItemForList item);
    SystemPrivileges? CanSetContracted(PrimeTcrItemForList item);
    SystemPrivileges? CanAddDocumentDate(PrimeTcrItemForList item);
    SystemPrivileges? CanSetHalfConfirmedContracted(PrimeTcrItemForList item);
    SystemPrivileges? CanUpdateHalfConfirmedContractedDate(PrimeTcrItemForList item);
    SystemPrivileges? CanSetConfirmedReserved(PrimeTcrItemForList item);
    SystemPrivileges? CanUpdateConfirmedReservedDate(PrimeTcrItemForList item);
    SystemPrivileges? CanSetCanceledByDeveloper(PrimeTcrItemForList item);


    SystemPrivileges? CanSetPostpone(PrimeTcrItemForList item);
    SystemPrivileges? CanAddPayments(PrimeTcrItemForList item);
    SystemPrivileges? CanSetInvoiced(PrimeTcrItemForList item);
    SystemPrivileges? CanSetConfirmedContracted(PrimeTcrItemForList item);
    SystemPrivileges? CanDelete(PrimeTcrItemForList item);
    SystemPrivileges? CanSetHalfCommissionPaid(PrimeTcrItemForList item);
    SystemPrivileges? CanUpdateCreationDate(PrimeTcrItemForList item);
    SystemPrivileges? CanSetReopen(PrimeTcrItemForList item);
    SystemPrivileges? CanUpdateConfirmationDate(PrimeTcrItemForList item);
    SystemPrivileges? CanSetConflict(PrimeTcrItemForList item);
    SystemPrivileges? CanSetReviewing(PrimeTcrItemForList item);
}