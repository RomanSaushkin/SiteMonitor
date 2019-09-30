using System;

namespace SiteMonitor.Web.Models
{
    public class SiteStatusCheckViewModel
    {
        public string SiteUrl { get; set; }

        public bool IsAvailable { get; set; }

        public DateTime LastCheckedDate { get; set; }
    }
}
