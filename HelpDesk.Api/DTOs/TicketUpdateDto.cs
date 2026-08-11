using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Api.DTOs;

/// <summary>Payload accepted by PUT /api/Ticket/{id}. Allows updating
/// Title, Description, Priority and Status (per the assignment's Edit Ticket
/// requirements).</summary>
public class TicketUpdateDto
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

    [Required(ErrorMessage = "Status is required.")]
    [RegularExpression("^(Open|In Progress|Closed)$",
    ErrorMessage = "Status must be Open, In Progress, or Closed.")]
    public string Status { get; set; } = string.Empty;

    [Required(ErrorMessage = "RaisedBy is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "RaisedBy must be between 2 and 100 characters.")]
    public string RaisedBy { get; set; } = string.Empty;
}
