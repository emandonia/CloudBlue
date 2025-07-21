using CloudBlue.Domain.DomainModels;

namespace CloudBlue.Domain.Interfaces.Services;

public interface INotificationsService
{
    Task SendAssignedLeadTicketNotificationAsync(List<LeadTicketInfoForEmail> leads);
    Task SendResetPasswordAsync(string password, string userEmail, string userFullName, int userId, string agentMobile);
}