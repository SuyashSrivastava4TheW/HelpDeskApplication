using Microsoft.AspNetCore.Mvc.Rendering;

namespace HelpDesk.Mvc.Models;

public class ErrorViewModel
{
    public string? RequestId { get; set; }
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    public string? Message { get; set; }
}

/// <summary>Backs the "Filter Tickets by Status" view: a status dropdown plus the matching results.</summary>
public class TicketStatusFilterViewModel
{
    public string? SelectedStatus { get; set; }

    public List<SelectListItem> StatusOptions { get; set; } = new()
    {
        new SelectListItem("Open", "Open"),
        new SelectListItem("In Progress", "In Progress"),
        new SelectListItem("Closed", "Closed")
    };

    public List<TicketViewModel> Tickets { get; set; } = new();
}
