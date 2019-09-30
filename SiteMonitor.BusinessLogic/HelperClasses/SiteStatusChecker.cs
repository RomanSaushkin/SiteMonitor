using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace SiteMonitor.BusinessLogic.HelperClasses
{
    public sealed class SiteStatusChecker : ISiteStatusChecker
    {
        private BlockingCollection<SiteStatusCheckItem> _sitesToCheckQueue;
        private Dictionary<int, SiteConfigurationState> _trackedSiteConfigurationStates;
        private Dictionary<int, SiteStatusCheckResult> _siteStatusCheckResults;
        private ReaderWriterLockSlim _trackedSiteConfigurationStatesLock;
        private ReaderWriterLockSlim _siteStatusesLock;

        private ISiteAvailabilityChecker _siteStatusCheckService;

        public SiteStatusChecker(ISiteAvailabilityChecker siteStatusCheckService)
        {
            _sitesToCheckQueue = new BlockingCollection<SiteStatusCheckItem>(new ConcurrentQueue<SiteStatusCheckItem>());
            _trackedSiteConfigurationStates = new Dictionary<int, SiteConfigurationState>();
            _siteStatusCheckResults = new Dictionary<int, SiteStatusCheckResult>();
            _trackedSiteConfigurationStatesLock = new ReaderWriterLockSlim();
            _siteStatusesLock = new ReaderWriterLockSlim();

            _siteStatusCheckService = siteStatusCheckService;
        }

        public void EnqueueSiteToCheck(SiteStatusCheckItem siteStatusCheckItem)
        {
            if (!_sitesToCheckQueue.IsAddingCompleted)
            {
                AddOrUpdateTrackedSiteConfigurationState(siteStatusCheckItem);
                EnqueueSiteToCheckInternal(siteStatusCheckItem);
            }
        }

        public void InvalidateSiteConfiguration(int siteConfigurationId)
        {
            _trackedSiteConfigurationStatesLock.EnterWriteLock();
            _siteStatusesLock.EnterWriteLock();

            try
            {
                _siteStatusCheckResults.Remove(siteConfigurationId);
                _trackedSiteConfigurationStates.Remove(siteConfigurationId);
            }
            finally
            {
                _trackedSiteConfigurationStatesLock.ExitWriteLock();
                _siteStatusesLock.ExitWriteLock();
            }
        }

        public void StartChecking()
        {
            foreach (var item in _sitesToCheckQueue.GetConsumingEnumerable())
            {
                try
                {
                    ProcessQueueItem(item);
                }
                catch (Exception exc)
                {
                    Debug.WriteLine(exc.Message);
                }
            }
        }

        public void StopChecking()
        {
            _sitesToCheckQueue.CompleteAdding();
        }

        public Dictionary<int, SiteStatusCheckResult> GetCheckedSiteStatusResults()
        {
            _siteStatusesLock.EnterReadLock();

            try
            {
                return _siteStatusCheckResults.ToDictionary(
                    s => s.Key,
                    s => s.Value);
            }
            finally
            {
                _siteStatusesLock.ExitReadLock();
            }
        }

        private void ProcessQueueItem(SiteStatusCheckItem item)
        {
            var siteConfigurationState = GetSiteConfigurationState(item.SiteConfigurationId);

            if (siteConfigurationState == null ||
                item.EnqueueDateTime < siteConfigurationState.LastUpdated)
            {
                return;
            }

            var isSiteAvailable = _siteStatusCheckService
                .GetIsSiteAvailable(item.SiteConfigurationState.SiteUrl);

            Debug.WriteLine($"{DateTime.Now} - {item.SiteConfigurationState.SiteUrl}: {(isSiteAvailable ? "Available" : "Unavailable")}");

            UpdateSiteStatusIfValid(
                item.SiteConfigurationId,
                isSiteAvailable,
                DateTime.UtcNow);
        }

        private void AddOrUpdateTrackedSiteConfigurationState(SiteStatusCheckItem siteStatusCheckItem)
        {
            _trackedSiteConfigurationStatesLock.EnterWriteLock();

            try
            {
                _trackedSiteConfigurationStates[siteStatusCheckItem.SiteConfigurationId] = siteStatusCheckItem.SiteConfigurationState;
            }
            finally
            {
                _trackedSiteConfigurationStatesLock.ExitWriteLock();
            }
        }

        private SiteConfigurationState GetSiteConfigurationState(int siteConfigurationId)
        {
            _trackedSiteConfigurationStatesLock.EnterReadLock();

            try
            {
                if (_trackedSiteConfigurationStates.TryGetValue(siteConfigurationId, out var siteConfigurationState))
                {
                    return siteConfigurationState;
                }
                else
                {
                    return null;
                }
            }
            finally
            {
                _trackedSiteConfigurationStatesLock.ExitReadLock();
            }
        }

        private void UpdateSiteStatusIfValid(int siteConfigurationId, bool isAvailable, DateTime siteCheckTime)
        {
            _trackedSiteConfigurationStatesLock.EnterReadLock();

            try
            {
                if (!_trackedSiteConfigurationStates.ContainsKey(siteConfigurationId))
                {
                    return;
                }

                _siteStatusesLock.EnterWriteLock();

                try
                {
                    _siteStatusCheckResults[siteConfigurationId] = new SiteStatusCheckResult
                    {
                        IsAvailable = isAvailable,
                        CheckTime = siteCheckTime
                    };
                }
                finally
                {
                    _siteStatusesLock.ExitWriteLock();
                }
            }
            finally
            {
                _trackedSiteConfigurationStatesLock.ExitReadLock();
            }
        }

        private void EnqueueSiteToCheckInternal(SiteStatusCheckItem siteStatusCheckItem)
        {
            _sitesToCheckQueue.TryAdd(siteStatusCheckItem);
        }
    }
}
