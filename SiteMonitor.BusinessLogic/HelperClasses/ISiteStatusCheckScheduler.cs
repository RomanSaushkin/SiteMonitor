using SiteMonitor.Data.Entities;

namespace SiteMonitor.BusinessLogic.HelperClasses
{
    public interface ISiteStatusCheckScheduler
    {
        void Schedule(SiteConfiguration siteConfiguration);

        void Unschedule(SiteConfiguration siteConfiguration);
    }
}
