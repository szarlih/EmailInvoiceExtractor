using EmailInvoiceExctractor.Models;
using System.Xml;

namespace EmailInvoiceExctractor.Clients
{
    public interface IEmailClient
    {
        public IInvoiceEmail GetEmailsWithInvoices(DateTime startDate);
        public IInvoiceEmail GetEmailsWithInvoices(UniqueId lastProcessedMessage);
    }
}
