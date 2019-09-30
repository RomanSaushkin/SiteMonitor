using SiteMonitor.Data.Contexts;
using SiteMonitor.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace SiteMonitor.Data.Repositories
{
    public class SiteStatusCheckIntervalTypeRepository : BaseRepository, ISiteStatusCheckIntervalTypeRepository
    {
        public SiteStatusCheckIntervalTypeRepository(AppDbContext context)
            : base(context)
        {
        }

        public IEnumerable<SiteStatusCheckIntervalType> ListSiteStatusCheckIntervalTypes()
        {
            return _context.SiteStatusCheckIntervalTypes.ToList();
        }
    }
}
