using FormerUrban_Afta.DataAccess.ProfileMapping;
using FormerUrban_Afta.DataAccess.Services;
using FormerUrban_Afta.DataAccess.Services.Sms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
namespace FormerUrban_Afta.DataAccess

{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<FromUrbanDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddAutoMapper(cfg => { }, typeof(AftaMappingProfile));
            services.AddScoped<IParvandehService, ParvandehService>();
            services.AddScoped<IMelkService, MelkService>();
            services.AddScoped<ITarifhaService, TarifhaService>();
            services.AddScoped<ISakhtemanService, SakhtemanService>();
            services.AddScoped<IApartmanService, ApartmanService>();
            services.AddScoped<IDv_malekinService, Dv_malekinService>();
            services.AddScoped<PermissionService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IHistoryLogService, HistoryLogService>();
            services.AddScoped<IEventLogThresholdService, EventLogThresholdService>();
            services.AddScoped<ISqlService, SqlService>();
            services.AddScoped<IDv_KarbariService, Dv_KarbariService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IUserPermissionService, UserPermissionService>();
            services.AddScoped<MyFunctions>();
            services.AddScoped<ISabethaService, SabethaService>();
            services.AddScoped<IDv_SavabeghService, Dv_SavabeghService>();
            services.AddScoped<IDarkhastService, DarkhastService>();
            services.AddScoped<IExpertService, ExpertService>();
            services.AddScoped<IErjaService, ErjaService>();
            services.AddScoped<IEstelamService, EstelamService>();
            services.AddScoped<ControlMapService>();
            services.AddScoped<TaeedErsalService>();
            services.AddScoped<IParvanehService, ParvanehService>();
            services.AddScoped<IWeakPasswordService, WeakPasswordService>();
            services.AddScoped<ExtensionUser>();
            services.AddScoped<ILogSMSService, LogSMSService>();
            services.AddScoped<MeliPayamakRestClientAsync>();
            services.AddScoped<MelipayamakSmsService>();
            services.AddHttpContextAccessor();
            services.AddScoped<IBrowserService, BrowserService>();
            services.AddScoped<IUserLoginedService, UserLoginedService>();
            services.AddScoped<IIpService, IpService>();
            services.AddScoped<IEventLogFilterService, EventLogFilterService>();
            services.AddScoped<IRolePermissionService, RolePermissionService>();
            services.AddScoped<IBlockedIPRange, IPRestrictionService>();
            services.AddScoped<IAllowedIPRange, IPRestrictionService>();
            services.AddScoped<ISendSmsService, SendSmsService>();
            services.AddScoped<IUserSessionService, UserSessionService>();
            services.AddScoped<IActivityLogFiltersService, ActivityLogFiltersService>();
            services.AddScoped<IRoleRestrictionService, RoleRestrictionService>();
            services.AddScoped<IAuditService, AuditService>();
            services.AddScoped<AuditService>();
            services.AddScoped<IAuditFilterService, AuditFilterService>();

            #region Validators
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            #endregion

            return services;
        }
    }
}
