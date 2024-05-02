using Vostok.Logging.Abstractions;

namespace VirtualLab.MiddleWare
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILog _log;
        public ExceptionHandlerMiddleware(RequestDelegate next, ILog log)
        {
            _next = next;
            log.ForContext("exception");
            _log = log;
        }
        // todo:: потенциально нужно допилить))) отправку ответа с ошибкой.
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Обработка исключения здесь
                // Например, логирование ошибки
                _log.Error($"{ex.ToString()}");
                // Можно также изменить контекст ответа, например, установить статус кода ошибки
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(ex.ToString());
            }
        }
    }
}