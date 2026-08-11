using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Api.DTOs;

/// <summary>Payload accepted by POST /api/Ticket. Status is not accepted here —
/// the service always creates new tickets as "Open", matching the assignment's
/// MVC requirement that Status is hardcoded to Open on create.</summary>
public class TicketCreateDto
{
    [Required(ErrorMessage = "Title is required.")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 200 characters.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required.")]
    [StringLength(2000, MinimumLength = 5, ErrorMessage = "Description must be between 5 and 2000 characters.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Priority is required.")]
    [RegularExpression("^(Low|Medium|High)$",
    ErrorMessage = "Priority must be Low, Medium, or High.")]
    public string Priority { get; set; } = string.Empty;

    [Required(ErrorMessage = "RaisedBy is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "RaisedBy must be between 2 and 100 characters.")]
    public string RaisedBy { get; set; } = string.Empty;
}
