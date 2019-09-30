namespace SiteMonitor.BusinessLogic.HelperClasses
{
    public interface ISiteStatusCheckIntervalConverter
    {
        int ConvertToMilliseconds(int siteStatusCheckInterval, int siteStatusCheckIntervalTypeId);
    }
}
