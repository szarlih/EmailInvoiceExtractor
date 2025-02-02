using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace EmailInvoiceExctractor
{
    public class ActiveAccounts : BackgroundService, IActiveAccounts
    {
        private readonly List<EmailAccount> _accounts;
        private List<int> _emails;

        public ActiveAccounts(IOptions<EmailInvoiceExtractorOptions> options)
        {
            _accounts = options.Value.Accounts;
            _emails = new List<int>();
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
                _emails.Add(account.ScanForNewInvoices(DateTime.Now));
            }
        }
    }
}
