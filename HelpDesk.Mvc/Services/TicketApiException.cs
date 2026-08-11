namespace HelpDesk.Mvc.Services;

/// <summary>Thrown by ITicketApiService when the HelpDesk.Api call fails
/// (network error, non-success status code, or a 404). MVC controllers
/// catch this and turn it into a friendly error/TempData message —
/// the raw HTTP/JSON details never leak to the view.</summary>
public class TicketApiException : Exception
{
    public int? StatusCode { get; }

    public TicketApiException(string message, int? statusCode = null) : base(message)
    {
        StatusCode = statusCode;
    }
}
