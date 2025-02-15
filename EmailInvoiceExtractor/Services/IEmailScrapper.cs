using EmailInvoiceExctractor.Models;

namespace EmailInvoiceExctractor.Services
{
    public interface IEmailScrapper : IDisposable
    {
        public List<IInvoiceEmail> GetProcessedEmails();
        public void TriggerImmediateCheck();
        public int GetProcessedEmailCount();
    }
}
