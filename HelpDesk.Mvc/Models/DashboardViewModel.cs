namespace HelpDesk.Mvc.Models;

public class DashboardViewModel
{
    public int TotalTickets { get; set; }
    public int OpenTickets { get; set; }
    public int InProgressTickets { get; set; }
    public int ClosedTickets { get; set; }
    public List<TicketViewModel> RecentTickets { get; set; } = new();
}
