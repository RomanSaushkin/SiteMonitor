namespace SiteMonitor.BusinessLogic.Services.ServiceDtos
{
    public class AddNewSiteConfigurationDto
    {
        public string SiteUrl { get; set; }

        public int SiteStatusCheckIntervalTypeId { get; set; }

        public int SiteStatusCheckInterval { get; set; }
    }
}
