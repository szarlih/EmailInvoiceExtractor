using EmailInvoiceExctractor.Models;

namespace EmailInvoiceExctractor
{
    public class EmailInvoiceExtractorOptions
    {
        public List<EmailAccount> Accounts { get; set; } = new();
    }
}
