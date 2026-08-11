namespace HelpDesk.Api.Models;

/// <summary>
/// Represents a support ticket raised by an employee.
/// Priority must be one of: Low, Medium, High.
/// Status must be one of: Open, In Progress, Closed.
/// </summary>
public class Ticket
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Priority { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public string RaisedBy { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}
