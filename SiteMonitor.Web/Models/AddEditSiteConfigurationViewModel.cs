using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace SiteMonitor.Web.Models
{
    public class AddEditSiteConfigurationViewModel
    {
        public AddEditSiteConfigurationSiteConfigurationViewModel SiteConfiguration { get; set; }

        [BindNever]
        public IEnumerable<SiteStatusCheckIntervalTypeViewModel> SiteStatusCheckIntervalTypes { get; set; }
    }
}
