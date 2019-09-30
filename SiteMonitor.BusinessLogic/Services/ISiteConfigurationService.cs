using SiteMonitor.BusinessLogic.Services.ServiceDtos;
using System.Collections.Generic;

namespace SiteMonitor.BusinessLogic.Services
{
    public interface ISiteConfigurationService
    {
        IEnumerable<SiteConfigurationDto> GetSiteConfigurations();

        void AddNewSiteConfiguration(AddNewSiteConfigurationDto addNewSiteConfigurationData);

        void UpdateSiteConfiguration(UpdateSiteConfigurationDto updateSiteConfigurationData);

        void DeleteSiteConfiguration(int siteConfigurationId);

        SiteConfigurationDto GetSiteConfigurationDto(int siteConfigurationId);

        IEnumerable<SiteStatusCheckIntervalTypeDto> GetSiteStatusCheckIntervalTypeDatas();
    }
}
