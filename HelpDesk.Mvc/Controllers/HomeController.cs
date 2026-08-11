using HelpDesk.Mvc.Models;
using HelpDesk.Mvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.Mvc.Controllers;

public class HomeController : Controller
{
    private readonly ITicketApiService _ticketApiService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ITicketApiService ticketApiService, ILogger<HomeController> logger)
    {
        _ticketApiService = ticketApiService;
        _logger = logger;
    }

    /// <summary>Dashboard: Total / Open / Closed ticket counts plus the most recent tickets.</summary>
    public async Task<IActionResult> Index()
    {
        try
        {
            var tickets = await _ticketApiService.GetAllTicketsAsync();

            var dashboard = new DashboardViewModel
            {
                TotalTickets = tickets.Count,
                OpenTickets = tickets.Count(t => t.Status == "Open"),
                InProgressTickets = tickets.Count(t => t.Status == "In Progress"),
                ClosedTickets = tickets.Count(t => t.Status == "Closed"),
                RecentTickets = tickets.OrderByDescending(t => t.CreatedDate).Take(5).ToList()
            };

            return View(dashboard);
        }
        catch (TicketApiException ex)
        {
            _logger.LogError(ex, "Failed to load dashboard data.");
            TempData["ErrorMessage"] = ex.Message;
            return View(new DashboardViewModel());
        }
    }

    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
    }
}
