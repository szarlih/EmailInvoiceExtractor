namespace EmailInvoiceExctractor
{
    public interface IActiveAccounts : IDisposable
    {
        public List<int> GetProcessedEmails();
        public void TriggerImmediateCheck();
        public int GetProcessedEmailCount();
    }
}
