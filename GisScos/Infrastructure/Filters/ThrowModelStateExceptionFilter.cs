using Microsoft.AspNetCore.Mvc.Filters;

namespace GisScos.Infrastructure.Filters
{
    /// <summary>
    /// Экшен аттрибут для обработки ошибок ModelState в ручном режиме
    /// </summary>
    public class ThrowModelStateExceptionFilter : IActionFilter
    {
        /// <summary>
        /// Вызов до запуска основного метода
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Вызов после запуска основного метода
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
