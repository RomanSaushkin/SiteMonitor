using System;

namespace SiteMonitor.BusinessLogic.HelperClasses
{
    public class SiteStatusCheckItem
    {
        public int SiteConfigurationId { get; set; }

        public DateTime EnqueueDateTime { get; set; }

        public SiteConfigurationState SiteConfigurationState { get; set; }
    }
}
