using AutoMapper;
using UserApi.Models;
using UserApi.Dtos;
using UserApi.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Novell.Directory.Ldap;
using Ozcorps.Core.Encryptors;
using Ozcorps.Generic.Bll;
using Ozcorps.Generic.Dal;
using Ozcorps.Generic.Entity;
using Ozcorps.Logger;
using Ozcorps.Ozt;
using Ozcorps.Tools.Ldap;
using System.Linq.Expressions;
using System.Net.Mail;
using Ozcorps.Core.Models;
using Ozcorps.Core.Extensions;
using Ozcorps.Localization;
using System.Text.RegularExpressions;

namespace UserApi.Business;

public class UserApiService : DbServiceBase, IUserApiService
{
    private readonly IRepository<User> _UserRepository;
    private readonly IRepository<UserRole> _UserRoleRepository;
    private readonly IRepository<UserPermission> _UserPermissionRepository;
    private readonly IRepository<UserCompany> _UserCompanyRepository;
    private readonly IRepository<Company> _CompanyRepository;
    private readonly IRepository<Account> _AccountRepository;
    private readonly IRepository<Settings> _SettingsRepository;
    private readonly IRepository<Role> _RoleRepository;
    private readonly IRepository<Permission> _PermissionRepository;
    private readonly IRepository<RolePermission> _RolePermissionRepository;
    private readonly IMapper _Mapper;
    private readonly IEncryptor _Encryptor;
    private readonly LdapOptions _LdapOptions;
    private readonly IOzLogger _Logger;
    private readonly OztTool _OztTool;
    private readonly IConfiguration _Config;
    private readonly IWebHostEnvironment _WebHostEnvironment;
    private readonly EmailTool _EmailTool;
    private readonly LocalizationTool _LocalizationTool;
    public EmailConfiguration Configuration { get; init; }

    public UserApiService(IUnitOfWork _unitOfWork,
        IMapper _mapper,
        IEncryptor _encryptor,
        IOptions<LdapOptions> _ldapOptions,
        IWebHostEnvironment _webHostEnvironment,
        IOzLogger _logger,
        EmailTool _emailTool,
        LocalizationTool _localizationTool,
        IConfiguration _config,
        IOptions<EmailConfiguration> _settings,
        OztTool _oztTool) : base(_unitOfWork)
    {
        _UserRepository = _unitOfWork.GetRepository<User>();

        _UserRoleRepository = _unitOfWork.GetRepository<UserRole>();

        _SettingsRepository = _unitOfWork.GetRepository<Settings>();

        _UserPermissionRepository = _unitOfWork.GetRepository<UserPermission>();

        _RoleRepository = _unitOfWork.GetRepository<Role>();

        _PermissionRepository = _unitOfWork.GetRepository<Permission>();

        _RolePermissionRepository = _unitOfWork.GetRepository<RolePermission>();

        _AccountRepository = _unitOfWork.GetRepository<Account>();

        _CompanyRepository = _unitOfWork.GetRepository<Company>();

        _UserCompanyRepository = _unitOfWork.GetRepository<UserCompany>();

        _Mapper = _mapper;

        _Encryptor = _encryptor;

        _LdapOptions = _ldapOptions?.Value;

        _Logger = _logger;

        _OztTool = _oztTool;

        _WebHostEnvironment = _webHostEnvironment;

        _EmailTool = _emailTool;

        _Config = _config;

        Configuration = _settings.Value;

        _LocalizationTool = _localizationTool;
    }

    public IEnumerable<User> GetAll() => _UserRepository.GetAll(x => !x.IsDeleted);

    public User Get(string _username)
    {
        var _user = _UserRepository.GetFirst(x => x.Username == _username);

        var _userDto = _Mapper.Map<User>(_user);

        return _userDto;
    }

    public User Add(UserDto _dto)
    {
        var _user = _Mapper.Map<User>(_dto);

        _user.SetModifiedEntity(_dto.UserId, _isInsert: true, _isActive: true);

        _user.Password = _Encryptor.Encrypt(_dto.Password);

        _UserRepository.Add(_user);

        Save();

        return _user;
    }

