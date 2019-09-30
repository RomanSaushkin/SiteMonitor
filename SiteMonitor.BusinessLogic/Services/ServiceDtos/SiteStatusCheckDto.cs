using System;

namespace SiteMonitor.BusinessLogic.Services.ServiceDtos
{
    public class SiteStatusCheckDto
    {
        public string SiteUrl { get; set; }

        public bool IsAvailable { get; set; }

        public DateTime LastCheckedDate { get; set; }
    }
}
