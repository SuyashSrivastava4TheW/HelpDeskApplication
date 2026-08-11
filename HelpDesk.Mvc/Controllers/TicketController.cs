using HelpDesk.Mvc.Models;
using HelpDesk.Mvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.Mvc.Controllers;

public class TicketController : Controller
{
    private readonly ITicketApiService _ticketApiService;
    private readonly ILogger<TicketController> _logger;

    public TicketController(ITicketApiService ticketApiService, ILogger<TicketController> logger)
    {
        _ticketApiService = ticketApiService;
        _logger = logger;
    }

    /// <summary>View All Tickets.</summary>
    public async Task<IActionResult> Index()
    {
        try
        {
            var tickets = await _ticketApiService.GetAllTicketsAsync();
            return View(tickets);
        }
        catch (TicketApiException ex)
        {
            _logger.LogError(ex, "Failed to load ticket list.");
            TempData["ErrorMessage"] = ex.Message;
            return View(new List<TicketViewModel>());
        }
    }

    /// <summary>View Ticket Details.</summary>
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var ticket = await _ticketApiService.GetTicketByIdAsync(id);
            if (ticket is null)
            {
                TempData["ErrorMessage"] = $"Ticket #{id} was not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(ticket);
        }
        catch (TicketApiException ex)
        {
            _logger.LogError(ex, "Failed to load ticket {TicketId} details.", id);
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    /// <summary>Raise New Ticket (GET) — Status is hardcoded to Open server-side; Priority is a dropdown.</summary>
    [HttpGet]
    public IActionResult Create()
    {
        return View(new TicketCreateViewModel());
    }

    /// <summary>Raise New Ticket (POST).</summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TicketCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var created = await _ticketApiService.CreateTicketAsync(model);
            TempData["SuccessMessage"] = $"Ticket #{created.Id} \"{created.Title}\" was raised successfully.";
            return RedirectToAction(nameof(Index));
        }
        catch (TicketApiException ex)
        {
            _logger.LogError(ex, "Failed to create ticket.");
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    /// <summary>Edit Ticket (GET) — allows updating Title, Description, Priority, Status.</summary>
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var ticket = await _ticketApiService.GetTicketByIdAsync(id);
            if (ticket is null)
            {
                TempData["ErrorMessage"] = $"Ticket #{id} was not found.";
                return RedirectToAction(nameof(Index));
            }

            var model = new TicketEditViewModel
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Description = ticket.Description,
                Priority = ticket.Priority,
                Status = ticket.Status,
                RaisedBy = ticket.RaisedBy
            };

            return View(model);
        }
        catch (TicketApiException ex)
        {
            _logger.LogError(ex, "Failed to load ticket {TicketId} for edit.", id);
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    /// <summary>Edit Ticket (POST).</summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TicketEditViewModel model)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            await _ticketApiService.UpdateTicketAsync(id, model);
            TempData["SuccessMessage"] = $"Ticket #{id} was updated successfully.";
            return RedirectToAction(nameof(Index));
        }
        catch (TicketApiException ex)
        {
            _logger.LogError(ex, "Failed to update ticket {TicketId}.", id);
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    /// <summary>Delete Ticket (GET) — confirmation page.</summary>
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var ticket = await _ticketApiService.GetTicketByIdAsync(id);
            if (ticket is null)
            {
                TempData["ErrorMessage"] = $"Ticket #{id} was not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(ticket);
        }
        catch (TicketApiException ex)
        {
            _logger.LogError(ex, "Failed to load ticket {TicketId} for delete confirmation.", id);
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    /// <summary>Delete Ticket (POST) — actually deletes.</summary>
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await _ticketApiService.DeleteTicketAsync(id);
            TempData["SuccessMessage"] = $"Ticket #{id} was deleted successfully.";
        }
        catch (TicketApiException ex)
        {
            _logger.LogError(ex, "Failed to delete ticket {TicketId}.", id);
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    /// <summary>Filter Tickets by Status — dropdown posts back (GET) with the selected status.</summary>
    [HttpGet]
    public async Task<IActionResult> FilterByStatus(string? status)
    {
        var model = new TicketStatusFilterViewModel { SelectedStatus = status };

        if (!string.IsNullOrWhiteSpace(status))
        {
            try
            {
                model.Tickets = await _ticketApiService.GetTicketsByStatusAsync(status);
            }
            catch (TicketApiException ex)
            {
                _logger.LogError(ex, "Failed to filter tickets by status {Status}.", status);
                TempData["ErrorMessage"] = ex.Message;
            }
        }

        return View(model);
    }
}
