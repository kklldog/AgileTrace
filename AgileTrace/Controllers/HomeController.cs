using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AgileTrace.Models;
using AgileTrace.Repository;
using AgileTrace.Repository.Entity;
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

        public IActionResult PageTrace(string appId, int pageIndex, int pageSize)
        {
            using (var db = new TraceDbContext())
            {
                var result = db.Traces
                    .Where(t => string.IsNullOrEmpty(appId) || t.AppId == appId)
                    .OrderByDescending(t => t.Time)
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize).ToList();
                var totalCount = db.Traces.Count();

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
                model.Id = Guid.NewGuid().ToString("N");
                db.Apps.Add(model);
                db.SaveChanges();

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

                return Json(true);
            }
        }
    }
}
