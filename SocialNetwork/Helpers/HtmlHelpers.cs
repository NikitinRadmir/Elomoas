using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace Elomoas.Helpers
{
    public static class HtmlHelpers
    {
        public static IHtmlContent SidebarLink(this IHtmlHelper html, string controller, string action, string title, string icon, string badge)
        {
            var currentController = html.ViewContext.RouteData.Values["Controller"] as string;
            var currentAction = html.ViewContext.RouteData.Values["Action"] as string;
            
            var isActive = currentController?.Equals(controller, StringComparison.OrdinalIgnoreCase) == true &&
                          currentAction?.Equals(action, StringComparison.OrdinalIgnoreCase) == true;

            var activeClass = isActive ? "active" : "";
            
            var link = $@"<li><a href=""/{controller}/{action}"" class=""nav-content-bttn open-font h-auto pt-2 pb-2 {activeClass}"">
                          <i class=""font-sm feather-{icon} mr-3 text-grey-500""></i>
                          <span>{title}</span>
                          <span class=""circle-icon""></span>
                          <span class=""badge badge-primary text-white badge-pill ml-auto"" style=""display: none;"">{badge}</span>
                        </a></li>";

            return new HtmlString(link);
        }
    }
} 