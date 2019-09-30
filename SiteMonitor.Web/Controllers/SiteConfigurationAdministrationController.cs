using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SiteMonitor.BusinessLogic.Services;
using SiteMonitor.BusinessLogic.Services.ServiceDtos;
using SiteMonitor.Web.Models;

namespace SiteMonitor.Web.Controllers
{
    public class SiteConfigurationAdministrationController : Controller
    {
        private ISiteConfigurationService _siteConfigurationService;

        public SiteConfigurationAdministrationController(ISiteConfigurationService siteConfigurationService)
        {
            _siteConfigurationService = siteConfigurationService;
        }

        public IActionResult Index()
        {
            var siteStatusCheckResults = _siteConfigurationService
                .GetSiteConfigurations()
                .Select(s => new SiteConfigurationViewModel
                {
                    Id = s.Id,
                    LastUpdated = s.LastUpdated,
                    SiteStatusCheckInterval = s.SiteStatusCheckInterval,
                    SiteStatusCheckIntervalTypeName = s.SiteStatusCheckIntervalTypeName,
                    SiteUrl = s.SiteUrl
                });

            return View(siteStatusCheckResults);
        }

        public IActionResult AddSiteConfiguration()
        {
            var viewModel = PopulateAddEditSiteConfigurationViewModel(
                new AddEditSiteConfigurationSiteConfigurationViewModel());

            return View(
                "AddEditSiteConfiguration",
                viewModel);
        }

        public IActionResult EditSiteConfiguration(int siteConfigurationId)
        {
            var data = _siteConfigurationService
                .GetSiteConfigurationDto(siteConfigurationId);

            if (data == null)
            {
                return View("SiteConfigurationNotFound");
            }

            var siteConfiguration = new AddEditSiteConfigurationSiteConfigurationViewModel
            {
                SiteUrl = data.SiteUrl,
                SiteConfigurationId = data.Id,
                SiteStatusCheckInterval = data.SiteStatusCheckInterval,
                SiteStatusCheckIntervalTypeId = data.SiteStatusCheckIntervalTypeId
            };

            var viewModel = PopulateAddEditSiteConfigurationViewModel(
                siteConfiguration);

            return View(
                "AddEditSiteConfiguration", 
                viewModel);
        }

        [HttpPost]
        public IActionResult SaveSiteConfiguration(AddEditSiteConfigurationViewModel model)
        {
            if(ModelState.IsValid)
            {
                if (model.SiteConfiguration.SiteConfigurationId.HasValue)
                {
                    _siteConfigurationService
                        .UpdateSiteConfiguration(new UpdateSiteConfigurationDto
                        {
                            Id = model.SiteConfiguration.SiteConfigurationId.Value,
                            SiteStatusCheckIntervalTypeId = model.SiteConfiguration.SiteStatusCheckIntervalTypeId.Value,
                            SiteUrl = model.SiteConfiguration.SiteUrl,
                            SiteStatusCheckInterval = model.SiteConfiguration.SiteStatusCheckInterval.Value
                        });
                }
                else
                {
                    _siteConfigurationService
                        .AddNewSiteConfiguration(new AddNewSiteConfigurationDto
                        {
                            SiteStatusCheckIntervalTypeId = model.SiteConfiguration.SiteStatusCheckIntervalTypeId.Value,
                            SiteUrl = model.SiteConfiguration.SiteUrl,
                            SiteStatusCheckInterval = model.SiteConfiguration.SiteStatusCheckInterval.Value
                        });
                }

                return RedirectToAction("Index");
            }
            else
            {
                var viewModel = PopulateAddEditSiteConfigurationViewModel(
                    model.SiteConfiguration);

                return View(
                    "AddEditSiteConfiguration",
                    viewModel);
            }
        }

        [HttpPost]
        public IActionResult DeleteSiteConfiguration(int siteConfigurationId)
        {
            _siteConfigurationService
                .DeleteSiteConfiguration(siteConfigurationId);

            return RedirectToAction("Index");
        }

        private AddEditSiteConfigurationViewModel PopulateAddEditSiteConfigurationViewModel(AddEditSiteConfigurationSiteConfigurationViewModel siteConfiguration)
        {
            var siteStatusCheckIntervalTypes = _siteConfigurationService
                .GetSiteStatusCheckIntervalTypeDatas();

            return new AddEditSiteConfigurationViewModel
            {
                SiteStatusCheckIntervalTypes = siteStatusCheckIntervalTypes
                    .Select(st =>
                        new SiteStatusCheckIntervalTypeViewModel
                        {
                            SiteStatusCheckIntervalTypeId = st.SiteStatusCheckIntervalTypeId,
                            SiteStatusCheckIntervalTypeName = st.SiteStatusCheckIntervalTypeName
                        })
                    .ToList(),
                SiteConfiguration = siteConfiguration
            };
        }
    }
}