    public void ChangePassword(string _password, long _id)
    {
        var _user = _UserRepository.Get(_id);

        _user.SetModifiedEntity(_id);

        _user.Password = _Encryptor.Encrypt(_password);

        Save();
    }

    public User Update(UserDto _dto)
    {
        var _user = _UserRepository.Get(_dto.Id);

        _user.SetModifiedEntity(_dto.UserId, _isActive: _dto.IsActive);

        _user.Email = _dto.Email;

        _user.Name = _dto.Name;

        if (_dto.Password != "***")
        {
            _user.Password = _Encryptor.Encrypt(_dto.Password);
        }

        _user.Surname = _dto.Surname;

        _user.Username = _dto.Username;

        Save();

        return _user;
    }

    public void Remove(long _id, long _userId)
    {
        var _user = _UserRepository.Get(_id);

        _user.SetModifiedEntity(_userId, true);

        Save();
    }


    public List<long> GetCompanyIds(long _userId) =>
        _UserCompanyRepository.GetAll(x => x.UserId == _userId && !x.IsDeleted).
        Select(x => x.CompanyId).
        ToList();

    public IEnumerable<SettingsDto> GetCustomLdapConfig(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException("Username must be non-empty.", nameof(username));
        }

        if (!username.Contains('/'))
        {
            var nonNumericConfigs = _SettingsRepository.GetAll()
                .Where(x => !x.Key.Any(char.IsDigit))
                .Select(x => new SettingsDto
                {
                    Key = x.Key,
                    Value = x.Value,
                })
                .OrderBy(x => x.Key)
                .ToList();

            return nonNumericConfigs;
        }

        string domainPart = username.Split('/').FirstOrDefault();
        if (string.IsNullOrWhiteSpace(domainPart))
        {
            throw new ArgumentException("Domain part cannot be empty.", nameof(username));
        }
        //Bu kısımda tüm kayıtları çekmek yerine başka bir kolondan vs filtreleme gerekebilir./cihan
        var allConfigs = _SettingsRepository.GetAll()
            .Where(x => x.Value != null && _Encryptor.Decrypt(x.Value) == domainPart)
            .ToList();

        if (!allConfigs.Any())
        {
            return null;
        }

        var domainConfig = allConfigs.FirstOrDefault();
        string numberInKey = new string(domainConfig.Key.Where(char.IsDigit).ToArray());

        if (string.IsNullOrWhiteSpace(numberInKey))
        {
            return null;
        }

        var relevantConfigs = _SettingsRepository.GetAll()
            .Where(x => x.Key.Contains(numberInKey))
            .Select(x => new SettingsDto
            {
                Key = x.Key,
                Value = x.Value,
            })
            .OrderBy(x => x.Key)
            .ToList();

