using System.Collections.Generic;

namespace SiteMonitor.BusinessLogic.HelperClasses
{
    public interface ISiteStatusChecker
    {
        void StartChecking();

        void StopChecking();

        void EnqueueSiteToCheck(SiteStatusCheckItem siteStatusCheckItem);

        void InvalidateSiteConfiguration(int siteConfigurationId);

        Dictionary<int, SiteStatusCheckResult> GetCheckedSiteStatusResults();
    }
}
