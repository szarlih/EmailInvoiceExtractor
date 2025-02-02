namespace EmailInvoiceExctractor
{
    public interface IEmailScrapper : IDisposable
    {
        public List<int> GetProcessedEmails();
        public void TriggerImmediateCheck();
        public int GetProcessedEmailCount();
    }
}
