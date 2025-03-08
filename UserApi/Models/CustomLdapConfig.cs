namespace UserApi.Models;

public class CustomLdapConfig
{
    public string Domain { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string LdapBase { get; set; }
    public string Ip { get; set; }
}