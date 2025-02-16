using EmailInvoiceExctractor.Models;

namespace EmailInvoiceExctractor.Settings
{
    public class EmailInvoiceExtractorOptions
    {
        public List<EmailAccount> Accounts { get; set; } = new();
        public int? ProcessingBulkSize { get; set; }
        public List<string> InvoiceKeywords { get; set; } = new();
    }
}
