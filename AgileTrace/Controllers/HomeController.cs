using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AgileTrace.Models;
using AgileTrace.Repository;
using AgileTrace.Repository.Entity;
using AgileTrace.Services;
using Microsoft.AspNetCore.Authorization;

namespace AgileTrace.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
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
            using (var db = new TraceDbContext())
            {
                var page = db.Traces.Where(t =>
               (string.IsNullOrEmpty(appId) || t.AppId == appId)
                    && (string.IsNullOrEmpty(logLevel) || t.Level == logLevel))
                    .OrderByDescending(t => t.Time)
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize);
                var result = page.ToList();
                var totalCount = db.Traces.Count(t =>
                                (string.IsNullOrEmpty(appId) || t.AppId == appId)
                                 && (string.IsNullOrEmpty(logLevel) || t.Level == logLevel));

                return Json(new
                {
                    result,
                    totalCount
                });
            }
        }

        public IActionResult Apps()
        {
            using (var db = new TraceDbContext())
            {
                var result = db.Apps.ToList();

                return Json(new
                {
                    result,
                });
            }
        }

        public IActionResult AddApp([FromBody]App model)
        {
            if (model == null || string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.SecurityKey))
            {
                return Json(false);
            }

            using (var db = new TraceDbContext())
            {
                if (string.IsNullOrEmpty(model.Id))
                {
                    model.Id = Guid.NewGuid().ToString("N");
                }
                db.Apps.Add(model);
                db.SaveChanges();
                AppService.Add(model);

                return Json(true);
            }
        }

        public IActionResult UpdateApp([FromBody]App model)
        {
            if (model == null || string.IsNullOrEmpty(model.Id))
            {
                return Json(false);
            }

            using (var db = new TraceDbContext())
            {
                var app = db.Apps.Find(model.Id);
                if (app == null)
                {
                    return Json(false);
                }

                app.Name = model.Name;
                app.SecurityKey = model.SecurityKey;

                db.Apps.Update(app);
                db.SaveChanges();

                AppService.Remove(app);
                AppService.Add(app);

                return Json(true);
            }
        }

        public IActionResult DeleteApp(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Json(false);
            }

            using (var db = new TraceDbContext())
            {
                var app = db.Apps.Find(id);
                if (app == null)
                {
                    return Json(false);
                }

                db.Apps.Remove(app);
                db.SaveChanges();

                AppService.Remove(app);

                return Json(true);
            }
        }

        public IActionResult GetHost()
        {
            return Json($"{HttpContext.Request.Host.Host}:{HttpContext.Request.Host.Port}");
        }

        public IActionResult GetChartData([FromBody]List<string> levels)
        {
            var result = new List<object>();
            List<string> appIds = null;
            using (var db = new TraceDbContext())
            {
                appIds = db.Apps.Select(a => a.Id).ToList();
                appIds.Insert(0, "");
               
            }
            foreach (var appId in appIds)
            {
                var data = AppChartData(appId, levels);
                result.Add(data);
            }

            return Json(result);
        }

        private object AppChartData(string appId, List<string> levels)
        {
            using (var db = new TraceDbContext())
            {
                var appName = string.IsNullOrEmpty(appId) ? "" : AppService.Get(appId).Name;
                var gp = db.Traces.Where(t => levels.Contains(t.Level)
                && (string.IsNullOrEmpty(appId) || t.AppId == appId))
                    .GroupBy(t => t.Level);
                var result = gp.Select(g => new
                {
                    name = g.Key,
                    value = g.Count()
                }).ToList();

                return new
                {
                    appName,
                    data = result
                };
            }
        }
    }
}
