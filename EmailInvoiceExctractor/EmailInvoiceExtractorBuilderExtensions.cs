﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace EmailInvoiceExctractor
{
    public static class EmailInvoiceExtractorBuilderExtensions
    {
        public static IServiceCollection AddEmailInvoiceExtractor(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.GetSection("EmailInvoiceExtractor");
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
