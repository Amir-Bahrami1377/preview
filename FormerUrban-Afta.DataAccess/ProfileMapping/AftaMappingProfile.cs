using DNTPersianUtils.Core;
using FormerUrban_Afta.DataAccess.DTOs.IdentityUser;
using FormerUrban_Afta.DataAccess.DTOs.Marahel;


namespace FormerUrban_Afta.DataAccess.ProfileMapping
{
    public class AftaMappingProfile : Profile
    {
        public AftaMappingProfile()
        {
            CreateMap<DateTime, string>().ConvertUsing(value => value.ToShortPersianDateString(true));
            CreateMap<string, DateTime>().ConvertUsing(value => value.ToGregorianDateTime(false, 1300) ?? DateTime.UtcNow.AddHours(3.5));
            CreateMap<DateTime?, string>().ConvertUsing(value => value.ToShortPersianDateString(true));
            CreateMap<string, DateTime?>().ConvertUsing(value => value.ToGregorianDateTime(false, 1300));

            #region Parvandeh
            CreateMap<Parvandeh, ParvandehDto>().ReverseMap();
            CreateMap<Melk, MelkDto>().ReverseMap();
            CreateMap<Sakhteman, SakhtemanDto>().ReverseMap();
            CreateMap<Apartman, ApartmanDto>().ReverseMap();
            CreateMap<Dv_savabegh, Dv_savabeghDTO>().ReverseMap();
            CreateMap<Dv_malekin, Dv_malekinDTO>().ReverseMap();
            #endregion

            #region Athentication and Authorization
            CreateMap<UserPermissionDto, PermissionDto>().ReverseMap();
            CreateMap<UserPermissionDto, UserPermission>();
            CreateMap<UserPermission, UserPermissionDto>();
            CreateMap<RolePermissionDto, RolePermission>().ReverseMap();
            CreateMap<CostumIdentityUser, CostumIdentityUserDto>().ReverseMap();
            CreateMap<CreateCostumIdentityUserDto, CostumIdentityUserDto>().ReverseMap();
            CreateMap<CostumIdentityUser, CreateCostumIdentityUserDto>().ReverseMap();
            CreateMap<CostumIdentityUser, UpdateCostumIdentityUserDto>().ReverseMap();
            #endregion

            #region Darkhast
            CreateMap<Darkhast, DarkhastDTO>().ReverseMap();
            CreateMap<Erja, KartableDTO>().ReverseMap();
            #endregion

            #region Dv_Karbari

            CreateMap<Dv_karbari, Dv_karbariDTO>().ReverseMap();

            #endregion

            #region Expert

            CreateMap<Expert, ExpertDto>().ReverseMap();

            #endregion

            #region Marahel
            CreateMap<ControlMap, ControlMapDto>().ReverseMap();
            CreateMap<Parvaneh, ParvanehDto>().ReverseMap();
            #endregion

            #region Estelam
            CreateMap<Estelam, EstelamDto>().ReverseMap();
            #endregion

            #region Tarifha
            CreateMap<Tarifha, SmsSettingDto>().ReverseMap();
            CreateMap<Tarifha, LoginSettingDto>().ReverseMap();
            CreateMap<Tarifha, PasswordSettingDto>().ReverseMap();
            #endregion

            #region Setting
            CreateMap<AllowedIPRangeDto, AllowedIPRange>()
                .AfterMap((src, dest) =>
                {
                    dest.FromDate = src.FromDate.ToGregorianDateTime(false, 1300) ?? DateTime.UtcNow.AddHours(3.5);
                    dest.ToDate = src.ToDate.ToGregorianDateTime(false, 1300);
                });

            CreateMap<AllowedIPRange, AllowedIPRangeDto>()
                .AfterMap((src, dest) =>
                {
                    dest.FromDate = src.FromDate.ToPersianDateTimeString("HH:mm:ss yyyy/MM/dd", true);
                    dest.ToDate = src.ToDate.ToPersianDateTimeString("HH:mm:ss yyyy/MM/dd", true);
                });

            CreateMap<BlockedIPRangeDto, BlockedIPRange>()
                .AfterMap((src, dest) =>
                {
                    dest.FromDate = src.FromDate.ToGregorianDateTime(false, 1300) ?? DateTime.UtcNow.AddHours(3.5);
                    dest.ToDate = src.ToDate.ToGregorianDateTime(false, 1300);
                });

            CreateMap<BlockedIPRange, BlockedIPRangeDto>()
                .AfterMap((src, dest) =>
                {
                    dest.FromDate = src.FromDate.ToPersianDateTimeString("HH:mm:ss yyyy/MM/dd", true);
                    dest.ToDate = src.ToDate.ToPersianDateTimeString("HH:mm:ss yyyy/MM/dd", true);
                });

            CreateMap<EventLogFilter, EventLogFilterDto>()
                .AfterMap((src, dest) =>
                {
                    dest.MustLoginBeLogged = src.MustLoginBeLogged == "true" ? true : false;
                    dest.LogBarayeGozarAzHaddeAstaneyeBohraneMomayezi = src.LogBarayeGozarAzHaddeAstaneyeBohraneMomayezi == "true" ? true : false;
                    dest.LogBarayeGozarAzHaddeAstaneyeHoshdareMomayezi = src.LogBarayeGozarAzHaddeAstaneyeHoshdareMomayezi == "true" ? true: false;
                    dest.LogBarayeHarGooneTalasheEhrazeHoviateChandGaneyeKarbar = src.LogBarayeHarGooneTalasheEhrazeHoviateChandGaneyeKarbar == "true" ? true : false;
                    dest.LogBarayeRaddeRamzeObour = src.LogBarayeRaddeRamzeObour == "true" ? true : false;
                });
            CreateMap<EventLogFilterDto, EventLogFilter>()
                .AfterMap((src, dest) =>
                {
                    dest.MustLoginBeLogged = src.MustLoginBeLogged == true ? "true" : "false";
                    dest.LogBarayeGozarAzHaddeAstaneyeBohraneMomayezi = src.LogBarayeGozarAzHaddeAstaneyeBohraneMomayezi == true ? "true" : "false";
                    dest.LogBarayeGozarAzHaddeAstaneyeHoshdareMomayezi = src.LogBarayeGozarAzHaddeAstaneyeHoshdareMomayezi == true ? "true" : "false";
                    dest.LogBarayeHarGooneTalasheEhrazeHoviateChandGaneyeKarbar = src.LogBarayeHarGooneTalasheEhrazeHoviateChandGaneyeKarbar == true ? "true" : "false";
                    dest.LogBarayeRaddeRamzeObour = src.LogBarayeRaddeRamzeObour == true ? "true" : "false";
                });
            CreateMap<EventLogThreshold, EventLogThresholdDto>().ReverseMap();
            CreateMap<UserLogined, UserLoginedDto>().ReverseMap();
            CreateMap<ActivityLogFilters, ActivityLogFiltersDto>().ReverseMap();
            //CreateMap<RoleRestriction, RoleRestrictionDto>().ReverseMap();

            CreateMap<RoleRestrictionDto, RoleRestriction>()
                .AfterMap((src, dest) =>
                {
                    dest.FromDate = src.FromDate.ToGregorianDateTime(false, 1300) ?? DateTime.UtcNow.AddHours(3.5);
                    dest.ToDate = src.ToDate.ToGregorianDateTime(false, 1300);
                });

            CreateMap<RoleRestriction, RoleRestrictionDto>()
                .AfterMap((src, dest) =>
                {
                    dest.FromDate = src.FromDate.ToPersianDateTimeString("HH:mm:ss yyyy/MM/dd", true);
                    dest.ToDate = src.ToDate.ToPersianDateTimeString("HH:mm:ss yyyy/MM/dd", true);
                });

            #endregion
            //CreateMap<Approvals, EstateViewModel>().AfterMap((src, dest) =>
            //{
            //    dest.ApprovalType = MyFunctions.GetDisplayEnumName(dest.ApprovalTypeEnum);
            //    dest.ApprovalId = src.Id;
            //});
        }
    }
}
