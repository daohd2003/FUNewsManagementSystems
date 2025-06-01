using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace MyMvcApp.Utilities
{
    public class ValidateJwtTokenAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Lấy controller và action name
            var controllerName = context.RouteData.Values["controller"]?.ToString();
            var actionName = context.RouteData.Values["action"]?.ToString();

            // Nếu là controller NewsArticle thì bỏ qua kiểm tra token
            if (string.Equals(controllerName, "NewsArticle", StringComparison.OrdinalIgnoreCase))
            {
                base.OnActionExecuting(context);
                return;
            }

            var httpContext = context.HttpContext;
            var token = JwtUtils.GetAccessTokenFromSession(httpContext);

            if (string.IsNullOrEmpty(token) || !JwtUtils.IsTokenValid(token))
            {
                // Chuyển hướng về trang Login
                context.Result = new RedirectToActionResult("Login", "Auth", null);
                return; // Không gọi base.OnActionExecuting nữa
            }

            base.OnActionExecuting(context);
        }
    }
}