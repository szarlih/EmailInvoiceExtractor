using EmailInvoiceExctractor.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace EmailInvoiceExctractor
{
    public class EmailScrapperService : BackgroundService, IEmailScrapper
    {
        private readonly List<EmailAccount> _accounts;
        private readonly int? _bulkSize;
        private List<int> _emails;

        public EmailScrapperService(IOptions<EmailInvoiceExtractorOptions> options)
        {
            _accounts = options.Value.Accounts;
            _emails = new List<int>();
            _bulkSize = options.Value.ProcessingBulkSize;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                ProcessEmails();
                await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
            }
        }

        public List<int> GetProcessedEmails()
        {
            return _emails;
        }

        public void TriggerImmediateCheck()
        {
            ProcessEmails();
        }

        public int GetProcessedEmailCount()
        {
            return _emails.FirstOrDefault();
        }

        private void ProcessEmails()
        {
            foreach (var account in _accounts)
            {
                _emails.Add(account.ScanForNewInvoices(DateTime.Now, _bulkSize ?? 11));
            }
        }
    }
}
