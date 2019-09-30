using System.Collections.Generic;
using System.Linq;
using SiteMonitor.BusinessLogic.HelperClasses;
using SiteMonitor.BusinessLogic.Services.ServiceDtos;
using SiteMonitor.Data.Repositories;

namespace SiteMonitor.BusinessLogic.Services
{
    public class SiteStatusCheckService : ISiteStatusCheckService
    {
        private ISiteStatusCheckScheduler _siteStatusCheckScheduler;
        private ISiteConfigurationRepository _siteConfigurationRepository;
        private ISiteStatusChecker _siteStatusChecker;

        public SiteStatusCheckService(ISiteStatusCheckScheduler siteStatusCheckScheduler,
            ISiteConfigurationRepository siteConfigurationRepository,
            ISiteStatusChecker siteStatusChecker)
        {
            _siteStatusCheckScheduler = siteStatusCheckScheduler;
            _siteConfigurationRepository = siteConfigurationRepository;
            _siteStatusChecker = siteStatusChecker;
        }        

        public IEnumerable<SiteStatusCheckDto> GetSiteStatusCheckResults()
        {            
            var checkedSiteStatusResults = _siteStatusChecker.GetCheckedSiteStatusResults();

            var siteConfigurations = _siteConfigurationRepository
                .GetSiteConfigurationByIds(checkedSiteStatusResults.Keys);

            return siteConfigurations
                .Join(
                    checkedSiteStatusResults,
                    sc => sc.Id,
                    csr => csr.Key,
                    (sc, csr) => new
                    {
                        sc.SiteUrl,
                        sc.LastUpdated,
                        csr.Value.CheckTime,
                        csr.Value.IsAvailable
                    }
                )
                .Where(s => s.CheckTime >= s.LastUpdated)
                .Select(s => new SiteStatusCheckDto
                {
                    SiteUrl = s.SiteUrl,
                    IsAvailable = s.IsAvailable,
                    LastCheckedDate = s.CheckTime
                })
                .ToList();
        }

        public void InitializeSiteStatusChecking()
        {
            var siteConfigurations = _siteConfigurationRepository.ListSiteConfigurations();

            foreach (var siteConfiguration in siteConfigurations)
            {
                _siteStatusCheckScheduler.Schedule(siteConfiguration);
            }
        }
    }
}
