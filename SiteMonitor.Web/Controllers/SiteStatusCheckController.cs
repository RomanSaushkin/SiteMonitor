using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SiteMonitor.BusinessLogic.Services;
using SiteMonitor.Web.Models;

namespace SiteMonitor.Web.Controllers
{
    public class SiteStatusCheckController : Controller
    {
        private ISiteStatusCheckService _siteStatusCheckService;

        public SiteStatusCheckController(ISiteStatusCheckService siteStatusCheckService)
        {
            _siteStatusCheckService = siteStatusCheckService;
        }

        public IActionResult Index()
        {
            var siteStatusCheckResults = _siteStatusCheckService
                .GetSiteStatusCheckResults()
                .Select(s => new SiteStatusCheckViewModel
                {
                    IsAvailable = s.IsAvailable,
                    LastCheckedDate = s.LastCheckedDate,
                    SiteUrl = s.SiteUrl
                });

            return View(siteStatusCheckResults);
        }
    }
}