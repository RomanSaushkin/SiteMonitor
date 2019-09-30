using System;

namespace SiteMonitor.Web.Models
{
    public class SiteConfigurationViewModel
    {
        public int Id { get; set; }

        public string SiteUrl { get; set; }

        public string SiteStatusCheckIntervalTypeName { get; set; }

        public int SiteStatusCheckInterval { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
