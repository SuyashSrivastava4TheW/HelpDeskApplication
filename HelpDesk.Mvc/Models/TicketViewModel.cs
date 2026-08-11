namespace HelpDesk.Mvc.Models;

/// <summary>
/// MVC's own shape for displaying a ticket. Deliberately not a shared
/// reference to the API's entity/DTO — MVC only knows about JSON shapes
/// returned over HTTP, never about the API's internal types.
/// </summary>
public class TicketViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string RaisedBy { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
}
