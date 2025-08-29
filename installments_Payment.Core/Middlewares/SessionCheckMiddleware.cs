using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace installments_Payment.Core.Middlewares
{
    public class SessionCheckMiddleware
    {
        private readonly RequestDelegate _next;

        public SessionCheckMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower();

            if (path.Contains("/Account/Login") || path.Contains("/Account/Register") || path.Contains("/Home/Index"))
            {
                await _next(context);
                return;
            }

            // فرض می‌کنیم Session کلید "UserId" دارد
            if (string.IsNullOrEmpty(context.Session.GetString("UserId")))
            {
                // اگر سشن نیست و درخواست Ajax هست
                if (context.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    context.Response.StatusCode = 440; // 440 = Session expired (custom)
                    return;
                }

                // Redirect به صفحه لاگین
                context.Response.Redirect("/Account/Login?sessionExpired=true");
                return;
            }

            await _next(context);
        }
    }

}
