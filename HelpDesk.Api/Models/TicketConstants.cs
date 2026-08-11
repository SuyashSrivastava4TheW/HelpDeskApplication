namespace HelpDesk.Api.Models;

/// <summary>
/// Centralized, single source of truth for the fixed value sets the assignment
/// defines for Ticket.Priority and Ticket.Status. Keeping these in one place
/// avoids "magic string" duplication across the repository, service, validators
/// and controller.
/// </summary>
public static class TicketConstants
{
    public static readonly string[] ValidPriorities = { "Low", "Medium", "High" };

    public static readonly string[] ValidStatuses = { "Open", "In Progress", "Closed" };

    public const string DefaultStatus = "Open";

    public static bool IsValidPriority(string? priority) =>
        priority is not null && ValidPriorities.Contains(priority, StringComparer.OrdinalIgnoreCase);

    public static bool IsValidStatus(string? status) =>
        status is not null && ValidStatuses.Contains(status, StringComparer.OrdinalIgnoreCase);
}
