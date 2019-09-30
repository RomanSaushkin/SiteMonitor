namespace SiteMonitor.BusinessLogic.HelperClasses
{
    public interface ISiteAvailabilityChecker
    {
        bool GetIsSiteAvailable(string siteUrl);
    }
}
