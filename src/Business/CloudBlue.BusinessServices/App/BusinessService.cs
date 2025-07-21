using CloudBlue.Domain.Interfaces.Services;

namespace CloudBlue.BusinessServices.App;

public class BusinessService : IService
{
    public async Task<T> ExecuteAsync<T>(string operationName, object request)
    {
        // Simulate business logic
        await Task.Delay(100);

        return (T)Convert.ChangeType($"Processed {operationName}", typeof(T));
    }
}