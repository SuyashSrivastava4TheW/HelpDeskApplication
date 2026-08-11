namespace HelpDesk.Api.DTOs;

/// <summary>Read-only shape of a Ticket returned to API consumers.</summary>
public class TicketDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string RaisedBy { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
}
