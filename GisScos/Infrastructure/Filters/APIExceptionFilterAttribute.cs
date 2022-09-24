using Common.Models.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace GisScos.Infrastructure.Filters
{
    /// <summary>
    /// Аттрибут для обработки ошибок API
    /// </summary>
    public class APIExceptionFilterAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// Возвращает json-объект с определенным статус-кодом при возникновении ошибки
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(ExceptionContext context)
        {
            context.HttpContext.Response.ContentType = "application/json";

            if (context.ModelState != null && !context.ModelState.IsValid)
            {
                var errors = new Dictionary<string, string[]>();
                foreach (var property in context.ModelState.Keys)
                {
                    var value = context.ModelState[property];
                    if (value != null)
                    {
                        var errMsgs = new List<string>();
                        foreach (var error in value.Errors)
                        {
                            errMsgs.Add(error.ErrorMessage);
                        }
                        errors.Add(property, errMsgs.ToArray());
                    }
                }

                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Result = new JsonResult(
                    new ErrorModel
                    {
                        Code = "validationException",
                        Message = "Произошла одна или несколько ошибок валидации данных.",
                        Errors = errors
                    }
                );
                return;
            }

            var code = HttpStatusCode.InternalServerError;
            string internalCode = "";

            if (context.Exception is ExceptionWithCode exceptionWithCode)
            {
                code = HttpStatusCode.BadRequest;
                internalCode = exceptionWithCode.Code;
            }

            IActionResult result;
            if (code == HttpStatusCode.InternalServerError)
            {
                result = new JsonResult(new ErrorModel
                {
                    Message = context.Exception.InnerException?.Message ?? context.Exception.Message
                });

                if (context?.Exception?.TargetSite?.DeclaringType != null)
                {
                    var loggerType = typeof(ILogger<>).MakeGenericType(new Type[] { context.Exception.TargetSite.DeclaringType });
                    if (loggerType != null)
                    {
                        var logger = (ILogger?)context.HttpContext.RequestServices.GetService(loggerType);
                        if (logger != null)
                        {
                            logger.LogError(context.Exception, context.Exception.InnerException?.Message ?? context.Exception.Message);
                        }
                    }
                }
            }
            else
            {
                result = new JsonResult(new ErrorModel
                {
                    Code = internalCode,
                    Message = context.Exception.Message
                });
            }

            if (context != null)
            {
                context.HttpContext.Response.StatusCode = (int)code;
                context.Result = result;
            }
        }
    }
}
