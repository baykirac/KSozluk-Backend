using System.Linq.Expressions;
using Ozcorps.Core.Models;
using UserApi.Dtos;
using UserApi.Entities;

namespace UserApi.Business;

public interface IUserApiService
{
    ResponseValidateUserDto Validate(RequestGetTokenDto _dto,
        bool _ldap = false,
        string _language = "tr");

    bool Any(Expression<Func<User, bool>> _predicate);

    User GetFirst(Expression<Func<User, bool>> _predicate);
    IEnumerable<UserRoleDescription> GetUserRoles(long _userId);

    User Get(string _username);

    void ResetPassword(string _email, long _userId);

    void ChangePassword(string _password, long _id);

    List<long> GetCompanyIds(long _userId);

    IEnumerable<User> GetAll();

    PaginatorResponseDto<UserDto> Paginate(PaginatorDto _dto ,List<long> _companyIds,long _userId);
}