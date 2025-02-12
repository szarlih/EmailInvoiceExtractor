using EmailInvoiceExctractor.Models;

namespace EmailInvoiceExctractor
{
    public class EmailInvoiceExtractorOptions
    {
        public List<EmailAccount> Accounts { get; set; } = new();
        public int? ProcessingBulkSize { get; set; }
    }
}
