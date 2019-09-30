using SiteMonitor.BusinessLogic.Services.ServiceDtos;
using System.Collections.Generic;

namespace SiteMonitor.BusinessLogic.Services
{
    public interface ISiteStatusCheckService
    {
        void InitializeSiteStatusChecking();

        IEnumerable<SiteStatusCheckDto> GetSiteStatusCheckResults();
    }
}
