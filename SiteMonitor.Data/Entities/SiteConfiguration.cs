using System;

namespace SiteMonitor.Data.Entities
{
    public class SiteConfiguration
    {
        public int Id { get; set; }

        public string SiteUrl { get; set; }

        public int SiteStatusCheckIntervalTypeId { get; set; }

        public int SiteStatusCheckInterval { get; set; }

        public DateTime LastUpdated { get; set; }

        public SiteStatusCheckIntervalType SiteStatusCheckIntervalType { get; set; }
    }
}
