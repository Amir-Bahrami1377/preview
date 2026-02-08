namespace FormerUrban_Afta.Attributes;

public class GlobalExceptionFilter : IExceptionFilter
{
    private readonly ILogger<GlobalExceptionFilter> _logger;


    public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        var actionId = context.ActionDescriptor.Id;
        _logger.LogCritical(context.Exception, $"خطایی رخ داده است شناسه خطا {actionId}");
        var statusCode = GetStatusCode(context.Exception);
        //context.Result = SetAction(statusCode, actionId);
        context.ExceptionHandled = true;

        var isAjax = context.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        context.Result = isAjax ? SetAjaxAction(statusCode, actionId) : SetAction(statusCode, actionId);
    }

    private int GetStatusCode(Exception exception)
    {
        return exception switch
        {
            ArgumentException => 400,
            UnauthorizedAccessException => 401,
            SecurityException => 403,
            KeyNotFoundException => 404,
            _ => 500
        };
    }

    private IActionResult SetAction(int statusCode, string? actionId)
    {
        return statusCode switch
        {
            400 => new RedirectToActionResult("Error400", "Error", new { area = "", actionId = actionId }),
            401 => new RedirectToActionResult("Error401", "Error", new { area = "", actionId = actionId }),
            403 => new RedirectToActionResult("Error403", "Error", new { area = "", actionId = actionId }),
            404 => new RedirectToActionResult("Error404", "Error", new { area = "", actionId = actionId }),
            500 => new RedirectToActionResult("Error500", "Error", new { area = "", actionId = actionId }),
            _ => new RedirectToActionResult("Generic", "Error", new { area = "", code = statusCode, actionId = actionId })
        };
    }

    private IActionResult SetAjaxAction(int statusCode, string? actionId)
    {
        return statusCode switch
        {
            400 => new StatusCodeResult(StatusCodes.Status400BadRequest),
            401 => new StatusCodeResult(StatusCodes.Status401Unauthorized),
            403 => new StatusCodeResult(StatusCodes.Status403Forbidden),
            404 => new StatusCodeResult(StatusCodes.Status404NotFound),
            500 => new StatusCodeResult(StatusCodes.Status500InternalServerError),
            _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
        };
    }
}
