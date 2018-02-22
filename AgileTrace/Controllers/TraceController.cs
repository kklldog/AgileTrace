using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgileTrace.Entity;
using AgileTrace.Filters;
using AgileTrace.IRepository;
using AgileTrace.Models;
using AgileTrace.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace AgileTrace.Controllers
{
    [ServiceFilter(typeof(ValidSignAttribute))]
    [Produces("application/json")]
    [Route("api/Trace")]
    public class TraceController : Controller
    {
        private readonly ITraceRepository _traceRepository;
        private readonly IWebsocketService _websocketService;
        public TraceController(ITraceRepository traceRepository, IWebsocketService websocketService)
        {
            _traceRepository = traceRepository;
            _websocketService = websocketService;
        }

        [HttpPost]
        public string Post([FromBody]Trace model)
        {
            if (model != null)
            {
                var appId = HttpContext.Request.Headers.First(h => h.Key == "appid")
                    .Value.ToArray()[0];
                model.AppId = appId;
                model.Time = DateTime.Now;
                _traceRepository.Insert(model);

                _websocketService.SendToAll(JsonConvert.SerializeObject(model));
            }

            return "ok";
        }

    }
}