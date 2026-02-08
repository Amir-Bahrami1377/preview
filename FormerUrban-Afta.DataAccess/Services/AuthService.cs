using FormerUrban_Afta.DataAccess.DTOs.IdentityUser;
using FormerUrban_Afta.DataAccess.DTOs.Login;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FormerUrban_Afta.DataAccess.Services
{
    public class AuthService : IAuthService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly UserManager<CostumIdentityUser> _userManager;
        private readonly SignInManager<CostumIdentityUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly RoleManager<CostumIdentityRole> _roleManager;
        private readonly IUserSessionService _userSessionService;
        private readonly IMapper _mapper;
        private readonly FromUrbanDbContext _context;
        //private readonly IRolePermissionService _rolePermissionService;


        public AuthService(UserManager<CostumIdentityUser> userManager, SignInManager<CostumIdentityUser> signInManager,
            IHttpContextAccessor httpContextAccessor, RoleManager<CostumIdentityRole> roleManager,
            IUserSessionService userSessionService, IServiceProvider serviceProvider, IMapper mapper, FromUrbanDbContext context /*, IRolePermissionService rolePermissionService*/)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _roleManager = roleManager;
            _userSessionService = userSessionService;
            _serviceProvider = serviceProvider;
            _mapper = mapper;
            _context = context;
            //_rolePermissionService = rolePermissionService;
        }

        #region Encryption

        //public CostumIdentityUser DecryptInfo(CostumIdentityUser entity)
        //{
        //    if (!string.IsNullOrWhiteSpace(entity.PhoneNumber))
        //    {
        //        var splitedData = entity.PhoneNumber.Split(' ');
        //        entity.PhoneNumber = EncryptionService.Decrypt(splitedData[0], splitedData[1], _shit);
        //    }
        //    if (!string.IsNullOrWhiteSpace(entity.Email))
        //    {
        //        var splitedData = entity.Email.Split(' ');
        //        entity.Email = EncryptionService.Decrypt(splitedData[0], splitedData[1], _shit);
        //    }
        //    return entity;
        //}

        //public CostumIdentityUser EncryptInfo(CostumIdentityUser entity)
        //{
        //    if (entity.PhoneNumber != null)
        //        entity.PhoneNumber = EncryptionService.Encrypt(entity.PhoneNumber, _shit);

        //    if (entity.Email != null)
        //        entity.Email = EncryptionService.Encrypt(entity.Email, _shit);

        //    return entity;
        //}

        #endregion

        public async Task<AuthResponse> SmsLogin(AuthRequest request)
        {
            var response = new AuthResponse();
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user?.UserName == null)
                return response.IsFailed(Strings.Persian.Fields.BadLogin);

            var roleRestrictionService = _serviceProvider.GetRequiredService<IRoleRestrictionService>();
            var isRestricted = await roleRestrictionService.IsUserRestricted(user.Id);
            if (isRestricted)
                return response.IsFailed(Strings.Persian.Fields.BadLoginRole);

            var isReachedMaxLoginFails = await CheckUserReachedMaxLogins(user);
            if (isReachedMaxLoginFails)
            {
                var eventLogFilterService = _serviceProvider.GetRequiredService<IEventLogFilterService>();
                var historyLogService = _serviceProvider.GetRequiredService<IHistoryLogService>();

                if (eventLogFilterService.Get().LogBarayeHarGooneTalasheEhrazeHoviateChandGaneyeKarbar)
                    historyLogService.PrepareForInsert($"ورود ناموفق بدلیل {request.UserName} رسیدن به سقف تلاش های ناموفق برای ورود", EnumFormName.UserLogined, EnumOperation.Post);
                var tarifhaService = _serviceProvider.GetRequiredService<ITarifhaService>();
                var tarifha = await tarifhaService.GetLoginSetting();
                await _userManager.SetLockoutEnabledAsync(user, false);
                await _userManager.SetLockoutEndDateAsync(user, DateTime.UtcNow.AddHours(3.5).AddMinutes(Convert.ToInt32(tarifha.UnblockingUserTime)));

                return response.IsFailed(Strings.Persian.Fields.BadLoginRepeat);
            }

            var allowUserNewSession = await _userSessionService.CheckUserMaxActiveSessions(user);
            if (!allowUserNewSession)
                return response.IsFailed(Strings.Persian.Fields.BadLoginSession);

            if (user.LockoutEnabled == false)
                return response.IsFailed(Strings.Persian.Fields.UserIsDisabled);

            await _userSessionService.AddNew(user);
            await _signInManager.SignInAsync(user, isPersistent: false);
            await _userManager.ResetAccessFailedCountAsync(user);

            return response.IsSuccess();
        }

        public async Task<AuthResponse> CheckLogin(AuthRequest request)
        {
            var response = new AuthResponse();

            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user?.UserName == null)
                return response.IsFailed(Strings.Persian.Fields.BadLogin);

            if (!CipherService.IsEqual(user.ToString(), user.Hashed))
            {
                //یوزر نباید بلاک شود
                return response.IsFailed(Strings.Persian.Fields.BadLoginHash);
            }

            if (!user.LockoutEnabled)
            {
                if (user.LockoutEnd != null && user.LockoutEnd < DateTime.UtcNow.AddHours(3.5))
                {
                    user.LockoutEnd = null;
                    user.LockoutEnabled = true;
                    user.AccessFailedCount = 0;
                    await _userManager.UpdateAsync(user);
                }
                else
                {
                    return response.IsFailed(Strings.Persian.Fields.BadLoginUserDeactive);
                }
            }

            // چک کردن تعداد دفعات تلاش ناموفق برای ورود
            var isReachedMaxLoginFails = await CheckUserReachedMaxLogins(user);
            if (isReachedMaxLoginFails)
            {
                var eventLogFilterService = _serviceProvider.GetRequiredService<IEventLogFilterService>();
                var historyLogService = _serviceProvider.GetRequiredService<IHistoryLogService>();
                var tarifhaService = _serviceProvider.GetRequiredService<ITarifhaService>();
                var tarifha = await tarifhaService.GetLoginSetting();
                if (eventLogFilterService.Get().LogBarayeHarGooneTalasheEhrazeHoviateChandGaneyeKarbar)
                    historyLogService.PrepareForInsert($"ورود ناموفق بدلیل {request.UserName} رسیدن به سقف تلاش های ناموفق برای ورود", EnumFormName.UserLogined, EnumOperation.Post);

                await _userManager.SetLockoutEnabledAsync(user, false);
                await _userManager.SetLockoutEndDateAsync(user, DateTime.UtcNow.AddHours(3.5).AddMinutes(Convert.ToInt32(tarifha.UnblockingUserTime)));

                return response.IsFailed(Strings.Persian.Fields.BadLoginRepeat);
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!passwordValid)
            {
                user.AccessFailedCount++;
                await _userManager.UpdateAsync(user);
                return response.IsFailed(Strings.Persian.Fields.BadLogin);
            }

            var roleRestrictionService = _serviceProvider.GetRequiredService<IRoleRestrictionService>();
            var isRestricted = await roleRestrictionService.IsUserRestricted(user.Id);
            if (isRestricted)
                return response.IsFailed(Strings.Persian.Fields.BadLoginRole);

            var allowUserNewSession = await _userSessionService.CheckUserMaxActiveSessions(user);
            if (!allowUserNewSession)
                return response.IsFailed(Strings.Persian.Fields.BadLoginSession);

            return response.IsFailed(user.TwoFactorEnabled ? "TwoFactorEnabled" : "SmsEnabled");
        }

        public async Task<AuthResponse> OtpLogin(AuthRequest request)
        {
            var response = new AuthResponse();

            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user?.UserName == null)
                return response.IsFailed(Strings.Persian.Fields.BadLogin);

            var roleRestrictionService = _serviceProvider.GetRequiredService<IRoleRestrictionService>();
            var isRestricted = await roleRestrictionService.IsUserRestricted(user.Id);
            if (isRestricted)
                return response.IsFailed(Strings.Persian.Fields.BadLoginRole);

            var isReachedMaxLoginFails = await CheckUserReachedMaxLogins(user);
            if (isReachedMaxLoginFails)
            {
                var eventLogFilterService = _serviceProvider.GetRequiredService<IEventLogFilterService>();
                var historyLogService = _serviceProvider.GetRequiredService<IHistoryLogService>();

                if (eventLogFilterService.Get().LogBarayeHarGooneTalasheEhrazeHoviateChandGaneyeKarbar)
                    historyLogService.PrepareForInsert($"ورود ناموفق بدلیل {request.UserName} رسیدن به سقف تلاش های ناموفق برای ورود", EnumFormName.UserLogined, EnumOperation.Post);

                var tarifhaService = _serviceProvider.GetRequiredService<ITarifhaService>();
                var tarifha = await tarifhaService.GetLoginSetting();
                await _userManager.SetLockoutEnabledAsync(user, false);
                await _userManager.SetLockoutEndDateAsync(user, DateTime.UtcNow.AddHours(3.5).AddMinutes(Convert.ToInt32(tarifha.UnblockingUserTime)));

                return response.IsFailed(Strings.Persian.Fields.BadLoginRepeat);
            }

            if (!await _userManager.VerifyTwoFactorTokenAsync(user, _userManager.Options.Tokens.AuthenticatorTokenProvider, request.Code))
                return response.IsFailed(Strings.Persian.Fields.BadLoginOtp);


            var allowUserNewSession = await _userSessionService.CheckUserMaxActiveSessions(user);
            if (!allowUserNewSession)
                return response.IsFailed(Strings.Persian.Fields.BadLoginSession);

            if (user.LockoutEnabled == false)
                return response.IsFailed(Strings.Persian.Fields.UserIsDisabled);

            await _userSessionService.AddNew(user);

            await _signInManager.SignInAsync(user, isPersistent: false);
            await _userManager.ResetAccessFailedCountAsync(user);
            return response.IsSuccess();
        }

        public async Task<CostumIdentityUser> GetCurentUserAsync()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userObject = await _userManager.GetUserAsync(user);
            return userObject ?? new CostumIdentityUser();
        }

        public CostumIdentityUser GetCurrentUser()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userObject = _userManager.GetUserAsync(user);
            return userObject.Result ?? new CostumIdentityUser();
        }

        public async Task<CostumIdentityUser> GetAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return user ?? new CostumIdentityUser();
        }

        public async Task<CostumIdentityUser> GetByUserNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            return user ?? new CostumIdentityUser();
        }

        public async Task<CostumIdentityUser> GetByUserNameAsNoTrackingAsync(string userName)
        {
            var user = await _userManager.Users.AsNoTracking().FirstOrDefaultAsync(x => x.UserName == userName);
            return user ?? new CostumIdentityUser();
        }

        public async Task<List<string>> GetRoleByUserIdAsync(string userId)
        {
            var user = await GetAsync(userId);
            return _userManager.GetRolesAsync(user).Result.ToList();
        }

        public async Task<IReadOnlyList<CostumIdentityRole>> GetAllRoleAsync() => await _roleManager.Roles.ToListAsync();

        public async Task<CostumIdentityRole> GetRoleByNameAsync(string name) => await _roleManager.FindByNameAsync(name) ?? new CostumIdentityRole();

        /// <summary>
        /// Get All User
        /// </summary>
        /// <returns></returns>
        public async Task<IReadOnlyList<CostumIdentityUser>> GetAllAsync() => await _userManager.Users.ToListAsync();

        public async Task<CostumIdentityUser> AddAsync(CostumIdentityUser entity)
        {
            IdentityResult result = await _userManager.CreateAsync(entity, entity.PasswordHash ?? "");
            if (result.Succeeded)
                return entity;

            string error = "";
            foreach (var item in result.Errors)
            {
                error += $"{item.Description}\n";
            }

            throw new ApplicationException(error);
        }

        public async Task<IdentityResult> CreateAsync(CostumIdentityUser entity) => await _userManager.CreateAsync(entity, entity.PasswordHash ?? "");

        async Task<IdentityResult> IAuthService.UpdateAsync(CostumIdentityUser entity)
        {
            // After password change or security-related updates
            var user = await _userManager.UpdateAsync(entity);
            await _userManager.UpdateSecurityStampAsync(entity);
            return user;
        }

        public async Task<bool> AddToRoleAsync(CostumIdentityUser user, string roleName)
        {
            await _userManager.AddToRoleAsync(user, roleName);
            return true;
        }

        public async Task<AuthResponse> CheckMobile(string mobile)
        {
            var response = new AuthResponse();
            var users = await _userManager.Users.ToListAsync();
            var user = users.FirstOrDefault(x => x.PhoneNumber == mobile);
            //var user = _userManager.Users.FirstOrDefault(x => x.PhoneNumber == mobile);
            if (user == null)
                return response.IsFailed("کاربری با این شماره موبایل یافت نشد!");
            if (!CipherService.IsEqual(user.ToString(), user.Hashed))
            {
                //یوزر نباید بلاک شود
                return response.IsFailed(Strings.Persian.Fields.BadLoginHash);
            }

            response.UserName = user?.UserName ?? "";
            return response.IsSuccess();
        }

        public async Task Logout(string userId)
        {
            await _userSessionService.UserSessionDeactivate(userId);
            await _signInManager.SignOutAsync();
        }

        public async Task<bool> CheckPassword(CostumIdentityUser user, string password) => await _userManager.CheckPasswordAsync(user, password);

        public async Task<bool> BlockUser(string userName)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user == null)
                    return false;
                user.LockoutEnabled = false;
                await _userManager.UpdateAsync(user);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        public async Task<bool> BlockUserByUserId(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return false;
                user.LockoutEnabled = false;
                await _userManager.UpdateAsync(user);
                await _userManager.UpdateSecurityStampAsync(user);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<AuthResponse> UnBlockUser(string userName)
        {
            var response = new AuthResponse();
            try
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user == null)
                    return response.IsFailed("کاربر مورد نظر یافت نشد!");

                //if (!user.TwoFactorEnabled)
                //    return response.IsFailed("ابتدا باید تایید دومرحله ای را فعال کنید!");

                user.LockoutEnabled = true;
                await _userManager.UpdateAsync(user);
                await _userManager.ResetAccessFailedCountAsync(user);
                await _userManager.UpdateSecurityStampAsync(user);
                return response.IsSuccess();
            }
            catch (Exception e)
            {
                return response.IsFailed(e.Message);
            }

        }

        public async Task<bool> CheckUserReachedMaxLogins(CostumIdentityUser user)
        {
            var count = await _userManager.GetAccessFailedCountAsync(user);

            var tarifhaService = _serviceProvider.GetRequiredService<ITarifhaService>();
            var tarifha = await tarifhaService.GetTarifhaNoLogAsync();

            if (string.IsNullOrWhiteSpace(tarifha.RetryLoginCount))
                tarifha.RetryLoginCount = "3";

            if (count >= Convert.ToInt32(tarifha.RetryLoginCount))
                return true;

            return false;
        }

        public async Task CreateDeveloperUser()
        {
            var user = await GetAllAsync();
            var existDeveloper = user.Any(a => a.UserName?.ToLower() == "developer");
            if (!existDeveloper)
            {
                var userDeveloper = new CostumIdentityUser()
                {
                    UserName = "developer",
                    Email = "developer@gmail.com",
                    Family = "مدیر",
                    Name = "آمارد",
                    EmailConfirmed = true,
                    PasswordHash = "Amard@123456",
                    PhoneNumber = "09383054926",
                };
                var result = await CreateAsync(userDeveloper);

                if (result.Succeeded)
                {
                    var newUser = await GetByUserNameAsync("developer");
                    if (newUser.UserName != null)
                    {
                        var role = await GetAllRoleAsync();
                        if (role.Count > 0)
                            await _userManager.AddToRolesAsync(newUser, role.Select(a => a.Name ?? ""));
                    }
                }
            }
        }

        public async Task SeedRolesAsync()
        {
            var roles = await GetAllRoleAsync();
            if (roles.Count == 0)
            {
                var newRoles = new List<CostumIdentityRole>
                {
                    new() { Name = "Administrator", Description = "مدیر" },
                    new() { Name = "Visit", Description = "مسئول بازدید" },
                    new() { Name = "IssuanceLicense", Description = "صدور پروانه" },
                    new() { Name = "ControlMap", Description = "کنترل نقشه" },
                    new() { Name = "Folder", Description = "پرونده" },
                    new() { Name = "InquiryResponse", Description = "پاسخ استعلام" },
                    new() { Name = "Archive", Description = "بایگانی" }
                };

                foreach (var item in newRoles)
                {
                    if (!await _roleManager.RoleExistsAsync(item.Name ?? ""))
                    {
                        item.NormalizedName = item.Name ?? "".ToUpper();
                        item.ConcurrencyStamp = Guid.NewGuid().ToString();
                        await _roleManager.CreateAsync(item);
                    }
                }

                var allEnumPermissions = Enum.GetValues(typeof(EnumPermission))
                    .Cast<EnumPermission>()
                    .Select(e => new RolePermissionDto
                    {
                        PermissionId = (int)e,
                        PermissionName = e.GetType()
                            .GetMember(e.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()?.Name ?? e.ToString(),
                        Access = true,
                    })
                .ToList();

                var role = await GetRoleByNameAsync("Administrator");

                var rolePermision = _mapper.Map<List<RolePermission>>(allEnumPermissions);
                rolePermision.ForEach(a => a.RoleId = role.Id);
                // rolePermision.ForEach(a => a.PermissionId = (int)a.Identity);
                rolePermision.ForEach(a => a.Identity = 0);

                var rolePermisions = await _context.RolePermission.Where(a => a.RoleId == role.Id).ToListAsync();
                _context.RolePermission.RemoveRange(rolePermisions);
                await _context.SaveChangesAsync();
                await _context.RolePermission.AddRangeAsync(rolePermision);
                await _context.SaveChangesAsync();
            }
        }
    }
}
