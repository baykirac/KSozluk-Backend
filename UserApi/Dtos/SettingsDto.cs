namespace UserApi.Dtos;

public class SettingsDto
{
    public string Key { get; set; }
    public string Value { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public long? UserId { get; set; }
    public long? CompanyId { get; set; }
}