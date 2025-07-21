namespace CloudBlue.Domain.Enums;

public enum RequestStatuses
{
	Pending = 1,
	Active = 2,
	Expired = 3,
	Void = 4,
	Reserved = 5,
	Closed = 6,
	PendingTcr = 7,
	CancelledTcr = 8
}