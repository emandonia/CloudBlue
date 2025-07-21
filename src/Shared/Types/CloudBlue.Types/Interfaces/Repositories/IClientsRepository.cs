using CloudBlue.Domain.DomainModels;
using CloudBlue.Domain.DomainModels.CallLeads;
using CloudBlue.Domain.DomainModels.Clients;

namespace CloudBlue.Domain.Interfaces.Repositories;

public interface IClientsRepository : IBaseRepository
{
    Task<ClientExtendedItem?> GetClientByPhoneAsync(string deviceInfo, int companyId);
    Task<bool> CreateClient(ClientInfoModel clientInfo);
    Task<bool> AddClientDevicesAsync(long clientId, List<ClientPhoneModel> newDevices);
    Task<bool> SearchPhoneExistAsync(string deviceInfo, int companyId);
    Task<bool> UpdateClientMissingDataAsync(ClientInfoModel clientInfo);
    Task UpdateClientDevicesAsync(long clientId);
}