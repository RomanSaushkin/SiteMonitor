using SiteMonitor.BusinessLogic.HelperClasses;
using SiteMonitor.BusinessLogic.Services.ServiceDtos;
using SiteMonitor.Data.Entities;
using SiteMonitor.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SiteMonitor.BusinessLogic.Services
{
    public class SiteConfigurationService : ISiteConfigurationService
    {
        private ISiteStatusCheckScheduler _siteStatusCheckScheduler;
        private ISiteConfigurationRepository _siteConfigurationRepository;
        private ISiteStatusCheckIntervalTypeRepository _siteStatusCheckIntervalTypeRepository;
        private IUnitOfWork _unitOfWork;

        public SiteConfigurationService(ISiteStatusCheckScheduler siteStatusCheckScheduler,
            ISiteConfigurationRepository siteConfigurationRepository,
            ISiteStatusCheckIntervalTypeRepository siteStatusCheckIntervalTypeRepository,
            IUnitOfWork unitOfWork)
        {
            _siteStatusCheckScheduler = siteStatusCheckScheduler;
            _siteConfigurationRepository = siteConfigurationRepository;
            _siteStatusCheckIntervalTypeRepository = siteStatusCheckIntervalTypeRepository;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<SiteConfigurationDto> GetSiteConfigurations()
        {
            return _siteConfigurationRepository
                .ListSiteConfigurations()
                .Select(sc =>
                    new SiteConfigurationDto
                    {
                        Id = sc.Id,
                        LastUpdated = sc.LastUpdated,
                        SiteStatusCheckInterval = sc.SiteStatusCheckInterval,
                        SiteStatusCheckIntervalTypeName = sc.SiteStatusCheckIntervalType.Name,
                        SiteUrl = sc.SiteUrl
                    })
                .ToList();
        }

        public void AddNewSiteConfiguration(AddNewSiteConfigurationDto addNewSiteConfigurationData)
        {
            var siteConfiguration = new SiteConfiguration
            {
                SiteUrl = addNewSiteConfigurationData.SiteUrl,
                SiteStatusCheckIntervalTypeId = addNewSiteConfigurationData.SiteStatusCheckIntervalTypeId,
                SiteStatusCheckInterval = addNewSiteConfigurationData.SiteStatusCheckInterval,
                LastUpdated = DateTime.UtcNow
            };

            _siteConfigurationRepository
                .AddNewSiteConfiguration(siteConfiguration);

            _unitOfWork.Complete();

            _siteStatusCheckScheduler.Schedule(siteConfiguration);
        }

        public void DeleteSiteConfiguration(int siteConfigurationId)
        {
            var siteConfiguration = _siteConfigurationRepository
                .GetSiteConfigurationById(siteConfigurationId);

            if (siteConfiguration != null)
            {
                _siteConfigurationRepository
                    .DeleteSiteConfiguration(siteConfiguration);
                _unitOfWork.Complete();

                _siteStatusCheckScheduler.Unschedule(siteConfiguration);
            }
        }

        public void UpdateSiteConfiguration(UpdateSiteConfigurationDto updateSiteConfigurationData)
        {
            var siteConfiguration = _siteConfigurationRepository
                .GetSiteConfigurationById(updateSiteConfigurationData.Id);

            if (siteConfiguration != null)
            {
                siteConfiguration.SiteUrl = updateSiteConfigurationData.SiteUrl;
                siteConfiguration.SiteStatusCheckIntervalTypeId = updateSiteConfigurationData.SiteStatusCheckIntervalTypeId;
                siteConfiguration.SiteStatusCheckInterval = updateSiteConfigurationData.SiteStatusCheckInterval;
                siteConfiguration.LastUpdated = DateTime.UtcNow;

                _siteConfigurationRepository
                    .EditSiteConfiguration(siteConfiguration);
                _unitOfWork.Complete();

                _siteStatusCheckScheduler.Schedule(siteConfiguration);
            }
        }

        public SiteConfigurationDto GetSiteConfigurationDto(int siteConfigurationId)
        {
            var siteConfiguration = _siteConfigurationRepository
                .GetSiteConfigurationById(siteConfigurationId);

            if (siteConfiguration == null)
            {
                return null;
            }

            return new SiteConfigurationDto
            {
                Id = siteConfiguration.Id,
                SiteStatusCheckInterval = siteConfiguration.SiteStatusCheckInterval,
                SiteStatusCheckIntervalTypeId = siteConfiguration.SiteStatusCheckIntervalTypeId,
                SiteUrl = siteConfiguration.SiteUrl
            };
        }

        public IEnumerable<SiteStatusCheckIntervalTypeDto> GetSiteStatusCheckIntervalTypeDatas()
        {
            return _siteStatusCheckIntervalTypeRepository
                .ListSiteStatusCheckIntervalTypes()
                .Select(st => new SiteStatusCheckIntervalTypeDto
                {
                    SiteStatusCheckIntervalTypeId = st.Id,
                    SiteStatusCheckIntervalTypeName = st.Name
                })
                .ToList();
        }
    }
}
