using System.ComponentModel.DataAnnotations;

namespace CloudBlue.Domain.DataModels;

public abstract class BaseDataModel<T>
{
    [Key]
    public T Id { get; set; } = default!;
}