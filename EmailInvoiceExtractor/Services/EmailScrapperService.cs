using EmailInvoiceExctractor.Models;
using EmailInvoiceExctractor.Settings;
using EmailInvoiceExtractor.Services.Imap;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace EmailInvoiceExctractor.Services
{
    public class EmailScrapperService : BackgroundService, IEmailScrapper
    {
        private readonly List<EmailAccount> _accounts;
        private readonly IImapService _imapService;
        private readonly int? _bulkSize;
        private List<IInvoiceEmail> _emails;

        public EmailScrapperService(IOptions<EmailInvoiceExtractorOptions> options, IImapService imapService)
        {
            _accounts = options.Value.Accounts;
            _bulkSize = options.Value.ProcessingBulkSize;
            _emails = new List<IInvoiceEmail>();
            _imapService = imapService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                ProcessEmails();
                await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
            }
        }

        public List<IInvoiceEmail> GetProcessedEmails()
        {
            var snapshot = new List<IInvoiceEmail>(_emails);
            _emails.Clear();
            return snapshot;
        }

        public void TriggerImmediateCheck()
        {
            ProcessEmails();
        }

        public int GetProcessedEmailCount()
        {
            return _emails.Count();
        }

        private void ProcessEmails()
        {
            foreach (var account in _accounts)
            {
                _emails.AddRange(_imapService.GetNewInvoiceEmails(account, DateTime.Now, _bulkSize ?? 10));
            }
        }
    }
}
