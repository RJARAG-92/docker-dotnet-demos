using Demo03.Requests.Api.Middlewares;
using Demo03.Requests.Application.Abstractions;
using System.Security.Claims;

namespace Demo03.Requests.Api.System
{
    public sealed class HttpUserContext : IUserContext
    {
        private readonly IHttpContextAccessor _http;

        public HttpUserContext(IHttpContextAccessor http) => _http = http;

        public string Actor
        {
            get
            {
                var ctx = _http.HttpContext;

                // 1) Real: del JWT
                var user = ctx?.User;
                var fromJwt =
                    user?.FindFirstValue(ClaimTypes.Email) ??
                    user?.FindFirstValue("preferred_username") ??
                    user?.FindFirstValue(ClaimTypes.Name) ??
                    user?.FindFirstValue("sub");

                if (!string.IsNullOrWhiteSpace(fromJwt))
                    return fromJwt;

                // 2) Demo fallback: header
                var fromHeader = ctx?.Request.Headers["X-Actor"].ToString();
                if (!string.IsNullOrWhiteSpace(fromHeader))
                    return fromHeader;

                return "system";
            }
        }

        public string? CorrelationId
            => _http.HttpContext?.Items[CorrelationIdMiddleware.HeaderName]?.ToString();
    }
}
