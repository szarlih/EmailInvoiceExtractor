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
            Invoice = GetInvoiceFromAttachments(message.Attachments);
        }

        private MimePart GetInvoiceFromAttachments(IEnumerable<MimeEntity> attachments)
        {
            var keywords = new List<string>() { "fv", "faktura" };

            if (attachments is null || attachments.Count() == 0)
            {
               throw new ArgumentException(nameof(attachments));
            }

            if (attachments.Count() == 1)
            {
                return attachments.First() as MimePart;
            }

            foreach (MimePart attachment in attachments)
            {
                if (keywords.Any(attachment.FileName.ToLower().Contains))
                {
                    return attachment;
                }
            }

            throw new ArgumentException("No invoice found");
        }
    }
}
