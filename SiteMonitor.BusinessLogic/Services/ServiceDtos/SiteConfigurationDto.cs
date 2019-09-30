using System;

namespace SiteMonitor.BusinessLogic.Services.ServiceDtos
{
    public class SiteConfigurationDto
    {
        public int Id { get; set; }

        public string SiteUrl { get; set; }

        public string SiteStatusCheckIntervalTypeName { get; set; }

        public int SiteStatusCheckInterval { get; set; }

        public int SiteStatusCheckIntervalTypeId { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
