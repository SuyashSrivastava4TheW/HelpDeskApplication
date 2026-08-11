using System.Net;
using System.Net.Http.Json;
using HelpDesk.Mvc.Models;

namespace HelpDesk.Mvc.Services;

public class TicketApiService : ITicketApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<TicketApiService> _logger;

    public TicketApiService(HttpClient httpClient, ILogger<TicketApiService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<List<TicketViewModel>> GetAllTicketsAsync()
    {
        var response = await _httpClient.GetAsync("api/Ticket/All");
        await EnsureSuccess(response, "load tickets");

        return await response.Content.ReadFromJsonAsync<List<TicketViewModel>>()
               ?? new List<TicketViewModel>();
    }

    public async Task<TicketViewModel?> GetTicketByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"api/Ticket/{id}");

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        await EnsureSuccess(response, $"load ticket {id}");
        return await response.Content.ReadFromJsonAsync<TicketViewModel>();
    }

    public async Task<TicketViewModel> CreateTicketAsync(TicketCreateViewModel model)
    {
        var payload = new
        {
            model.Title,
            model.Description,
            model.Priority,
            model.RaisedBy
        };

        var response = await _httpClient.PostAsJsonAsync("api/Ticket", payload);
        await EnsureSuccess(response, "create ticket");

        return await response.Content.ReadFromJsonAsync<TicketViewModel>()
               ?? throw new TicketApiException("The API returned an empty response for a successful create.");
    }

    public async Task UpdateTicketAsync(int id, TicketEditViewModel model)
    {
        var payload = new
        {
            model.Title,
            model.Description,
            model.Priority,
            model.Status,
            model.RaisedBy
        };

        var response = await _httpClient.PutAsJsonAsync($"api/Ticket/{id}", payload);
        await EnsureSuccess(response, $"update ticket {id}");
    }

    public async Task DeleteTicketAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/Ticket/{id}");
        await EnsureSuccess(response, $"delete ticket {id}");
    }

    public async Task<List<TicketViewModel>> GetTicketsByStatusAsync(string status)
    {
        var response = await _httpClient.GetAsync($"api/Ticket/Status/{Uri.EscapeDataString(status)}");
        await EnsureSuccess(response, $"filter tickets by status '{status}'");

        return await response.Content.ReadFromJsonAsync<List<TicketViewModel>>()
               ?? new List<TicketViewModel>();
    }

    /// <summary>Central place that turns a non-success HTTP response into a
    /// TicketApiException with a readable message, so every method above
    /// stays free of repeated status-code checks.</summary>
    private async Task EnsureSuccess(HttpResponseMessage response, string actionDescription)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var body = await response.Content.ReadAsStringAsync();
        _logger.LogError(
            "HelpDesk.Api call failed while trying to {Action}. Status: {StatusCode}. Body: {Body}",
            actionDescription, (int)response.StatusCode, body);

        var message = response.StatusCode switch
        {
            HttpStatusCode.NotFound => "The requested ticket could not be found.",
            HttpStatusCode.BadRequest => "The request was rejected — please check the values you entered.",
            _ => $"Something went wrong while trying to {actionDescription}. Please try again."
        };

        throw new TicketApiException(message, (int)response.StatusCode);
    }
}
