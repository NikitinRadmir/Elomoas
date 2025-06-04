using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;

namespace Elomoas.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlContent SidebarLink(this IHtmlHelper htmlHelper, string controller, string action, string text, string iconClass, string dataTab)
        {
            var routeData = htmlHelper.ViewContext.RouteData;
            var currentController = routeData.Values["controller"]?.ToString();
            var currentAction = routeData.Values["action"]?.ToString();

            var isActive = currentController == controller && currentAction == action;
            var activeClass = isActive ? "active" : "";

            var url = $"/{controller}/{action}";

            var link = $@"
            <li>
                <a href=""{url}"" class=""nav-content-bttn open-font {activeClass}"" data-tab=""{dataTab}"">
                    <i class=""{iconClass} mr-3""></i>
                    <span>{text}</span>
                </a>
            </li>";

            return new HtmlString(link);
        }
    }
}