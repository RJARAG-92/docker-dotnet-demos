using Demo03.Requests.Application.UseCases.ApproveRequest;
using Demo03.Requests.Application.UseCases.CreateRequest;
using Demo03.Requests.Application.UseCases.GetRequestAudit;
using Demo03.Requests.Application.UseCases.GetRequestById;
using Demo03.Requests.Application.UseCases.RejectRequest;
using Demo03.Requests.Application.UseCases.SubmitRequest;
using Microsoft.Extensions.DependencyInjection;

namespace Demo03.Requests.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRequestsApplication(this IServiceCollection services)
        {
            services.AddTransient<CreateRequestHandler>();
            services.AddTransient<SubmitRequestHandler>();
            services.AddTransient<ApproveRequestHandler>();
            services.AddTransient<RejectRequestHandler>();
            services.AddTransient<GetRequestByIdHandler>();
            services.AddTransient<GetRequestAuditHandler>();

            return services;
        }
    }
}
