using Microsoft.AspNetCore.Mvc;

namespace HongPet.CustomerMVC.Controllers;
public class UserController : Controller
{
    public IActionResult OrderHistory()
    {
        return View();
    }
}
