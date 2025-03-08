namespace UserApi.Dtos;

public class RequestGetTokenDto
{
    public string Username { get; set; }

    public string Password { get; set; }

    public bool RetriesControl { get; set; }

    public bool CaptchaControl { get; set; }

    public int RetriesCount { get; set; }
}
