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
    public class AppController : Controller
    {
        private readonly IAppRepository _appRepository;
        private readonly IMemoryCache _memoryCache;
        public AppController(IAppRepository appRepository,
            ITraceRepository traceRepository,
            IMemoryCache memoryCache)
        {
            _appRepository = appRepository;
            _memoryCache = memoryCache;
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
            _memoryCache.Remove($"app_{app.Id}");

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
            _memoryCache.Remove($"app_{app.Id}");

            return Json(true);
        }

    }
}
