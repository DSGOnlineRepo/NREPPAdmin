using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NREPPAdminSite.Models
{
    public class LookupPageModel
    {
        private List<Answer> firstLookup = new List<Answer>();
        private List<Answer> secondLookup = new List<Answer>();
        private List<MaskValue> maskList = new List<MaskValue>();

        public List<Answer> FirstLookup
        {
            get { return firstLookup; }
        }

        public List<Answer> SecondLookup
        {
            get { return secondLookup; }
        }

        public List<MaskValue> MaskList
        {
            get { return maskList; }
        }

        public LookupPageModel() { }

        public LookupPageModel(IEnumerable<Answer> inFirstLookup,
            IEnumerable<Answer> inSecondLookup, IEnumerable<MaskValue> inMasks)
        {
            firstLookup = inFirstLookup.ToList<Answer>();
            //secondLookup = inSecondLookup.ToList<Answer>();
            maskList = inMasks.ToList<MaskValue>();
        }
    }
}