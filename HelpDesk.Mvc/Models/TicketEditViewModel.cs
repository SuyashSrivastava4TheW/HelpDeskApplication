using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HelpDesk.Mvc.Models;

public class TicketEditViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 200 characters.")]
    [Display(Name = "Title")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required.")]
    [StringLength(2000, MinimumLength = 5, ErrorMessage = "Description must be between 5 and 2000 characters.")]
    [Display(Name = "Description")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Priority is required.")]
    [Display(Name = "Priority")]
    public string Priority { get; set; } = string.Empty;

    [Required(ErrorMessage = "Status is required.")]
    [Display(Name = "Status")]
    public string Status { get; set; } = string.Empty;

    [Required(ErrorMessage = "Your name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
    [Display(Name = "Raised By")]
    public string RaisedBy { get; set; } = string.Empty;

    public List<SelectListItem> PriorityOptions { get; set; } = new()
    {
        new SelectListItem("Low", "Low"),
        new SelectListItem("Medium", "Medium"),
        new SelectListItem("High", "High")
    };

    public List<SelectListItem> StatusOptions { get; set; } = new()
    {
        new SelectListItem("Open", "Open"),
        new SelectListItem("In Progress", "In Progress"),
        new SelectListItem("Closed", "Closed")
    };
}
