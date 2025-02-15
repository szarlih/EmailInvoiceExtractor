using iText.Kernel.Pdf;
using MimeKit;

namespace EmailInvoiceExctractor.Models
{
    public class IInvoiceEmail
    {
        public string MessageId { get; set; }
        public string Sender { get; set; }
        public string Email { get; set; }
        public MimePart Invoice { get; set; }
    }
}
