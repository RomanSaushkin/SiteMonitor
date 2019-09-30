using SiteMonitor.Data.Entities;
using System.Collections.Generic;

namespace SiteMonitor.Data.Repositories
{
    public interface ISiteStatusCheckIntervalTypeRepository
    {
        IEnumerable<SiteStatusCheckIntervalType> ListSiteStatusCheckIntervalTypes();
    }
}
