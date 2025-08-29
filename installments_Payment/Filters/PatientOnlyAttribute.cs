using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using installments_Payment.DataAccessLayer;
using installments_Payment.DataAccessLayer.Context;

public class PatientOnlyAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var session = context.HttpContext.Session;
        var userId = session.GetInt32("UserId");
        var role = session.GetString("UserRole");

        if (userId == null || role != "Patient")
        {
            context.Result = new RedirectToActionResult("Login", "Account", new { area = "" });
            context.HttpContext.Items["SessionExpired"] = true;
            return;
        }

        base.OnActionExecuting(context);
    }
}
