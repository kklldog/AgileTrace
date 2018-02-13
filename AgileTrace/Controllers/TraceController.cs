using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgileTrace.Filters;
using AgileTrace.Models;
using AgileTrace.Repository;
using AgileTrace.Repository.Entity;
using AgileTrace.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace AgileTrace.Controllers
{
    [ValidSign]
    [Produces("application/json")]
    [Route("api/Trace")]
    public class TraceController : Controller
    {
        [HttpPost]
        public string Post([FromBody]Trace model)
        {
            if (model != null)
            {
                var appId = HttpContext.Request.Headers.First(h => h.Key == "appid")
                    .Value.ToArray()[0];
                model.AppId = appId;
                model.Time = DateTime.Now;
                using (var db = new TraceDbContext())
                {
                    db.Traces.Add(model);
                    db.SaveChanges();
                }

                WebsocketService.SendToAll(JsonConvert.SerializeObject(model));
            }

            return "ok";
        }

    }
}