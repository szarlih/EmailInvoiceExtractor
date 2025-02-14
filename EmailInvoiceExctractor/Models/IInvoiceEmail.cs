using MimeKit;

namespace EmailInvoiceExctractor.Models
{
    public class IInvoiceEmail
    {
        public string MessageId { get; set; }
        public string Sender { get; set; }
        public string Email { get; set; }

        public IEnumerable<MimeEntity> Attachemnts { get; set; }
    }
}
