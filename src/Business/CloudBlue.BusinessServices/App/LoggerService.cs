using CloudBlue.Domain.Interfaces.Services;
using System.Text.Json;

namespace CloudBlue.BusinessServices.App;

public class LoggerService : ILoggerService
{
    public void Log(string message)
    {
        Console.WriteLine($"{DateTime.UtcNow}: {message}");
    }

    public void LogRequest(string methodName, object request)
    {
        Log($"Request to {methodName}: {JsonSerializer.Serialize(request)}");
    }

    public void LogResponse(string methodName, object response)
    {
        Log($"Response from {methodName}: {JsonSerializer.Serialize(response)}");
    }
}