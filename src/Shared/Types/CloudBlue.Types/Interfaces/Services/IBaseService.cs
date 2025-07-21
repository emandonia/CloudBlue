using CloudBlue.Domain.Enums;

namespace CloudBlue.Domain.Interfaces.Services;

public interface IBaseService
{
    List<Errors> LastErrors { set; get; }
    long CreateItemId { set; get; }
}