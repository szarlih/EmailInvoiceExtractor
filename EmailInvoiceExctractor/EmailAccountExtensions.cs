using EmailInvoiceExctractor.Models;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;
using System.Net.Mime;

namespace EmailInvoiceExctractor
{
    public static class EmailAccountExtensions
    {

        public static int ScanForNewInvoices(this EmailAccount account, DateTime start, int bulkSize = 10)
        {
            using (var imapClient = new ImapClient())
            {
                imapClient.OpenInbox(account);

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
                            FlagBulk(bulk, account);
                            processedMessagesCount += bulk.Count;
                        }
                        catch (Exception ex)
                        {
                            //TODO: maybe add one by one processing, add global logger!

                            processedMessagesCount += bulkSize;
                            imapClient.OpenInbox(account);
                        }
                        
                    }
                    while (notPorcessedMessages.Count > processedMessagesCount);
                    
                }
                var list = GetMessages(matched, account);

                imapClient.Disconnect(true);

                return matched.Count;
            }
        }

        private static UniqueIdSet GetMessageIdsWithPdfAttachment(IList<IMessageSummary> messages)
        {
            var matched = new UniqueIdSet();
            matched.AddRange(
                messages
                .Where(m => m.BodyParts.Any(x => x.IsAttachment && x.ContentType.MimeType == MediaTypeNames.Application.Pdf))
                .Select(u => u.UniqueId));

            return matched;
        }

        private static void FlagBulk(IList<IMessageSummary> messages, EmailAccount account)
        {
            using (var imapClient = new ImapClient())
            {
                imapClient.OpenInbox(account, FolderAccess.ReadWrite);
                imapClient.Inbox.AddFlags(messages.Select(m => m.UniqueId).ToList(), MessageFlags.Flagged, true);
            }
        }

        private static IList<MimeMessage> GetMessages(UniqueIdSet uniqueIds, EmailAccount account)
        {
            IList<MimeMessage> messages = new List<MimeMessage>();
            using (var imapClient = new ImapClient())
            {
                imapClient.OpenInbox(account);
                foreach (var messageId in uniqueIds)
                {
                    var message = imapClient.Inbox.GetMessage(messageId);
                    messages.Add(message);
                }
            }

            return messages;
        }

        private static void GetInvoiceAttachments()
        {

        }
    }
}
