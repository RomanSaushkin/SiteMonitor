using System.ComponentModel.DataAnnotations;

namespace SiteMonitor.Web.Models
{
    public class AddEditSiteConfigurationSiteConfigurationViewModel
    {
        public int? SiteConfigurationId { get; set; }

        [Required(ErrorMessage = "Please enter a site URL")]
        public string SiteUrl { get; set; }

        [Required(ErrorMessage = "Please select a check interval type")]
        public int? SiteStatusCheckIntervalTypeId { get; set; }

        [Required(ErrorMessage = "Please enter check interval")]
        [Range(1, 60, ErrorMessage = "Check interval can be from 1 to 60")]
        public int? SiteStatusCheckInterval { get; set; }
    }
}
