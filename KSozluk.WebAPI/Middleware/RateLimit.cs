//namespace KSozluk.WebAPI.Middleware
//{
//    public class RateLimit
//    {
//        private readonly RequestDelegate _next;
//        private static DateTime _lastRequestTime = DateTime.MinValue;
//        private static readonly TimeSpan _timeSpan = TimeSpan.FromSeconds(1);

//        public RateLimit(RequestDelegate next)
//        {
//            _next = next;
//        }

//        public async Task InvokeAsync(HttpContext context)
//        {
//            if (DateTime.UtcNow - _lastRequestTime < _timeSpan)
//            {
//                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
//                await context.Response.WriteAsync("Rate limit exceeded. Please wait.");
//                return;
//            }

//            _lastRequestTime = DateTime.UtcNow;
//            await _next(context);
//        }
//    }

//}
