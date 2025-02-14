using MimeKit;

namespace EmailInvoiceExctractor.Models
{
    public class InvoiceEmail :IInvoiceEmail
    {
        public InvoiceEmail(MimeMessage message)
        {
            MessageId = message.MessageId;
            Sender = message.From.FirstOrDefault().ToString();
            Email = message.To.FirstOrDefault().ToString();
            Attachemnts = message.Attachments;
        }
    }
}
