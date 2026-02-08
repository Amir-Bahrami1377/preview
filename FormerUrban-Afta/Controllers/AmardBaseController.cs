using Microsoft.AspNetCore.Authorization;

namespace FormerUrban_Afta.Controllers
{
    [Authorize]
    [RequireHttps]

    public abstract class AmardBaseController : Controller
    {
        protected JsonResult Json(object data, string contentType = null)
        {
            return new JsonResult(data)
            {
                ContentType = contentType,
                SerializerSettings = new JsonSerializerSettings
                {
                    MaxDepth = int.MaxValue
                },
            };
        }

        private static bool ShouldHandleException(Exception exception) =>
            exception is not (StackOverflowException or OutOfMemoryException or
                AccessViolationException or AppDomainUnloadedException or ThreadAbortException or SecurityException or SEHException);

        public void OnException(ExceptionContext context)
        {
            if (context.Exception != null)
            {
                context.HttpContext.Response.Headers["X-Message-Type"] = "error";
                context.HttpContext.Response.Headers["X-Message"] = context.Exception.Message;

                context.Result = new ObjectResult(new
                {
                    error = context.Exception.Message,
                    details = context.Exception.StackTrace
                })
                {
                    StatusCode = 500 // Internal Server Error
                };
            }

            //base.OnException(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            // Example: Logging action execution time
            Debug.WriteLine($"Action {context.ActionDescriptor.DisplayName} executed at {DateTime.UtcNow.AddHours(3.5)}");

            // Example: Modifying Response Headers
            context.HttpContext.Response.Headers["X-Custom-Header"] = "Action Executed";

            base.OnActionExecuted(context);
        }
    }
}
