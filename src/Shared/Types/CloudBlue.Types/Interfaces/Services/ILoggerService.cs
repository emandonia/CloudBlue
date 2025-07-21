namespace CloudBlue.Domain.Interfaces.Services;

public interface ILoggerService
{
    void Log(string message);
    void LogRequest(string methodName, object request);
    void LogResponse(string methodName, object response);
}