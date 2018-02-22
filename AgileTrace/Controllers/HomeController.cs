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

namespace AgileTrace.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IAppRepository _appRepository;
        private readonly ITraceRepository _traceRepository;
        private readonly IAppCache _appCache;
        public HomeController(IAppRepository appRepository,ITraceRepository traceRepository,IAppCache appCache)
        {
            _appRepository = appRepository;
            _traceRepository = traceRepository;
            _appCache = appCache;
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

        public IActionResult PageTrace(string appId, string logLevel, int pageIndex, int pageSize)
        {
            var result = _traceRepository.Page(pageIndex, pageSize, appId, logLevel);
            var totalCount = _traceRepository.Count(appId,logLevel);

            return Json(new
            {
                result,
                totalCount
            });
        }

        public IActionResult Apps()
        {
            var result = _appRepository.All();
            return Json(new
            {
                result,
            });
        }

        public IActionResult AddApp([FromBody]App model)
        {
            if (model == null || string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.SecurityKey))
            {
                return Json(false);
            }

            if (string.IsNullOrEmpty(model.Id))
            {
                model.Id = Guid.NewGuid().ToString("N");
            }

            _appRepository.Insert(model);

            return Json(true);
        }

        public IActionResult UpdateApp([FromBody]App model)
        {
            if (model == null || string.IsNullOrEmpty(model.Id))
            {
                return Json(false);
            }

            var app = _appRepository.Get(model.Id);
            if (app == null)
            {
                return Json(false);
            }

            app.Name = model.Name;
            app.SecurityKey = model.SecurityKey;

            _appRepository.Update(app);
            _appCache.Remove(app.Id);

            return Json(true);
        }

        public IActionResult DeleteApp(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Json(false);
            }

            var app = _appRepository.Get(id);
            if (app == null)
            {
                return Json(false);
            }


            _appRepository.Delete(app);
            _appCache.Remove(app.Id);

            return Json(true);
        }

        public IActionResult GetHost()
        {
            return Json($"{HttpContext.Request.Host.Host}:{HttpContext.Request.Host.Port}");
        }

        public IActionResult GetChartData([FromBody]List<string> levels)
        {
            var result = new List<object>();
            List<string> appIds = null;
            appIds = _appRepository.All().Select(a => a.Id).ToList();
            appIds.Insert(0, "");
            foreach (var appId in appIds)
            {
                var data = AppChartData(appId, levels);
                result.Add(data);
            }

            return Json(result);
        }

        private object AppChartData(string appId, List<string> levels)
        {
            var appName = string.IsNullOrEmpty(appId) ? "" : _appCache.Get(appId).Name;
            var result = _traceRepository.GroupLevel(levels, appId);

            return new
            {
                appName,
                data = result
            };
        }
    }
}
