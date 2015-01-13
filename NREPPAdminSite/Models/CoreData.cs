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

    public class MaskValue
    {
        [Display(Name = "Name")]
        public string Name { get; set; }
        public int Value { get; set; }
        public bool Selected { get; set; }

        public static IEnumerable<MaskValue> SplitMask(List<MaskValue> inList, int inMask)
        {
            List<MaskValue> outList = new List<MaskValue>();

            int principal = inMask;
            int powerCounter = 0;

            /*while (principal > 0)
            {
                /*if (principal % 2 > 0)
                {
                    outList.Add((int)Math.Pow(2, powerCounter));
                }

                outList.Add(new MaskValue() { Name = inList[powerCounter].Name, Value = inList[powerCounter].Value, Selected = (principal % 2 > 0) });

                if (principal == 1)
                    break;

                principal = principal >> 1;
                powerCounter++;
                //powerCounter += 1;
            }*/

            for (int i = 0; i < inList.Count; i++ )
            {
                int someOtherVal = (int)Math.Pow(2, inList[i].Value);
                bool someValue = (principal & someOtherVal) > 0;

                outList.Add(new MaskValue()
                {
                    Name = inList[i].Name,
                    Value = inList[i].Value,
                    Selected =
                        someValue
                });
            }

                return outList;
        }
    }

}