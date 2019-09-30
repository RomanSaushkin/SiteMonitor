using SiteMonitor.Data.Enums;
using System;

namespace SiteMonitor.BusinessLogic.HelperClasses
{
    public class SiteStatusCheckIntervalConverter : ISiteStatusCheckIntervalConverter
    {
        public int ConvertToMilliseconds(int siteStatusCheckInterval, int siteStatusCheckIntervalTypeId)
        {
            var seconds = 0;

            switch ((SiteStatusCheckIntervalTypesEnum)siteStatusCheckIntervalTypeId)
            {
                case SiteStatusCheckIntervalTypesEnum.Seconds:
                    {
                        seconds = siteStatusCheckInterval;
                        break;
                    }
                case SiteStatusCheckIntervalTypesEnum.Minutes:
                    {
                        seconds = siteStatusCheckInterval * 60;
                        break;
                    }
                case SiteStatusCheckIntervalTypesEnum.Hours:
                    {
                        seconds = siteStatusCheckInterval * 60 * 60;
                        break;
                    }
                default:
                    {
                        throw new InvalidOperationException("Invalid site status check interval.");
                    }
            }

            return seconds * 1000;
        }
    }
}
