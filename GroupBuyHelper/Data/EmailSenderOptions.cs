namespace GroupBuyHelper.Data
{
    public class EmailSenderOptions
    {
        public string Sender { get; set; }
        public string EmailAddress { get; set; }

        public string Host { get; set; }
        public int Port { get; set; }
        public bool UseSSL { get; set; }

        public string Login { get; set; }
        public string Password { get; set; }
    }
}