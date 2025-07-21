using CloudBlue.Domain.BaseTypes;
using CloudBlue.Domain.DomainModels;
using CloudBlue.Domain.DomainModels.CallLeads;
using CloudBlue.Domain.DomainModels.Clients;
using CloudBlue.Domain.DomainModels.Users.UserSessions;
using CloudBlue.Domain.Enums;
using CloudBlue.Domain.Interfaces.Repositories;
using CloudBlue.Domain.Interfaces.Services;

namespace CloudBlue.BusinessServices.Crm;

public sealed class ClientsService(
    IClientsRepository repo,
    LoggedInUserInfo loggedInUserInfo,
    IDataLoggingService dataLogger) : BaseService, IClientsService
{
    public async Task<SearchClientItem> SearchClientByPhoneAsync(ClientPhoneModel clientPhoneItem,
        BusinessActions action, CbPages page)
    {
        var searchClientModel = new SearchClientItem();
        var client = await repo.GetClientByPhoneAsync(clientPhoneItem.DeviceInfo!, loggedInUserInfo.CompanyId);

        if (client == null || client.LeadTickets.Count == 0)
        {
            searchClientModel.CanAddLeadTicket = true;
            searchClientModel.Exist = false;
            searchClientModel.Message = "This client is <b>new</b>";

            return searchClientModel;
        }

        searchClientModel.Exist = true;
        ConfirmExistClient(searchClientModel, client);

        searchClientModel.ClientItem = new ClientItem
        {
            ClientCompanyName = client.ClientCompanyName,
            ClientName = client.ClientName,
            ClientNameArabic = client.ClientNameArabic,
            ClientOccupation = client.ClientOccupation,
            ClientTitleId = client.ClientTitleId,
            ClientTypeId = client.ClientTypeId,
            ClientWorkFieldId = client.ClientWorkFieldId,
            Email = client.Email,
            ClientBirthDate = client.ClientBirthDate,
            GenderId = client.GenderId,
            ClientContactDevices = client.ClientContactDevices,
            Id = client.Id
        };

        await dataLogger.AddDataLog(action, page,
            $"Search number: {clientPhoneItem.DeviceInfo} , Result: {searchClientModel.Message}");

        return searchClientModel;
    }

    public async Task<bool> CreateClientAsync(ClientInfoModel clientInfo)
    {
        var result = await repo.CreateClient(clientInfo);
        CreateItemId = repo.LastCreatedItemId;

        return result;
    }

    public async Task<bool> AddClientDevicesAsync(long clientId, List<ClientPhoneModel> newDevices)
    {
        var result = await repo.AddClientDevicesAsync(clientId, newDevices);

        if (result)
        {
            await repo.UpdateClientDevicesAsync(clientId);
        }

        return result;
    }

    public async Task<bool> SearchPhoneAsync(string deviceInfo, BusinessActions action, CbPages page)
    {
        var exist = await repo.SearchPhoneExistAsync(deviceInfo, loggedInUserInfo.CompanyId);

        return exist;
    }

    public async Task<bool> UpdateClientAsync(ClientInfoModel clientInfo)
    {
        return await repo.UpdateClientMissingDataAsync(clientInfo);
    }

    protected override UserPrivilegeItem? CheckPrivilege(SystemPrivileges privilege)
    {
        return loggedInUserInfo.Privileges.FirstOrDefault(z =>
            z.Privilege == privilege && z.PrivilegeScope != PrivilegeScopes.Denied);
    }

    protected override void PopulateInitialData()
    {
        if (repo.CurrentUserId > 0)
        {
            return;
        }

        repo.CurrentUserBranchId = loggedInUserInfo.BranchId;
        repo.CurrentUserCompanyId = loggedInUserInfo.CompanyId;
        repo.CurrentUserId = loggedInUserInfo.UserId;
    }

    private void ConfirmExistClient(SearchClientItem searchClientModel, ClientExtendedItem client)
    {
        var stringItems = new List<string>();

        var activeLeads = client.LeadTickets.Where(z =>
                z.LeadTicketStatus == LeadTicketStatuses.Assigned ||
                z.LeadTicketStatus == LeadTicketStatuses.InProgress ||
                z.LeadTicketStatus == LeadTicketStatuses.Unassigned ||
                (z.LeadTicketStatus == LeadTicketStatuses.Prospect && z.ProspectStatus != null &&
                 z.ProspectStatus != ProspectStatuses.Void && z.ProspectStatus != ProspectStatuses.Closed &&
                 z.ProspectStatus != ProspectStatuses.CanceledTcr && z.ProspectStatus != ProspectStatuses.PendingTcr))
            .ToList();

        var closedLeads = client.LeadTickets.Where(z =>
                z.ProspectStatus != null && (z.ProspectStatus == ProspectStatuses.PendingTcr ||
                                             z.ProspectStatus == ProspectStatuses.Closed))
            .ToList();

        searchClientModel.CanAddLeadTicket = true;

        if (activeLeads.Count > 0)
        {
            var items = new List<string>();
            searchClientModel.CanAddLeadTicket = false;

            foreach (var leadTicket in activeLeads.OrderByDescending(z => z.CreationDateNumeric)
                        .Take(2)
                        .ToList())
            {
                items.Add(
                    $"<b>Company: {leadTicket.CompanyName}, Branch: {leadTicket.BranchName}, Agent: {leadTicket.AgentName}</b>");
            }

            stringItems.Add(string.Format(
                "This client <b>already exists</b>, and has <b>{0} active lead tickets</b>; the most recent lead ticket(s) are with:<br/>{1}",
                activeLeads.Count, string.Join("<br/>", items)));
        }

        if (closedLeads.Count > 0)
        {
            var latestClosedLead = closedLeads.OrderByDescending(z => z.CreationDateNumeric)
                .First();

            searchClientModel.CanAddLeadTicket = false;

            stringItems.Add(string.Format(
                "This client <b>already exists</b> and have {0} closed leads; the most recent closed lead ticket is in: {1}-{2}-{3} ",
                closedLeads.Count, latestClosedLead.CompanyName, latestClosedLead.CompanyName,
                latestClosedLead.AgentName));
        }

        if (stringItems.Count == 0)
        {
            searchClientModel.Message = "This client <b>already exists</b>, and has <b>no active lead tickets</b>";
            searchClientModel.HasActiveLeads = false;
        }
        else
        {
            searchClientModel.Message = string.Join("</br>", stringItems.ToArray());
            searchClientModel.HasActiveLeads = true;
        }

        if (CheckPrivilege(SystemPrivileges.LeadTicketsByPassExistingRules) != null)
        {
            searchClientModel.CanAddLeadTicket = true;
        }
    }
}