using installments_Payment.DataAccessLayer.Context;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace installments_Payment.Filters
{
    public class AssistantOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;
            var userId = session.GetInt32("UserId");
            var role = session.GetString("UserRole");

            if (userId == null || role != "Assistant")
            {
                context.Result = new RedirectToActionResult("Login", "Account", new { area = "" });
                context.HttpContext.Items["SessionExpired"] = true;
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}