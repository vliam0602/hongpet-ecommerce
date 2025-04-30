using HongPet.CustomerMVC.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HongPet.CustomerMVC.Filters;

public class AuthorizeSessionAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var session = context.HttpContext.Session;
        if (session == null || session.GetString(AppConstant.AccessToken) == null)
        {
            context.Result = new RedirectToActionResult("Login", "Auth", null);
        }
    }
}
