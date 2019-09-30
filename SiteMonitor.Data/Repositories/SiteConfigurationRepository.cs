using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SiteMonitor.Data.Contexts;
using SiteMonitor.Data.Entities;

namespace SiteMonitor.Data.Repositories
{
    public class SiteConfigurationRepository : BaseRepository, ISiteConfigurationRepository
    {
        public SiteConfigurationRepository(AppDbContext context)
            : base(context)
        {
        }

        public void AddNewSiteConfiguration(SiteConfiguration siteConfiguration)
        {
            _context
                .SiteConfigurations
                .Add(siteConfiguration);
        }

        public void DeleteSiteConfiguration(SiteConfiguration siteConfiguration)
        {
            _context
                .SiteConfigurations
                .Remove(siteConfiguration);
        }

        public void EditSiteConfiguration(SiteConfiguration siteConfiguration)
        {
            _context
                .SiteConfigurations
                .Update(siteConfiguration);
        }

        public SiteConfiguration GetSiteConfigurationById(int siteConfigurationId)
        {
            return _context
                .SiteConfigurations
                .FirstOrDefault(sc => sc.Id == siteConfigurationId);
        }

        public IEnumerable<SiteConfiguration> GetSiteConfigurationByIds(IEnumerable<int> siteConfigurationIds)
        {
            if (!siteConfigurationIds.Any())
            {
                return new List<SiteConfiguration>();
            }

            return _context
                .SiteConfigurations
                .AsQueryable()
                .Where(sc => siteConfigurationIds.Contains(sc.Id))
                .ToList();
        }

        public IEnumerable<SiteConfiguration> ListSiteConfigurations()
        {
            return _context
                .SiteConfigurations
                .Include(sc => sc.SiteStatusCheckIntervalType)
                .ToList();
        }
    }
}
