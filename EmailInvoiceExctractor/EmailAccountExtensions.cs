using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using System.Net.Mime;

namespace EmailInvoiceExctractor
{
    public static class EmailAccountExtensions
    {

        public static int ScanForNewInvoices(this EmailAccount account, DateTime start)
        {
            using (var imapClient = new ImapClient())
            {
                imapClient.Connect(account.Server, account.Port, MailKit.Security.SecureSocketOptions.SslOnConnect);
                imapClient.Authenticate(account.Address, account.Password);
                imapClient.Inbox.Open(FolderAccess.ReadOnly);

                var uids = imapClient.Inbox.Search(SearchQuery.All); // TODO: verify, add more conditions
                var matched = new UniqueIdSet();

                if (uids != null && uids.Count > 0)
                {
               /*     var items = MessageSummaryItems.BodyStructure | MessageSummaryItems.UniqueId;
                    foreach (var message in imapClient.Inbox.Fetch(uids, items))
                    {
                        if (message.BodyParts.Any(x => x.IsAttachment))
                        {
                            matched.Add(message.UniqueId);
                        }
                    }*/
                    matched.AddRange(uids);
                }

                imapClient.Disconnect(true);

                return uids.Count;
            }
        }

        private static UniqueIdSet GetMessageIdsWithPdfAttachment(IList<MessageSummary> messages)
        {
            var matched = new UniqueIdSet();

            foreach (var message in messages)
            {
                if (message.BodyParts.Any(x => x.IsAttachment && x.ContentType.Name == MediaTypeNames.Application.Pdf))
                {
                    matched.Add(message.UniqueId);
                }
            }

            return matched;
        }
    }
}
