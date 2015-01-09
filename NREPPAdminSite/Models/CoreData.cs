using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace NREPPAdminSite.Models
{
    public class Intervention
    {
        //private int id;
        
        [Display(Name = "Title")]
        public string Title { get; set; }
        public int SubmitterId { get; set; }

        [Display(Name = "Full Description")]
        public string FullDescription { get; set; }

        [Display(Name = "Submitter")]
        public string Submitter { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime? PublishDate { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }
        public int StatusId { get; set; }

        public int Id {get; set;}
        /*{
            get { return id; }
        }*/

        public Intervention()
        {
            Id = -1;
            Title = "";
            FullDescription = "";
            Submitter = "";
            SubmitterId = 0;
            PublishDate = DateTime.Now;
            UpdatedDate = DateTime.Now;
            Status = "Submitted";
            StatusId = 1;
        }

        public Intervention(int inId, string title, string fullDescription, string submitter, DateTime? publishDate, DateTime updateDate, int submitterId, string status, int statusId)
        {
            Id = inId;
            Title = title;
            FullDescription = fullDescription;
            Submitter = submitter;
            PublishDate = publishDate;
            UpdatedDate = updateDate;
            SubmitterId = submitterId;
            Status = status;
            StatusId = statusId;

        }

        // private int submitterId; Is this a thing?
    }

    public class InterventionDoc
    {
        [Display(Name = "Description")]
        public string FileDescription { get; set; }
        public string Link { get; set; }

        [Display(Name = "Uploaded By")]
        public string Uploader { get; set; }

    }

    public class Answer
    {
        public int AnswerId { get; set; }
        public string LongAnswer { get; set; }
        public string ShortAnswer { get; set; }

        public Answer(int Id, string Long, string Short)
        {
            AnswerId = Id;
            LongAnswer = Long;
            ShortAnswer = Short;
        }

        public Answer()
        {
            AnswerId = -1;
        }
    }
}