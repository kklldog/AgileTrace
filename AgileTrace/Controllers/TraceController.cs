using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgileTrace.Filters;
using AgileTrace.Models;
using AgileTrace.Repository;
using AgileTrace.Repository.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

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
                model.Time = DateTime.Now;
                using (var db = new TraceDbContext())
                {
                    db.Traces.Add(model);
                    db.SaveChanges();
                }
            }

            return "ok";
        }

    }
}