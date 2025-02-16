using EmailInvoiceExtractor.Services.Imap;
using MailKit;
using MimeKit;
using NSubstitute;
using Shouldly;

namespace EmailInvoiceExtractorTests
{
    public class MessagesTests
    {
        [Fact]
        public void GetMessageIdsWithPdfAttachment_Should_Returns_A_Set()
        {
            var imapService = new ImapInteractionsService();

            var messages = imapService.GetMessageIdsWithPdfAttachment(GetExampleMessages());

            messages.Count().ShouldBe(3);
        }

        private IList<IMessageSummary> GetExampleMessages()
        {
            IList<IMessageSummary> exampleMessages = new List<IMessageSummary>();

            for (uint i = 0; i < 6; i++)
            {
                var message = Substitute.For<IMessageSummary>();
                if (i < 3)
                {
                    var attachments = new List<BodyPartBasic>();
                    attachments.Add(new BodyPartMessage() { ContentType = ContentType.Parse("application/pdf"), ContentDisposition = new ContentDisposition("attachment") });
                    message.Attachments.Returns(attachments);
                    message.BodyParts.Returns(attachments);
                }

                message.UniqueId.Returns(new UniqueId(123+i));
                exampleMessages.Add(message);
            }

            return exampleMessages;
        }
    }
}
