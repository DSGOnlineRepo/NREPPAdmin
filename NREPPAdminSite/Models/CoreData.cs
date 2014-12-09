using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NREPPAdminSite.Models
{
    public class Intervention
    {
        private int id;
        public string Title { get; set; }
        public string FullDescription { get; set; }
        public string Submitter { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime SubmittedDate { get; set; }

        public int Id
        {
            get { return id; }
        }

        public Intervention()
        {
            id = -1;
            Title = "";
            FullDescription = "";
            Submitter = "";
            SubmittedDate = DateTime.Now;
            UpdatedDate = DateTime.Now;
        }

        public Intervention(int inId, string title, string fullDescription, string submitter, DateTime submitDate, DateTime updateDate)
        {
            id = inId;
            Title = title;
            FullDescription = fullDescription;
            Submitter = submitter;
            SubmittedDate = submitDate;

        }

        // private int submitterId; Is this a thing?
    }
}