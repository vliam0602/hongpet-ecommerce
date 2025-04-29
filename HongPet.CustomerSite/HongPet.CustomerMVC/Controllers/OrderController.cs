using Microsoft.AspNetCore.Mvc;

public class OrderController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
    public IActionResult OrderConfirm()
    {
        return View();
    }
}
