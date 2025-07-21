namespace CloudBlue.Domain.DomainModels;

public class EntityActionResult
{
    public long ItemId { set; get; }
    public bool ActionResult { set; get; }
    public string? Message { set; get; }
}