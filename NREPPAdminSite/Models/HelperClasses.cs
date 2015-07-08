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

        public static MvcHtmlString CompareBlock(QoRAnswer rev1, QoRAnswer rev2)
        {
            var theTag = new TagBuilder("td");
            MvcHtmlString answer1 = AnswerBlock(rev1);
            MvcHtmlString answer2 = AnswerBlock(rev2);

            if (rev1.FixedAnswer != rev2.FixedAnswer)
                theTag.MergeAttribute("style", "color: red; font-weight: bold;");

            theTag.InnerHtml += answer1.ToHtmlString() + "<br />" + answer2.ToHtmlString();
            
            return MvcHtmlString.Create(theTag.ToString());
        }

        public static MvcHtmlString AnswerBlock(QoRAnswer answer)
        {
            var theTag = new TagBuilder("span");
            theTag.InnerHtml = answer.FixedAnswer;
            if (answer.CalcAnswer != answer.FixedAnswer)
                theTag.InnerHtml += string.Format(" ({0})", answer.CalcAnswer);

            return MvcHtmlString.Create(theTag.ToString());
        }
    }

}