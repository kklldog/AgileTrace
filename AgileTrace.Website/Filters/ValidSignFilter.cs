using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AgileTrace.Entity;
using AgileTrace.IRepository;
using AgileTrace.IService;
using AgileTrace.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace AgileTrace.Filters
{
    public class ValidSignAttribute : ActionFilterAttribute
    {
        private readonly IAppRepository _appRepository;
        private readonly IMemoryCache _appCache;
        private readonly ILogger _logger;
        public ValidSignAttribute(
            IMemoryCache appCache,
            IAppRepository appRepository,
            ILoggerFactory loggerFactory)
        {
            _appRepository = appRepository;
            _appCache = appCache;
            _logger = loggerFactory.CreateLogger<ValidSignAttribute>();
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var sign = context.HttpContext.Request.Headers.FirstOrDefault(h => h.Key == "sign");
            if (sign.Value.Count == 0 || string.IsNullOrEmpty(sign.Value[0]))
            {
                SetSignFail(context);
            }

            var result = CheckSign(context);
            if (!result)
            {
                SetSignFail(context);
            }

            base.OnActionExecuting(context);
        }

        private bool CheckSign(ActionExecutingContext context)
        {
            var signHeaders = GetSignHeaders(context);
            var app = _appCache.Get<App>($"app_{signHeaders.appid}");
            if (app == null)
            {
                app = _appRepository.Get(signHeaders.appid);
            }
            if (app == null)
            {
                SetSignFail(context);
                return false;
            }

            var rightSign = SignHelper.MakeApiSign(app.SecurityKey, signHeaders.time, signHeaders.requestid);

            var result = signHeaders.sign.Equals(rightSign, StringComparison.CurrentCultureIgnoreCase);
            if (!result)
            {
                _logger.LogWarning($"Check sign faild,IP:{context.HttpContext.Connection.RemoteIpAddress} App:{signHeaders.appid} RightSign:{rightSign} wrongSign:{signHeaders.sign}");
            }

            return result;
        }

        private void SetSignFail(ActionExecutingContext context)
        {
            //var signHeaders = GetSignHeaders(context);
            //_logger.LogWarning("api sign fail . sign:{0} appid:{1} time:{2} requestid:{3}",signHeaders.sign,signHeaders.appid,signHeaders.time,signHeaders.requestid);

            context.Result = new ContentResult()
            {
                Content = "sign fail",
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.BadRequest
            };
        }

        private (string sign, string appid, string time, string requestid) GetSignHeaders(ActionExecutingContext context)
        {
            var sign = context.HttpContext.Request.Headers.FirstOrDefault(h => h.Key == "sign").Value.ToArray();
            var appid = context.HttpContext.Request.Headers.FirstOrDefault(h => h.Key == "appid").Value.ToArray();
            var time = context.HttpContext.Request.Headers.FirstOrDefault(h => h.Key == "time").Value.ToArray();
            var requestid = context.HttpContext.Request.Headers.FirstOrDefault(h => h.Key == "requestid").Value.ToArray();

            var signStr = string.Join("", sign);
            var appidStr = string.Join("", appid);
            var timeStr = string.Join("", time);
            var requestidStr = string.Join("", requestid);

            return (signStr, appidStr, timeStr, requestidStr);
        }
    }
}
