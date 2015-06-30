using System;
using System.Web.Mvc;
using System.Collections.Generic;
using NREPPAdminSite.Models;
using System.Linq;

namespace NREPPAdminSite {

    public static class NREPPHtml
    {
        public static MvcHtmlString QoRColHeader(RigorQuestion q)
        {
            var thTag = new TagBuilder("th");
            thTag.InnerHtml = q.QuestionString;

            return MvcHtmlString.Create(thTag.ToString());
        }

        public static MvcHtmlString QoRColHeader(RigorQuestion q, IDictionary<string, object> attribs)
        {
            var thTag = new TagBuilder("th");
            thTag.InnerHtml = q.QuestionString;

            foreach (string k in attribs.Keys)
                thTag.MergeAttribute(k, attribs[k].ToString());

            return MvcHtmlString.Create(thTag.ToString());
        }

        public static MvcHtmlString GroupHeader(IEnumerable<RigorQuestion> questions, string HeaderName)
        {
            var thTag = new TagBuilder("th");

            IEnumerable<RigorQuestion> someQuestions = questions.Where(x => x.QuestionGroup == HeaderName);

            thTag.MergeAttribute("colspan", someQuestions.Count().ToString());
            thTag.MergeAttribute("style", "text-align: center"); // Remove this before sending to Lisa
            thTag.InnerHtml = HeaderName;

            return MvcHtmlString.Create(thTag.ToString());
        }
    }

}