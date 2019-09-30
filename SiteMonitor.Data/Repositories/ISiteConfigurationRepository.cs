using SiteMonitor.Data.Entities;
using System.Collections.Generic;

namespace SiteMonitor.Data.Repositories
{
    public interface ISiteConfigurationRepository
    {
        IEnumerable<SiteConfiguration> ListSiteConfigurations();

        SiteConfiguration GetSiteConfigurationById(int siteConfigurationId);

        IEnumerable<SiteConfiguration> GetSiteConfigurationByIds(IEnumerable<int> siteConfigurationIds);

        void AddNewSiteConfiguration(SiteConfiguration siteConfiguration);

        void EditSiteConfiguration(SiteConfiguration siteConfiguration);

        void DeleteSiteConfiguration(SiteConfiguration siteConfiguration);
    }
}
