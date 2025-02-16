using EmailInvoiceExctractor.Models;

namespace EmailInvoiceExtractor.Services.Imap
{
    public interface IImapService
    {
        public IList<IInvoiceEmail> GetNewInvoiceEmails(EmailAccount account, DateTime start, int bulkSize = 10);
    }
}
