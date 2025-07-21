using CloudBlue.Domain.Enums;

namespace CloudBlue.Domain.Interfaces.Services;

public interface IDataLoggingService
{
    Task AddDataLog(BusinessActions action, CbPages page, string metaData);
}