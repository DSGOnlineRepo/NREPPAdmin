using System;
using System.Web.Mvc;

public static class DisableHtmlControlExtension
{
    public static MvcHtmlString DisableIf(this MvcHtmlString htmlString, Func<bool> expression)
    {
        if (expression.Invoke())
        {
            var html = htmlString.ToString();
            const string disabled = "\"disabled\"";
            html = html.Insert(html.IndexOf(">", 
                StringComparison.Ordinal), " disabled= " + disabled);
            return new MvcHtmlString(html);
        }
        return htmlString;
    }

    public static MvcHtmlString HideIf(this MvcHtmlString htmlString, Func<bool> expression)
    {
        if (expression.Invoke())
        {
            var html = htmlString.ToString();
            const string disabled = "\"disabled\"";
            html = html.Insert(html.IndexOf(">",
                StringComparison.Ordinal), " disabled= " + disabled);
            return new MvcHtmlString(html);
        }
        return htmlString;
    }
}