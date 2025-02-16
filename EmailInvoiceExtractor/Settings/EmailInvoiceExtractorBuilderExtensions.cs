using EmailInvoiceExctractor.Services;
using EmailInvoiceExtractor.Services.Imap;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace EmailInvoiceExctractor.Settings
{
    public static class EmailInvoiceExtractorBuilderExtensions
    {
        public static IServiceCollection AddEmailInvoiceExtractor(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.GetSection("EmailInvoiceExtractor");
            services.AddSingleton<IImapService, ImapInteractionsService>();
            services.AddSingleton<IEmailScrapper, EmailScrapperService>();

            return services;
        }

        public static IApplicationBuilder UseEmailInvoiceExtractor(this IApplicationBuilder app)
        {
            var options = app.ApplicationServices.GetRequiredService<IOptions<EmailInvoiceExtractorOptions>>();
            var settings = options.Value;

            return app;
        }
    }
}
