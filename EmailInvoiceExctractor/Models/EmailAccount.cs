namespace EmailInvoiceExctractor.Models
{
    public class EmailAccount
    {
        public string Server { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int Port { get; set; }
    }
}
