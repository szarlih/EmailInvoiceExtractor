using EmailInvoiceExctractor.Models;
using MailKit;
using MailKit.Net.Imap;

namespace EmailInvoiceExtractor.Services.Imap
{
    public static class ImapClientExtensdions
    {
        public static void OpenInbox(this ImapClient imapClient, EmailAccount account, FolderAccess accessType = FolderAccess.ReadOnly)
        {
            if (!imapClient.IsConnected)
            {
                imapClient.Connect(account.Server, account.Port, MailKit.Security.SecureSocketOptions.SslOnConnect);
                imapClient.Authenticate(account.Address, account.Password);
            }

            imapClient.Inbox.Open(accessType);
        }
    }
}
