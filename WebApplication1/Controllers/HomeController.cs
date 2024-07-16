using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using WebApplication1.Helpers;
using WebApplication1.Interfaces;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IClublRepository _clublRepository;

        public HomeController(ILogger<HomeController> logger, IClublRepository clublRepository) 
        {
            _logger = logger;
            _clublRepository = clublRepository;

        }

        public async Task<IActionResult> Index()
        {
            var ipInfo = new IPinfo();
            var HomeViewModel = new HomeViewModel();
            try
            {
                string url = "https://ipinfo.io?token=8efe36fb47cd3d";
                var info=new WebClient().DownloadString(url);
                ipInfo=JsonConvert.DeserializeObject<IPinfo>(info);
                RegionInfo myRT1=new RegionInfo(ipInfo.Country);
                ipInfo.Country = myRT1.EnglishName;
                HomeViewModel.City = ipInfo.City;
                HomeViewModel.State = ipInfo.Region;
                if (HomeViewModel.City != null)
                {
                    HomeViewModel.Clubs = await _clublRepository.GetClubByCity(HomeViewModel.City);

                }
                else {
                    HomeViewModel.Clubs = null;
                
                }
                return View(HomeViewModel);
            }
            catch (Exception ex)
            {
                HomeViewModel.Clubs = null;

            }
            return View(HomeViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
