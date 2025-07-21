using CloudBlue.Domain.Interfaces.Services;

namespace CloudBlue.BusinessServices.App;

public class LoggingServiceDecorator(IService innerService, ILoggerService logger) : IService
{
    public async Task<T> ExecuteAsync<T>(string operationName, object request)
    {
        logger.LogRequest(operationName, request);
        var response = await innerService.ExecuteAsync<T>(operationName, request);
        logger.LogResponse(operationName, response);

        return response;
    }
}