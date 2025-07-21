namespace CloudBlue.Domain.Interfaces.Services;

public interface IService
{
    Task<T> ExecuteAsync<T>(string operationName, object request);
}