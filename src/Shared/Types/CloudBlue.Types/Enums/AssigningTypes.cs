namespace CloudBlue.Domain.Enums;

public enum AssigningTypes
{
    FreshFirstAgent = 1,
    AllFreshLeads = 6,
    FreshReAssigned = 2,
    ReAssigned = 3,
    ReassignedOnce = 4,
    QualifiedOnce = 5,
    QualifiedLeadsExceedTwoWeeks = 10,
    AssignedLeadsExceedTwoHours = 7,
    NoAnswerLeadsExceedTwoHours = 8,
    CallLaterLeadsExceedFollowUpDate = 9,

}

public enum PendingActivities
{
    PendingCallLater = 1,
    PendingReminders = 2,
    PendingAssignedLeadTicketView = 3,
    PendingAlreadyExistView = 4
}