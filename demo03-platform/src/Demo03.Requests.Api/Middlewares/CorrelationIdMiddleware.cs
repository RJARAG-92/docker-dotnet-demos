namespace Demo03.Requests.Api.Middlewares
{
    public sealed class CorrelationIdMiddleware
    {
        public const string HeaderName = "X-Correlation-Id";
        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context)
        {
            var cid = context.Request.Headers[HeaderName].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(cid))
                cid = Guid.NewGuid().ToString("N");

            context.Items[HeaderName] = cid;
            context.Response.Headers[HeaderName] = cid;

            await _next(context);
        }
    }
}
