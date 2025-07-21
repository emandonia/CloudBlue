using CloudBlue.Domain.DataModels.Crm;
using CloudBlue.Domain.DomainModels;
using CloudBlue.Domain.DomainModels.CallLeads;
using CloudBlue.Domain.DomainModels.Clients;
using CloudBlue.Domain.Interfaces.DbContext;
using CloudBlue.Domain.Interfaces.Repositories;
using CloudBlue.Domain.Utilities;
using Microsoft.EntityFrameworkCore;

namespace CloudBlue.Data.Repositories.Crm;

public class ClientsRepository(ICrmDataContext appDb) : IClientsRepository
{
    public int CurrentUserId { get; set; }
    public int CurrentUserBranchId { get; set; }
    public int CurrentUserCompanyId { get; set; }

    public long LastCreatedItemId { get; set; }

    public async Task<ClientExtendedItem?> GetClientByPhoneAsync(string deviceInfo, int companyId)
    {
        var rawClient = await appDb.Clients.FirstOrDefaultAsync(x =>
            x.ContactDevices.Any(y => y.DeviceInfo == deviceInfo));

        if (rawClient == null)
        {
            return null;
        }

        var client = new ClientExtendedItem
        {
            Id = rawClient.Id,
            ClientName = rawClient.ClientName,
            ClientNameArabic = rawClient.ClientNameArabic,
            ClientTypeId = rawClient.ClientTypeId,
            ClientCompanyName = rawClient.ClientCompanyName,
            ClientTitleId = rawClient.ClientTitleId,
            ClientBirthDate = rawClient.ClientBirthDate,
            ClientOccupation = rawClient.ClientOccupation,
            ClientWorkFieldId = rawClient.ClientOccupationFieldId,
            GenderId = rawClient.GenderId
        };

        if (rawClient.ContactDevicesJsonb != null)
        {
            var devices =
                UtilityFunctions.DeserializeJsonDocument<List<ClientPhoneItem>>(rawClient.ContactDevicesJsonb);

            if (devices.Count > 0)
            {
                client.ClientContactDevices = devices.OrderBy(z => z.DeviceType)
                    .ThenBy(z => z.IsDefault)
                    .ToList();
            }
        }

        client.LeadTickets = await appDb.VwClientLeadTickets.Where(z =>
                z.ClientId == client.Id && (companyId == 0 || z.CompanyId == companyId))
            .Select(z => new LeadTicketBriefInfoItem
            {
                CompanyId = z.CompanyId,
                AgentName = z.AgentName,
                BranchName = z.BranchName,
                CompanyName = z.CompanyName,
                ClientId = z.ClientId,
                LeadTicketId = z.LeadTicketId,
                LeadTicketStatus = z.LeadTicketStatus,
                ProspectStatus = z.ProspectStatus,
                IsFullLeadTicket = z.IsFullLeadTicket,
                BranchId = z.BranchId,
                AgentId = z.AgentId,
                CreationDateNumeric = z.CreationDateNumeric
            })
            .ToListAsync();

        return client;
    }

    public async Task<bool> CreateClient(ClientInfoModel clientInfo)
    {
        var client = new Client
        {
            ClientBirthDate = clientInfo.BirthDate,
            ClientCategoryId = 0,
            ClientCompanyName = clientInfo.ClientCompanyName,
            ClientName = clientInfo.ClientName,
            ClientNameLowered = clientInfo.ClientName.Trim().ToLower(),
            ClientNameArabic = clientInfo.ClientArabicName,
            ClientOccupation = clientInfo.Occupation,
            ClientOccupationFieldId = clientInfo.WorkFieldId,
            ClientTitleId = clientInfo.ClientTitleId,
            ClientTypeId = clientInfo.ClientTypeId,
            CompanyId = CurrentUserCompanyId,
            CreationDate = DateTime.UtcNow,
            CreationDateNumeric = int.Parse(DateTime.UtcNow.ToString("yyyyMMdd")),
            CreatedById = CurrentUserId,
            GenderId = clientInfo.GenderId,
            IsOptedOut = false,
            IsPotential = false,
            IsVip = false,
            LastEditDate = null,
            LastEditDateNumeric = 0,
            LastEditorId = 0,
            Notes = string.Empty,
            WebLeadId = 0
        };

        await appDb.Clients.AddAsync(client);
        var result = await appDb.SaveChangesAsync();
        LastCreatedItemId = client.Id;

        return result > 0;
    }

    public async Task<bool> AddClientDevicesAsync(long clientId, List<ClientPhoneModel> newDevices)
    {
        foreach (var device in newDevices)
        {
            appDb.ClientContactDevices.Add(new ClientContactDevice
            {
                ClientId = clientId,
                DeviceInfo = device.DeviceInfo.Trim().ToLower(),
                DeviceType = device.DeviceType,
                IsDefault = device.IsDefault,
                CreationDate = DateTime.UtcNow,
                CreatedById = CurrentUserId,
                WebLeadId = 0,
                CountryId = device.CountryId,
                Phone = device.Phone,
                PhoneAreaCode = device.AreaCode,
                PhoneCountryCode = device.CountryCode
            });
        }

        return await appDb.SaveChangesAsync() > 0;
    }

    public async Task<bool> SearchPhoneExistAsync(string deviceInfo, int companyId)
    {
        var clientId = await appDb.Clients.Where(x => x.ContactDevices.Any(y => y.DeviceInfo == deviceInfo))
            .Select(z => z.Id)
            .FirstOrDefaultAsync();

        return clientId > 0;
    }

    public async Task<bool> UpdateClientMissingDataAsync(ClientInfoModel clientInfo)
    {
        var client = await appDb.Clients.FirstOrDefaultAsync(x => x.Id == clientInfo.ClientId);

        if (client == null)
        {
            return false;
        }

        if (clientInfo.BirthDate != null)
        {
            client.ClientBirthDate = clientInfo.BirthDate;
        }

        if (string.IsNullOrEmpty(clientInfo.ClientArabicName) == false)
        {
            client.ClientNameArabic = clientInfo.ClientArabicName;
        }

        if (string.IsNullOrEmpty(clientInfo.Occupation) == false)
        {
            client.ClientOccupation = clientInfo.Occupation;
        }

        if (string.IsNullOrEmpty(clientInfo.ClientCompanyName) == false)
        {
            client.ClientCompanyName = clientInfo.ClientCompanyName;
        }

        if (clientInfo.GenderId > 0)
        {
            client.GenderId = clientInfo.GenderId;
        }

        if (clientInfo.ClientCategoryId > 0)
        {
            client.ClientCategoryId = clientInfo.ClientCategoryId;
        }

        if (clientInfo.WorkFieldId > 0)
        {
            client.ClientOccupationFieldId = clientInfo.WorkFieldId;
        }

        await appDb.SaveChangesAsync();

        return true;
    }

    public async Task UpdateClientDevicesAsync(long clientId)
    {
        await appDb.UpdateClientDevicesAsync(clientId);
    }
}