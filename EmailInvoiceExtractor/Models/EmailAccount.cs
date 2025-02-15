using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using System.Net.Mime;

namespace EmailInvoiceExctractor.Models
{
    public class EmailAccount : IEmailAccount
    {
        public IList<IInvoiceEmail> GetNewInvoiceEmails(DateTime start, int bulkSize = 10)
        {
            using (var imapClient = new ImapClient())
            {
                imapClient.OpenInbox(this);

                var notPorcessedMessages = imapClient.Inbox.Search(SearchQuery.NotFlagged); // TODO: verify, add more conditions
                var matched = new UniqueIdSet();

                if (notPorcessedMessages != null && notPorcessedMessages.Count > 0)
                {
                    var items = MessageSummaryItems.BodyStructure | MessageSummaryItems.UniqueId;
                    int processedMessagesCount = 0;

                    do
                    {
                        try
                        {
                            var bulk = imapClient.Inbox.Fetch(notPorcessedMessages.Skip(processedMessagesCount).Take(bulkSize).ToList(), items);
                            matched.AddRange(GetMessageIdsWithPdfAttachment(bulk));
                            FlagBulk(bulk);
                            processedMessagesCount += bulk.Count;
                        }
                        catch (Exception ex)
                        {
                            //TODO: maybe add one by one processing, add global logger!

                            processedMessagesCount += bulkSize;
                            imapClient.OpenInbox(this);
                        }

                    }
                    while (notPorcessedMessages.Count > processedMessagesCount);

                }
                var list = GetMessages(matched);

                imapClient.Disconnect(true);

                return list;
            }
        }

        private UniqueIdSet GetMessageIdsWithPdfAttachment(IList<IMessageSummary> messages)
        {
            var matched = new UniqueIdSet();
            matched.AddRange(
                messages
                .Where(m => m.BodyParts.Any(x => x.IsAttachment && x.ContentType.MimeType == MediaTypeNames.Application.Pdf))
                .Select(u => u.UniqueId));

            return matched;
        }

        private void FlagBulk(IList<IMessageSummary> messages)
        {
            using (var imapClient = new ImapClient())
            {
                imapClient.OpenInbox(this, FolderAccess.ReadWrite);
                imapClient.Inbox.AddFlags(messages.Select(m => m.UniqueId).ToList(), MessageFlags.Flagged, true);
            }
        }

        private IList<IInvoiceEmail> GetMessages(UniqueIdSet uniqueIds)
        {
            IList<IInvoiceEmail> messages = new List<IInvoiceEmail>();
            using (var imapClient = new ImapClient())
            {
                imapClient.OpenInbox(this);
                foreach (var messageId in uniqueIds)
                {
                    try
                    {
                        var message = imapClient.Inbox.GetMessage(messageId);
                        messages.Add(new InvoiceEmail(message));
                    }
                    catch(ArgumentException aex)
                    {
                        // add logger later
                    }
                }
            }

            return messages;
        }
    }
}
