using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AgileTrace.Entity;
using AgileTrace.IRepository;
using AgileTrace.IService;
using Microsoft.AspNetCore.Mvc;
using AgileTrace.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;

namespace AgileTrace.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IAppRepository _appRepository;
        private readonly ITraceRepository _traceRepository;
        private readonly IMemoryCache _memoryCache;
        public HomeController(IAppRepository appRepository,
            ITraceRepository traceRepository,
            IMemoryCache memoryCache)
        {
            _appRepository = appRepository;
            _traceRepository = traceRepository;
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult GetView(string viewName)
        {
            return View(viewName);
        }

        public IActionResult PageTrace(string appId, string logLevel, int pageIndex, int pageSize, DateTime startDate, DateTime endDate)
        {
            endDate = endDate.Date.AddDays(1);
            var result = _traceRepository.Page(pageIndex, pageSize, appId, logLevel, startDate, endDate);
            var totalCount = _traceRepository.Count(appId, logLevel, startDate, endDate);

            return Json(new
            {
                result,
                totalCount
            });
        }

        public IActionResult GetHost()
        {
            return Json($"{HttpContext.Request.Host.Host}:{HttpContext.Request.Host.Port}");
        }

        public IActionResult GetChartData([FromBody]List<string> levels)
        {
            const string cacheKey = "DashChatData";
            _memoryCache.TryGetValue(cacheKey, out List<object> result);
            if (result != null)
            {
                return Json(result);
            }

            result = new List<object>();
            List<string> appIds = null;
            appIds = _appRepository.All().Select(a => a.Id).ToList();
            appIds.Insert(0, "");
            foreach (var appId in appIds)
            {
                var data = AppChartData(appId, levels);
                result.Add(data);
            }

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromHours(12));
            _memoryCache.Set(cacheKey, result, cacheEntryOptions);

            return Json(result);
        }

        private object AppChartData(string appId, List<string> levels)
        {
            var app = _memoryCache.Get<App>($"app_{appId}");
            if (app == null)
            {
                app = _appRepository.Get(appId);
            }

            var appName = string.IsNullOrEmpty(appId) ? "" : app.Name;
            var result = _traceRepository.GroupLevel(levels, appId);

            return new
            {
                appName,
                data = result
            };
        }
    }
}
