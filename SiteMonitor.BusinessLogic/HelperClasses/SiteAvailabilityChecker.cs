using System;
using System.Net;

namespace SiteMonitor.BusinessLogic.HelperClasses
{
    public class SiteAvailabilityChecker : ISiteAvailabilityChecker
    {
        public bool GetIsSiteAvailable(string siteUrl)
        {
            try
            {
                var uri = new UriBuilder(siteUrl);

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(uri.Uri);
                httpWebRequest.Timeout = 2000;
                httpWebRequest.Method = "GET";
                httpWebRequest.KeepAlive = false;

                using (var response = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    return response.StatusCode == HttpStatusCode.OK;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
