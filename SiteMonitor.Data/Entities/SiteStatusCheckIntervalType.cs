using System.Collections.Generic;

namespace SiteMonitor.Data.Entities
{
    public class SiteStatusCheckIntervalType
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IList<SiteConfiguration> SiteConfigurations { get; set; } = new List<SiteConfiguration>();
    }
}
