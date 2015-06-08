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

    public static MvcHtmlString ReadOnlyIf(this MvcHtmlString htmlString, Func<bool> expression)
    {
        if (expression.Invoke())
        {
            var html = htmlString.ToString();
            const string readOnly = "\"true\"";
            html = html.Insert(html.IndexOf(">",
                StringComparison.Ordinal), " readonly= " + readOnly);
            return new MvcHtmlString(html);
        }
        return htmlString;
    }
}