        return relevantConfigs;
    }

    public ResponseValidateUserDto Validate(RequestGetTokenDto _dto,
        bool _ldap = false,
        string _language = "tr")
    {
        var _trimmedUsername = _dto.Username?.Split('/')?.LastOrDefault()?.Trim() ?? string.Empty;
        bool RetriesControl = _dto.RetriesControl;
        int RetriesCount = _dto.RetriesCount;
        var _ldapCof = GetCustomLdapConfig(_dto.Username);
        var _result = new ResponseValidateUserDto();

        User _user = null;

        var _isPasswordCorrect = false;
        var _ldapIpValue = "";
        var _ldapDomain = "";


        #region ldap kontrolü
        if (_ldap)
        {
            _user = _UserRepository.Get(x => x.Username == _trimmedUsername &&
                    !x.IsDeleted &&
                    x.IsActive && x.IsLdap);

            if (_ldapCof != null)
            {

                _ldapIpValue = _Encryptor.Decrypt(_ldapCof
      .FirstOrDefault(x => Regex.IsMatch(x.Key, @"^LdapIp(\d*)$"))?.Value);

                _ldapDomain = _Encryptor.Decrypt(_ldapCof
       .FirstOrDefault(x => Regex.IsMatch(x.Key, @"^LdapDomain(\d*)$"))?.Value);


            }
            else
            {
                Console.WriteLine("LDAP Config listesi boş döndü.");
            }

            using (var _conn = new LdapConnection())
            {
                try
                {
                    _conn.SecureSocketLayer = false;
                    // Linux secure portu kontrol edilmeli. 
                    int _ldapPort = LdapConnection.DefaultPort;

                    _conn.ConnectionTimeout = 15;

                    _conn.Connect(_ldapIpValue, _ldapPort);

                    _conn.Bind(_trimmedUsername + "@" + _ldapDomain, _dto.Password);

                    if (_conn.Bound)
                    {
                        _isPasswordCorrect = true;
                    }

                    _conn.Disconnect();
                }
                catch (LdapException _ex)
                {
                    if (_user != null)
                    {
                        _Logger.Error(_ex);
                    }

                    _conn.Disconnect();
                }

            }

            if (_user == null)
            {
                if (_isPasswordCorrect)
                {
                    _user = _UserRepository.Add(new User
                    {
                        Name = _dto.Username.Contains("@") ? _dto.Username.Split("@")[0].Split(".")[0] : _dto.Username.Split(".")[0],
                        Surname = (_dto.Username.Contains("@") ? _dto.Username.Split("@")[0].Split(".")[1] : _dto.Username.Split(".")[1]),
                        Email = (_dto.Username.Contains("@") ? _dto.Username : _dto.Username + "@" + _LdapOptions.Domain),
                        Username = _dto.Username,
                        InsertedUserId = 1,
                        IsActive = false,
                        IsDeleted = false,
                        IsLdap = true,
                    });

                    _user.SetModifiedEntity(1, _isInsert: true);


                    Save();
                    _result.Message = "Uygulamaya giriş yetkiniz bulunmamaktadır.Lütfen yöneticiniz ile irtbata geçiniz.";
                    return _result;
                }

            }
            else
            {
                _user.SetModifiedEntity(1);

                if (!_isPasswordCorrect)
                {
                    var _message = "Ldap kullanıcı adı veya şifre yanlış.";

                    // _result.IsPasswordCorrect = true;

                    if (RetriesControl)
                    {
                        if (_user.RetriesDate is null)
                        {
                            _user.Retries += 1;
                        }

                        if (!_dto.CaptchaControl)
                        {
                            if ((_user.Retries - 1) == RetriesCount)
                            {
                                if (_user.RetriesDate == null)
                                {
                                    _user.RetriesDate = DateTime.Now;
                                }

                                var _controlMinute = Convert.ToInt64(DateTime.Now.Subtract(_user.RetriesDate.Value).TotalMinutes);

                                if (_controlMinute >= 30)
                                {
                                    _user.Retries = 0;

                                    _user.RetriesDate = null;
                                }
                                else
                                {
                                    var _remainingMinute = Math.Abs(_controlMinute - 30);

                                    _message = $"Kullanıcınız {_remainingMinute}'DK boyunca kilitlenmiştir.";
                                }
                            }
                        }
                        else
                        {
                            if (_user.Retries == RetriesCount)
                            {
                                if (_user.RetriesDate == null)
                                {
                                    _user.RetriesDate = DateTime.Now;
                                }

                                var _controlMinute = Convert.ToInt64(DateTime.Now.Subtract(_user.RetriesDate.Value).TotalMinutes);

                                if (_controlMinute >= 30)
                                {
                                    _user.Retries = 0;

                                    _user.RetriesDate = null;
                                }
                                else
                                {
                                    var _remainingMinute = Math.Abs(_controlMinute - 30);

                                    _message = $"Kullanıcınız {_remainingMinute}'DK boyunca kilitlenmiştir.";
                                }
                            }
                        }

                        Save();

                    }
                    else
                    {
                        _user.Retries += 1;

                        Save();
                    }
                    _result.Retries = _user.Retries;
                    _result.Message = _message;

                    return _result;
                }
                else
                {
                    if (RetriesControl)
                    {

                        if (!_dto.CaptchaControl)
                        {
                            if ((_user.Retries - 1) == RetriesCount) // Giriş deneme sayısı
                            {
                                var _controlMinute = Convert.ToInt64(DateTime.Now.Subtract(_user.RetriesDate.Value).TotalMinutes);

                                if (_controlMinute >= 30)
                                {
                                    _user.Retries = 0;

                                    _user.RetriesDate = null;

                                    Save();
                                }
                                else
                                {
                                    var _remainingMinute = Math.Abs(_controlMinute - 30);

                                    _result.Message = $"Kullanıcınız {_remainingMinute}'DK boyunca kilitlenmiştir.";

                                    return _result;
                                }
                            }
                            else
                            {
                                if (_user.Retries > 0 && _user.Retries - 1 < RetriesCount)
                                {
                                    _user.Retries = 0;

                                    _user.RetriesDate = null;

                                    Save();
                                }
                            }
                        }
                        else
                        {
                            if (_user.Retries == RetriesCount) // Giriş deneme sayısı
                            {
                                var _controlMinute = Convert.ToInt64(DateTime.Now.Subtract(_user.RetriesDate.Value).TotalMinutes);

                                if (_controlMinute >= 30)
                                {
                                    _user.Retries = 0;

                                    _user.RetriesDate = null;

                                    Save();
                                }
                                else
                                {
                                    var _remainingMinute = Math.Abs(_controlMinute - 30);

                                    _result.Message = $"Kullanıcınız {_remainingMinute}'DK boyunca kilitlenmiştir.";

                                    return _result;
                                }
                            }
                            else
                            {
                                if (_user.Retries > 0 && _user.Retries < RetriesCount)
                                {
                                    _user.Retries = 0;

                                    _user.RetriesDate = null;

                                    Save();
                                }
                            }
                        }

                    }
                    _user.Retries = 0;
                    Save();
                }
            }
        }
        #endregion

        if (_user == null)
        {
            _user = _UserRepository.Get(x => x.Username == _dto.Username &&
               !x.IsDeleted &&
               x.IsActive);

            if (_user == null)
            {
                _result.Message = "Kullanıcı bulunamadı.";

                return _result;
            }
            _user.SetModifiedEntity(1);

            if (_Encryptor.Decrypt(_user.Password) != _dto.Password)
            {
                var _message = "Kullanıcı adı veya şifre yanlış.";

                if (RetriesControl)
                {
                    if (_user.RetriesDate is null)
                    {
                        _user.Retries += 1;
                    }

                    if (!_dto.CaptchaControl)
                    {

                        if (_user.Retries - 1 == RetriesCount)
                        {
                            if (_user.RetriesDate == null)
                            {
                                _user.RetriesDate = DateTime.Now;
                            }

                            var _controlMinute = Convert.ToInt64(DateTime.Now.Subtract(_user.RetriesDate.Value).TotalMinutes);

                            if (_controlMinute >= 30)
                            {

                                _user.Retries = 0;

                                _user.RetriesDate = null;
                            }
                            else
                            {
                                var _remainingMinute = Math.Abs(_controlMinute - 30);

                                _message = $"Kullanıcınız {_remainingMinute}'DK boyunca kilitlenmiştir.";
                            }
                        }
                    }
                    else
                    {
                        if (_user.Retries == RetriesCount)
                        {
                            if (_user.RetriesDate == null)
                            {
                                _user.RetriesDate = DateTime.Now;
                            }

                            var _controlMinute = Convert.ToInt64(DateTime.Now.Subtract(_user.RetriesDate.Value).TotalMinutes);

                            if (_controlMinute >= 30)
                            {

                                _user.Retries = 0;

                                _user.RetriesDate = null;
                            }
                            else
                            {
                                var _remainingMinute = Math.Abs(_controlMinute - 30);

                                _message = $"Kullanıcınız {_remainingMinute}'DK boyunca kilitlenmiştir.";
                            }
                        }
                    }

                    Save();
                    // _result.IsPasswordCorrect = true;

                }
                //_result.Message = _LocalizationTool.Get(_language, "err_001_001_003");
                else
                {
                    _user.Retries += 1;

                    Save();
                }
                _result.Retries = _user.Retries;
                _result.Message = _message;

                return _result;
            }
            else
            {

                if (RetriesControl)
                {
                    if (!_dto.CaptchaControl)
                    {
                        if ((_user.Retries - 1) == RetriesCount) // Giriş deneme sayısı
                        {
                            var _controlMinute = Convert.ToInt64(DateTime.Now.Subtract(_user.RetriesDate.Value).TotalMinutes);

                            if (_controlMinute >= 30)
                            {
                                _user.Retries = 0;

                                _user.RetriesDate = null;

                                Save();
                            }
                            else
                            {
                                var _remainingMinute = Math.Abs(_controlMinute - 30);

                                _result.Message = $"Kullanıcınız {_remainingMinute}'DK boyunca kilitlenmiştir.";

                                return _result;
                            }
                        }
                        else
                        {
                            if (_user.Retries > 0 && _user.Retries - 1 < RetriesCount)
                            {
                                _user.Retries = 0;

                                _user.RetriesDate = null;

                                Save();
                            }
                        }
                    }
                    else
                    {
                        if (_user.Retries == RetriesCount) // Giriş deneme sayısı
                        {
                            if (_user.RetriesDate.HasValue)
                            {
                                var _controlMinute = Convert.ToInt64(
                                    DateTime.Now.Subtract(
                                        _user.RetriesDate.Value).TotalMinutes);

                                if (_controlMinute >= 30)
                                {
                                    _user.Retries = 0;

                                    _user.RetriesDate = null;

                                    Save();
                                }
                                else
                                {
                                    var _remainingMinute = Math.Abs(_controlMinute - 30);

                                    _result.Message = $"Kullanıcınız {_remainingMinute}'DK boyunca kilitlenmiştir.";

                                    return _result;
                                }
                            }
                        }
                        else
                        {
                            if (_user.Retries > 0 && _user.Retries < RetriesCount)
                            {
                                _user.Retries = 0;

                                _user.RetriesDate = null;

                                Save();
                            }
                        }
                    }
                }
                else
                {
                    _user.Retries = 0;

                    Save();
                }
            }

        }

        if (_user == null)
        {
            var existingUserRole = _UserRoleRepository.Get(x => x.UserId == _user.Id && x.RoleId == 2);

            if (existingUserRole == null)
            {
                var _userRole = new UserRole
                {
                    UserId = _user.Id,
                    RoleId = 2,
                    InsertedUserId = 1,
                    IsActive = true,
                    IsDeleted = false
                };

                _UserRoleRepository.Add(_userRole);
                Save();
            }

            var existingUserPermission = _UserPermissionRepository.Get(x => x.UserId == _user.Id && x.PermissionId == 2);

            if (existingUserPermission == null)
            {
                var _userPermission = new UserPermission
                {
                    UserId = _user.Id,
                    PermissionId = 2,
                    InsertedUserId = 1,
                    IsActive = true,
                    IsDeleted = false
                };

                _UserPermissionRepository.Add(_userPermission);
                Save();
            }

        }
        
        var _roles = _UserRoleRepository.GetAll(x =>
                x.UserId == _user.Id && !x.IsDeleted && x.IsActive).
            ToList().
            Join(_RoleRepository.GetAll(x => !x.IsDeleted && x.IsActive),
                x => x.RoleId,
                y => y.Id,
                (x, y) => new
                {
                    Role = y,
                    permission = (string)null
                });
        var _permission = _roles.
             Join(_RolePermissionRepository.GetAll(x => !x.IsDeleted && x.IsActive),
                 ur => ur.Role.Id,
                 rp => rp.RoleId,
                 (ur, rp) => new
                 {
                     ur.Role,
                     RolePermission = rp
                 }).
             Join(_PermissionRepository.GetAll(x => !x.IsDeleted && x.IsActive),
                 rpu => rpu.RolePermission.PermissionId,
                 p => p.Id,
                 (rpu, p) => new
                 {
                     rpu.Role,
                     Permission = p
                 }).
             Select(x => new
             {
                 x.Permission,
                 x.Role
             });

        var _userPermissions = _UserPermissionRepository.GetAll(x =>
                x.UserId == _user.Id && !x.IsDeleted && x.IsActive).
            ToList().
            Join(_PermissionRepository.GetAll(x => !x.IsDeleted && x.IsActive),
                up => up.PermissionId,
                p => p.Id,
                (rpu, p) => new
                {
                    Permission = p
                }).
            Select(x => x.Permission).ToList();

        _userPermissions.AddRange(_permission.Select(x => x.Permission).ToList());

        _result.Permissions = _userPermissions.Distinct().ToList();

        _result.Roles = _roles.Select(x => x.Role).Distinct().ToList();

        _result.User = _user;

        _result.Success = true;

        return _result;

    }

    public bool Any(Expression<Func<User, bool>> _predicate) =>
        _UserRepository.Any(_predicate);

    public User GetFirst(Expression<Func<User, bool>> _predicate) =>
        _UserRepository.GetFirst(_predicate);

    public void ResetPassword(string _email, long _userId)
    {
        var _token = _OztTool.GenerateToken(new OztUser
        {
            Email = _email,
            UserId = _userId,
            Roles = new List<string>() { "reset-pass" }
        });

        SendResetPasswordEmail(_email, _token);
    }

    // public void ResetPassword(string _email, long _userId)
    // {
    //     _email = "ogunozan@gmail.com";

    //     using (var _emailMessage = new MailMessage(new MailAddress(Configuration.From, "Başarsoft"), new MailAddress(_email)))
    //     {
    //         var _html = File.ReadAllText($"{_WebHostEnvironment.ContentRootPath}/" +
    //         "reset.html");

    //         _emailMessage.Body = _html.Replace("{ResetUrl}", "test")
    //             .Replace("{Email}", _email)
    //             .Replace("{CurrentYear}", DateTime.Now.Year.ToString());

    //         _emailMessage.IsBodyHtml = true;

    //         _emailMessage.Subject = "test";

    //         using (var _smtpClient = new SmtpClient(Configuration.SmtpServer, Configuration.Port))
    //         {
    //             _smtpClient.UseDefaultCredentials = false;
    //             _smtpClient.Credentials = new System.Net.NetworkCredential(Configuration.UserName, Configuration.Password);
    //             _smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
    //             _smtpClient.EnableSsl = true;
    //             _smtpClient.Send(_emailMessage);
    //         }
    //     }
    // }

    private void SendResetPasswordEmail(string _to, string _token)
    {
        var _html = File.ReadAllText($"{_WebHostEnvironment.ContentRootPath}/" +
            "reset-password.html");

        var _resetLink = _Config["WebUrl"] + "/reset-password?token=" + _token;

        _html = _html.Replace("[reset-link]", _resetLink);

        _html = _html.Replace("[app-name]", "Başarsoft Core App");

        _html = _html.Replace("[url]", _Config["WebUrl"]);

        var _inlineLogo = new LinkedResource(
            $"{_WebHostEnvironment.ContentRootPath}/images/logo.png",
            "image/png")
        {
            ContentId = Guid.NewGuid().ToString()
        };

        _html = _html.Replace("[logo]", _inlineLogo.ContentId);

        var _view = AlternateView.CreateAlternateViewFromString(_html, null, "text/html");

        _view.LinkedResources.Add(_inlineLogo);

        var _email = new EmailMessage(_EmailTool.Configuration.From,
            _to,
            "Şifre Sıfırlama",
            _view);

        _EmailTool.SendEmail(_email);
    }

    public IEnumerable<UserRoleDescription> GetUserRoles(long _userId)
    {
        var userRoles = _UserRoleRepository
            .GetAll(x => x.UserId == _userId && !x.IsDeleted && x.IsActive)
            .ToList();
        var roles = _RoleRepository
            .GetAll()
            .ToList();
        var userRoleDescriptions = userRoles
            .Join(roles, ur => ur.RoleId, r => r.Id, (ur, r) => new UserRoleDescription
            {
                RoleId = r.Id,
                RoleName = r.Name,
            });

        return userRoleDescriptions;
    }

    public PaginatorResponseDto<UserDto> Paginate(PaginatorDto _dto, List<long> _companyIds, long _userId)
    {
        var accountIds = _UserCompanyRepository.
        GetAll(x => x.IsDeleted == false).
        GroupBy(x => x.UserId).
        Where(g => g.Count() > 1).Select(g => g.Key)
        .ToList();

        var usersWithCompanies = _UserRepository.GetAll(x => x.IsDeleted == false && x.Id != _userId).ToList()
                    .GroupJoin(_UserCompanyRepository.GetAll(),
                               user => user.Id,
                               userCompany => userCompany.UserId,
                               (user, userCompanies) => new { User = user, UserCompanies = userCompanies })
                    .SelectMany(
                        userWithCompanies => userWithCompanies.UserCompanies.DefaultIfEmpty(),
                        (user, userCompany) => new { User = user.User, UserCompany = userCompany })
                        .Where(userWithCompany => (
                            (userWithCompany.UserCompany is not null ? (userWithCompany.UserCompany.IsDeleted == false && !accountIds.Contains(userWithCompany.UserCompany.UserId)) : true)
                             && (_companyIds is not null ? (userWithCompany.UserCompany != null && _companyIds.Contains(userWithCompany.UserCompany.CompanyId)) : true)))
                        .GroupJoin(_CompanyRepository.GetAll(),
                               userCompany => userCompany?.UserCompany?.CompanyId ?? -1,
                               company => company.Id,
                               (userCompany, companies) => new { User = userCompany.User, Companies = companies.FirstOrDefault() }).
                               Select(x => new UserDto
                               {
                                   Id = x.User.Id,
                                   Name = x.User.Name,
                                   Username = x.User.Username,
                                   Surname = x.User.Surname,
                                   Email = x.User.Email,
                                   IsActive = x.User.IsActive,
                                   InsertedDate = x.User.InsertedDate,
                                   ModifiedDate = x.User.ModifiedDate,
                                   CompanyId = x.Companies != null ? x.Companies.Id : 0,
                                   CompanyName = x.Companies != null ? x.Companies.Name : "Şirket Yok"
                               });

        var groupedUsers = usersWithCompanies
            .GroupBy(u => new { u.Id, u.Username, u.Name, u.Surname, u.IsActive, u.Email, u.InsertedDate, u.ModifiedDate })
            .Select(g => new
                    UserDto
            {
                Id = g.Key.Id,
                Name = g.Key.Name,
                Surname = g.Key.Surname,
                IsActive = g.Key.IsActive,
                Email = g.Key.Email,
                Username = g.Key.Username,
                InsertedDate = g.Key.InsertedDate,
                ModifiedDate = g.Key.ModifiedDate,
                CompanyId = g.Select(x => x.CompanyId).ToList().Count > 1 ? -1 : g.Select(x => x.CompanyId).FirstOrDefault(),
                CompanyName = string.Join(", ", g.Select(x => x.CompanyName).ToList())
            });

        var _rows = groupedUsers.
            OrderByDescending(x => x.Id).
            AsQueryable().
            Paginate(_dto, out int _count).
            ToListDynamic<UserDto>().
            ToList();

        return new PaginatorResponseDto<UserDto>
        {
            Count = _count,
            Rows = _rows
        };
    }

}
