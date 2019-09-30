using SiteMonitor.Data.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace SiteMonitor.BusinessLogic.HelperClasses
{
    public class SiteStatusCheckScheduler : ISiteStatusCheckScheduler
    {
        private Dictionary<int, Timer> _scheduledSiteChecks;
        private object _locker;

        private ISiteStatusChecker _siteStatusChecker;
        private ISiteStatusCheckIntervalConverter _siteStatusCheckIntervalConverter;

        public SiteStatusCheckScheduler(ISiteStatusChecker siteStatusChecker, 
            ISiteStatusCheckIntervalConverter siteStatusCheckIntervalConverter)
        {
            _scheduledSiteChecks = new Dictionary<int, Timer>();
            _locker = new object();

            _siteStatusChecker = siteStatusChecker;
            _siteStatusCheckIntervalConverter = siteStatusCheckIntervalConverter;
        }

        public void Schedule(SiteConfiguration siteConfiguration)
        {
            try
            {
                lock(_locker)
                {
                    if (_scheduledSiteChecks.TryGetValue(siteConfiguration.Id, out var scheduledSiteCheck))
                    {
                        scheduledSiteCheck.Change(
                            Timeout.Infinite,
                            Timeout.Infinite);
                    }

                    var siteStatusCheckIntervalInMilliseconds = _siteStatusCheckIntervalConverter.ConvertToMilliseconds(
                        siteConfiguration.SiteStatusCheckInterval,
                        siteConfiguration.SiteStatusCheckIntervalTypeId);

                    _scheduledSiteChecks[siteConfiguration.Id] = new Timer(
                        SiteStatusCheckEnqueuer(siteConfiguration),
                        null,
                        0,
                        siteStatusCheckIntervalInMilliseconds);
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
            }
        }

        public void Unschedule(SiteConfiguration siteConfiguration)
        {
            try
            {
                lock (_locker)
                {
                    if (_scheduledSiteChecks.TryGetValue(siteConfiguration.Id, out var scheduledSiteCheck))
                    {
                        scheduledSiteCheck.Change(
                            Timeout.Infinite,
                            Timeout.Infinite);

                        _scheduledSiteChecks.Remove(siteConfiguration.Id);

                        _siteStatusChecker.InvalidateSiteConfiguration(siteConfiguration.Id);
                    }
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
            }
        }

        private TimerCallback SiteStatusCheckEnqueuer(SiteConfiguration siteConfiguration)
        {
            return state =>
            {
                try
                {
                    _siteStatusChecker.EnqueueSiteToCheck(new SiteStatusCheckItem
                    {
                        SiteConfigurationId = siteConfiguration.Id,
                        EnqueueDateTime = DateTime.UtcNow,
                        SiteConfigurationState = new SiteConfigurationState
                        {
                            SiteUrl = siteConfiguration.SiteUrl,
                            LastUpdated = siteConfiguration.LastUpdated
                        }
                    });
                }
                catch (Exception exc)
                {
                    Debug.WriteLine(exc.Message);
                }
            };
        }
    }
}
