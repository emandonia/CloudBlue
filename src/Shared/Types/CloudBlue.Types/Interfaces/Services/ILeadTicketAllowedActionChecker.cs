using CloudBlue.Domain.DomainModels.CallLeads;
using CloudBlue.Domain.Enums;

namespace CloudBlue.Domain.Interfaces.Services;

public interface ILeadTicketAllowedActionChecker : IAllowedActionChecker<LeadTicketItemForList>
{
    SystemPrivileges? CanCreateDuplicate(LeadTicketItemForList item);
    SystemPrivileges? CanConvertToPrimeTcr(LeadTicketItemForList item);
    SystemPrivileges? CanConvertToBuyerRequest(LeadTicketItemForList item);
    SystemPrivileges? CanConvertToSellerRequest(LeadTicketItemForList item);
    SystemPrivileges? CanReject(LeadTicketItemForList item);
    SystemPrivileges? CanSetVoid(LeadTicketItemForList item);
    SystemPrivileges? CanAddFeedBackEvent(LeadTicketItemForList item);
}