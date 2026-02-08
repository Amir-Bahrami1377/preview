namespace FormerUrban_Afta.Controllers;

public class ErrorController : Controller
{
    public IActionResult Error400(string? actionId = "")
    {
        ViewBag.actionId = actionId;
        return View();
    }

    public IActionResult Error401(string? actionId = "")
    {
        ViewBag.actionId = actionId;
        return View();
    }

    public IActionResult Error403(string? actionId = "")
    {
        ViewBag.actionId = actionId;
        return View();
    }

    public IActionResult Error404(string? actionId = "")
    {
        ViewBag.actionId = actionId;
        return View();
    }

    public IActionResult Error429(string? actionId = "")
    {
        ViewBag.actionId = actionId;
        return View();
    }

    public IActionResult Error500(string? actionId = "")
    {
        ViewBag.actionId = actionId;
        return View();
    }

    public IActionResult Generic(int code, string? actionId = "")
    {
        ViewBag.Code = code;
        ViewBag.actionId = actionId;
        return View("Generic");
    }
}

