namespace Moses.Services
{
    public interface IMailClient
    {
        string DisplayEmail { get; set; }
        string DisplayName { get; set; }
        string ReplyTo { get; set; }
        string CcList { get; set; }
        string CcoList { get; set; }
        bool? EnableSsl { get; set; }
        string Host { get; set; }
        int? Port { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
    }
}