using System;

namespace KSozluk.WebAPI.DTOs.Request;

public class RequestUpdateUserRole
{
    public long newRoleAndPermissionId { get; set; }

    public long userId { get; set; }

}
