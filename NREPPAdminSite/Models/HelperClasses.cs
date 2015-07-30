using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace NREPPAdminSite.Models {

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
            MvcHtmlString answer1 = AnswerBlock(rev1, rev1.FixedAnswer != rev2.FixedAnswer);
            MvcHtmlString answer2 = AnswerBlock(rev2, rev1.FixedAnswer != rev2.FixedAnswer);

           /* if (rev1.FixedAnswer != rev2.FixedAnswer)
                theTag.MergeAttribute("style", "color: red; font-weight: bold; text-align: right; border: 1px solid black;");
            else
                theTag.MergeAttribute("style", "text-align: right; border: 1px solid;");

            theTag.InnerHtml += answer1.ToHtmlString() + "&nbsp;" + answer2.ToHtmlString();*/
            
            return MvcHtmlString.Create(answer1.ToHtmlString() + answer2.ToHtmlString());
        }

        public static MvcHtmlString AnswerBlock(QoRAnswer answer, bool inDispute)
        {
            var theTag = new TagBuilder("td");

            if (inDispute)
                theTag.MergeAttribute("style", "color: red; font-weight: bold; text-align: right; border: 1px solid black;");
            else
                theTag.MergeAttribute("style", "text-align: right; border: 1px solid;");

            theTag.InnerHtml = answer.FixedAnswer;
            if (answer.CalcAnswer != answer.FixedAnswer)
                theTag.InnerHtml += string.Format(" ({0})", answer.CalcAnswer);

            return MvcHtmlString.Create(theTag.ToString());
        }

        public static MvcHtmlString CompareBlock2(QoRAnswer rev1, QoRAnswer rev2, string formula)
        {
            var theTag = new TagBuilder("td");
            MvcHtmlString answer1 = AnswerBlock2(rev1, formula);
            MvcHtmlString answer2 = AnswerBlock2(rev2, formula);

            /* if (rev1.FixedAnswer != rev2.FixedAnswer)
                 theTag.MergeAttribute("style", "color: red; font-weight: bold; text-align: right; border: 1px solid black;");
             else
                 theTag.MergeAttribute("style", "text-align: right; border: 1px solid;");

             theTag.InnerHtml += answer1.ToHtmlString() + "&nbsp;" + answer2.ToHtmlString();*/

            return MvcHtmlString.Create(answer1.ToHtmlString() + answer2.ToHtmlString());
        }

        public static MvcHtmlString AnswerBlock2(QoRAnswer answer, string formula)
        {
            var theTag = new TagBuilder("td");

            theTag.MergeAttribute("style", "text-align: right; border: 1px solid black;");
            theTag.MergeAttribute("name", answer.IdString());
            theTag.MergeAttribute("data-formula", formula);

            theTag.InnerHtml = answer.FixedAnswer;
            if (answer.CalcAnswer != answer.FixedAnswer)
                theTag.InnerHtml += string.Format(" ({0})", answer.CalcAnswer);

            return MvcHtmlString.Create(theTag.ToString());
        }
    }

}