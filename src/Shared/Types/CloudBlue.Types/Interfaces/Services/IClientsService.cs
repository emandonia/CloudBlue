using CloudBlue.Domain.DomainModels;
using CloudBlue.Domain.DomainModels.CallLeads;
using CloudBlue.Domain.Enums;

namespace CloudBlue.Domain.Interfaces.Services;

public interface IClientsService : IBaseService
{
    Task<SearchClientItem> SearchClientByPhoneAsync(ClientPhoneModel clientPhoneItem, BusinessActions action,
        CbPages page);

    Task<bool> CreateClientAsync(ClientInfoModel clientInfo);
    Task<bool> AddClientDevicesAsync(long clientId, List<ClientPhoneModel> newDevices);
    Task<bool> SearchPhoneAsync(string deviceInfo, BusinessActions action, CbPages page);
    Task<bool> UpdateClientAsync(ClientInfoModel clientInfo);
}