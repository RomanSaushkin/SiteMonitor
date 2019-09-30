using System;

namespace SiteMonitor.BusinessLogic.HelperClasses
{
    public class SiteStatusCheckResult
    {
        public bool IsAvailable { get; set; }
        public DateTime CheckTime { get; set; } 
    }
}
