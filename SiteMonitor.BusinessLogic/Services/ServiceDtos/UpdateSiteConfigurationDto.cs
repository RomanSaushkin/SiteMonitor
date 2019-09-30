namespace SiteMonitor.BusinessLogic.Services.ServiceDtos
{
    public class UpdateSiteConfigurationDto
    {
        public int Id { get; set; }

        public string SiteUrl { get; set; }

        public int SiteStatusCheckIntervalTypeId { get; set; }

        public int SiteStatusCheckInterval { get; set; }
    }
}
