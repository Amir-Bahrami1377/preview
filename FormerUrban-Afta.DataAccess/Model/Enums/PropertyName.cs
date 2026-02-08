using System.ComponentModel;

namespace FormerUrban_Afta.DataAccess.Model.Enums
{
    public enum PropertyName
    {
        #region Erja

        [Description("شماره درخواست")]
        sh_darkhast,

        [Description("تاریخ درخواست")]
        tarikh_darkhast,

        [Description("کد نوع درخواست")]
        c_nodarkhast,

        [Description("نوع درخواست")]
        noedarkhast,

        [Description("کد مرحله")]
        c_marhaleh,

        [Description("مرحله")]
        marhaleh,

        [Description("کد نوسازی")]
        code_nosazi,

        [Description("شماره پرونده")]
        shop,

        [Description("نام متقاضی")]
        name_mot,

        [Description("تاریخ ارجاع")]
        tarikh_erja,

        [Description("کد ایجادکننده")]
        ijadkonandeh_c,

        [Description("ایجادکننده")]
        ijadkonandeh,

        [Description("کد ارسال‌کننده")]
        ersalkonandeh_c,

        [Description("ارسال‌کننده")]
        ersalkonandeh,

        [Description("کد گیرنده")]
        girandeh_c,

        [Description("گیرنده")]
        girandeh,

        [Description("وضعیت")]
        flag,

        [Description("ساعت ارجاع")]
        saat_erja,

        [Description("کد وضعیت ارجاع")]
        c_vaziatErja,

        [Description("وضعیت ارجاع")]
        vaziatErja,

        #endregion

        #region ActivityLogFilters

        [Description("نام فرم")]
        FormName,

        [Description("ایجاد")]
        AddStatus,

        [Description("ویرایش")]
        UpdateStatus,

        [Description("نمایش")]
        GetStatus,

        [Description("حذف")]
        DeleteStatus,

        #endregion

        #region AllowedIPRange

        [Description("رنج آی پی")]
        IPRange,

        [Description("توضیحات")]
        Description,

        [Description("تا تاریخ")]
        ToDate,

        [Description("از تاریخ")]
        FromDate,

        #endregion

        #region Apartman

        [Description("ردیف")]
        radif,

        [Description("پلاک آبی")]
        pelakabi,

        [Description("کد پستی")]
        codeposti,

        [Description("تلفن")]
        tel,

        [Description("آدرس")]
        address,

        [Description("کد نوع سند")]
        c_noesanad,

        [Description("نوع سند")]
        noesanad,

        [Description("کد وضعیت سند")]
        c_vazsanad,

        [Description("وضعیت سند")]
        vazsanad,

        [Description("کد نوع مالکیت")]
        c_noemalekiyat,

        [Description("نوع مالکیت")]
        noemalekiyat,

        [Description("شماره ثبت")]
        sabti,

        [Description("مساحت کل")]
        MasahatKol,

        [Description("مساحت عرصه")]
        MasahatArse,

        [Description("شماره درخواست")]
        sh_Darkhast,

        [Description("تفکیکی")]
        tafkiki,

        [Description("از فرعی")]
        azFari,

        [Description("فرعی")]
        fari,

        [Description("اصلی")]
        asli,

        [Description("بخش")]
        bakhsh,

        [Description("فعال")]
        Active,

        [Description("نوع سازه")]
        NoeSaze,

        [Description("کد نوع سازه")]
        c_NoeSaze,

        [Description("کد جهت")]
        C_Jahat,

        [Description("جهت")]
        jahat,

        #endregion

        #region Audit

        [Description("شناسه")]
        Id,

        [Description("فرم")]
        Form,

        [Description("شناسه رکورد")]
        EntityId,

        [Description("نوع عملیات")]
        Action,

        [Description("فیلد")]
        Field,

        [Description("مقدار اولیه")]
        OriginValue,

        [Description("مقدار جدید")]
        CurrentValue,

        [Description("تغییر دهنده")]
        ChangedBy,

        [Description("تاریخ تغییر")]
        ChangedAt,

        [Description("آی‌پی آدرس")]
        IpAddress,
        //BlockedIPRange  all columns were added before

        #endregion

        #region ControlMap

        [Description("مساحت طبق سند")]
        masahat_s,

        [Description("مساحت موجود")]
        masahat_m,

        [Description("مساحت اصلاحی")]
        masahat_e,

        [Description("مساحت باقی‌مانده")]
        masahat_b,

        [Description("کد نوع نما")]
        C_NoeNama,

        [Description("نوع نما")]
        NoeNama,

        [Description("کد نوع سقف")]
        c_noesaghf,

        [Description("نوع سقف")]
        noesaghf,

        [Description("تراکم")]
        tarakom,

        [Description("سطح اشغال")]
        satheshghal,

        [Description("تعداد طبقات")]
        TedadTabaghe,

        #endregion

        #region CostumIdentityUser

        [Description("نام")]
        Name,

        [Description("نام خانوادگی")]
        Family,

        [Description("شماره موبایل")]
        PhoneNumber,

        [Description("ایمیل")]
        Email,

        [Description("رمز عبور")]
        PasswordHash,

        #endregion

        #region Darkhast

        [Description("شماره درخواست")]
        shodarkhast,


        [Description("کد نوع درخواست")]
        c_noedarkhast,

        [Description("نوع متقاضی")]
        noemot,

        [Description("کد نوع متقاضی")]
        c_noemot,


        [Description("تلفن همراه")]
        mob,

        [Description("نام متقاضی")]
        moteghazi,

        [Description("ایمیل")]
        email,

        [Description("کد نوسازی")]
        c_nosazi,

        [Description("کد ملی")]
        CodeMeli,

        #endregion

        #region Dv_karbari

        [Description("ردیف درخواست")]
        d_radif,

        [Description("شناسه")]
        id,

        [Description("نام جدول")]
        mtable_name,

        [Description("کد طبقه")]
        c_tabagheh,

        [Description("نام طبقه")]
        tabagheh,

        [Description("کد کاربری")]
        c_karbari,

        [Description("کاربری")]
        karbari,

        [Description("کد نوع استفاده")]
        c_noeestefadeh,

        [Description("نوع استفاده")]
        noeestefadeh,

        [Description("مساحت")]
        masahat_k,

        [Description("کد نوع ساختمان")]
        c_noesakhteman,

        [Description("نوع ساختمان")]
        noesakhteman,

        [Description("کد نوع سازه")]
        c_noesazeh,

        [Description("نوع سازه")]
        noesazeh,

        [Description("تاریخ احداث")]
        tarikhehdas,

        #endregion

        #region Dv_malekin

        [Description("کد نوع مالک")]
        c_noemalek,

        [Description("نوع مالک")]
        noemalek,

        [Description("نام")]
        name,

        [Description("نام خانوادگی")]
        family,

        [Description("نام پدر")]
        father,

        [Description("شماره شناسنامه")]
        sh_sh,

        [Description("کد ملی")]
        kodemeli,

        [Description("سهم عرصه")]
        sahm_a,

        [Description("دانگ عرصه")]
        dong_a,

        [Description("سهم اعیان")]
        sahm_b,

        [Description("دانگ اعیان")]
        dong_b,

        [Description("ارزش عرصه")]
        ArzeshArse,

        [Description("ارزش اعیان")]
        ArzeshAyan,

        [Description("مقدار سهم عرصه")]
        meghdarsahmarse,

        [Description("مقدار سهم اعیان")]
        meghdarsahmayan,

        #endregion

        #region Dv_savabegh

        [Description("کد نوع")]
        c_noe,

        [Description("نوع")]
        noe,

        [Description("شماره درخواست")]
        sh_darkhast1,

        #endregion

        #region Estelam

        [Description("شماره درخواست")]
        Sh_Darkhast,

        [Description("شماره پاسخ")]
        Sh_Pasokh,

        [Description("تاریخ پاسخ")]
        Tarikh_Pasokh,

        [Description("دانگ انتقالی")]
        Dang_Enteghal,

        [Description("نام خریدار")]
        Kharidar,

        [Description("توضیحات")]
        Tozihat,

        [Description("کد نوع مالکیت")]
        codeNoeMalekiat,

        [Description("نوع مالکیت")]
        NoeMalekiat,

        #endregion

        #region EventLogFilter

        [Description("ثبت لاگ برای ورود کاربران")]
        MustLoginBeLogged,

        [Description("ثبت لاگ برای فعالیت کاربران")]
        MustActivityBeLogged,

        [Description("ثبت لاگ برای ورود داده‌ها")]
        LogBarayeVoroudeDade,

        [Description("ثبت لاگ برای استخراج داده‌ها")]
        LogBarayeEstekhrajeDade,

        [Description("ثبت لاگ برای رد رمز عبور")]
        LogBarayeRaddeRamzeObour,

        [Description("ثبت لاگ برای گذر از حد آستانه بحران ممیزی")]
        LogBarayeGozarAzHaddeAstaneyeBohraneMomayezi,

        [Description("ثبت لاگ برای گذر از حد آستانه هشدار ممیزی")]
        LogBarayeGozarAzHaddeAstaneyeHoshdareMomayezi,

        [Description("ثبت لاگ برای تلاش احراز هویت چندگانه کاربر")]
        LogBarayeHarGooneTalasheEhrazeHoviateChandGaneyeKarbar,

        #endregion

        #region EventLogThreshold

        [Description("آستانه هشدار ورود کاربران")]
        UsersLoginLogWarning,

        [Description("آستانه بحرانی ورود کاربران")]
        UsersLoginLogCritical,

        [Description("آستانه هشدار فعالیت کاربران")]
        UsersActivityLogWarning,

        [Description("آستانه بحرانی فعالیت کاربران")]
        UsersActivityLogCritical,

        [Description("آستانه هشدار لاگ تغییرات داده های سیستم")]
        UsersAuditsLogWarning,

        [Description("آستانه بحرانی لاگ تغییرات داده های سیستم")]
        UsersAuditsLogCritical,

        [Description("شناسه کاربر")]
        UserId,

        [Description("وضعیت ارسال پیامک هشدار ورود کاربران")]
        IsUserLoginLogWarningSmsSent,

        [Description("وضعیت ارسال پیامک هشدار فعالیت کاربران")]
        IsUserActivityLogWarningSmsSent,

        #endregion

        #region Expert

        [Description("شماره درخواست")]
        RequestNumber,

        [Description("تاریخ بازدید")]
        DateVisit,

        #endregion

        #region History

        [Description("شماره درخواست")]
        shod,

        [Description("نام کاربری")]
        user_name,

        [Description("تاریخ")]
        tarikh,

        [Description("ساعت")]
        saat,

        [Description("شرح عملیات")]
        sharh,

        [Description("نام کاربر")]
        name_karbar,

        [Description("نام فرم")]
        name_form,

        [Description("نوع عملیات")]
        noeamal,

        [Description("آدرس IP")]
        IPAddress,

        [Description("کد نوسازی")]
        CNosazi,

        #endregion

        #region LogSMS

        [Description("متن پیامک")]
        TextSMS,

        [Description("شماره موبایل")]
        MobileSMS,

        [Description("وضعیت پیامک")]
        StatusSMS,

        [Description("کد کاربر")]
        UserCode,

        [Description("تاریخ و زمان ارسال")]
        DateTimeSMS,

        [Description("هش شده")]
        Hashed,

        #endregion

        #region Melk

        [Description("کد وضعیت ملک")]
        c_vazmelk,

        [Description("وضعیت ملک")]
        vazmelk,

        [Description("کد محدوده")]
        c_mahdodeh,

        [Description("محدوده")]
        mahdodeh,

        [Description("کد نوع ملک")]
        c_noemelk,

        [Description("نوع ملک")]
        noemelk,

        [Description("کد کاربری اصلی")]
        C_karbariAsli,

        [Description("کاربری اصلی")]
        KarbariAsli,

        [Description("مختصات UTM-X")]
        utmx,

        [Description("مختصات UTM-Y")]
        utmy,

        #endregion

        #region Parvandeh

        [Description("منطقه")]
        mantaghe,

        [Description("حوزه")]
        hoze,

        [Description("بلوک")]
        blok,

        [Description("شماره ملک")]
        shomelk,

        [Description("شماره ساختمان")]
        sakhteman,

        [Description("شماره آپارتمان")]
        apar,

        [Description("شماره صنفی")]
        senfi,

        [Description("شناسه والد")]
        idparent,

        [Description("کد درختی")]
        code_tree,

        [Description("وضعیت SWS")]
        sws,

        [Description("فرمول")]
        Formol,

        [Description("کد نوسازی")]
        codeN,

        [Description("شناسه منطقه‌ای")]
        AreaId,

        [Description("قفل شده")]
        locked,

        #endregion

        #region Parvaneh

        [Description("شماره پروانه")]
        sho_parvaneh,

        [Description("کد نوع پروانه")]
        c_noeParvaneh,

        [Description("نوع پروانه")]
        noe_parvaneh,

        [Description("تاریخ صدور پروانه")]
        tarikh_parvaneh,

        [Description("مساحت مندرج با تراکم")]
        masahat_m_s_tarakom,

        [Description("مساحت مجاز زمین")]
        masahat_m_esh_zamin,

        [Description("تاریخ پایان عملیات ساختمانی")]
        tarikh_end_amaliat_s,

        [Description("شماره بیمه‌نامه")]
        sho_bimenameh,

        [Description("تاریخ اعتبار بیمه")]
        tarikh_e_bimeh,

        [Description("توضیحات پروانه")]
        tozihat_parvaneh,

        #endregion

        #region RolePermission

        [Description("شناسه نقش")]
        RoleId,

        [Description("شناسه دسترسی")]
        PermissionId,

        [Description("نقش")]
        Role,

        #endregion

        #region Sakhteman

        [Description("مساحت کل")]
        masahatkol,

        [Description("کد نوع نما")]
        c_noenama,

        [Description("نوع نما")]
        noenama,

        [Description("کد نوع ساختمان")]
        c_NoeSakhteman,

        [Description("تاریخ احداث")]
        TarikhEhdas,

        [Description("نوع ساختمان")]
        NoeSakhteman,

        [Description("مساحت زیربنا")]
        MasahatZirbana,

        #endregion

        #region Tarifha

        [Description("نام کاربری پیامک")]
        sms_user,

        [Description("رمز عبور پیامک")]
        sms_pass,

        [Description("شماره پیامک")]
        sms_shomare,

        [Description("تعداد دفعات مجاز تلاش ورود")]
        RetryLoginCount,

        [Description("حداکثر نشست‌های همزمان")]
        MaximumSessions,

        [Description("پایان نشست پس از (دقیقه)")]
        KhatemeSessionAfterMinute,

        [Description("حداکثر دفعات رد دسترسی")]
        MaximumAccessDenied,

        [Description("محدودیت نرخ درخواست‌ها")]
        RequestRateLimitter,

        [Description("طول رمز عبور")]
        PasswordLength,

        #endregion

        #region UserLogined

        [Description("آدرس آی‌پی")]
        Ip,

        [Description("زمان ورود")]
        LoginDateTime,

        [Description("زمان خروج")]
        LogoutDatetime,

        [Description("اطلاعات مرورگر (User Agent)")]
        UserAgent,

        [Description("نام کامل")]
        FullName,

        [Description("نام کاربری")]
        UserName,

        [Description("بیننده است؟")]
        IsViewer,

        #endregion

        #region UserPermission

        [Description("کاربر")]
        User,

        #endregion

        #region UserSession

        [Description("شناسه نشست")]
        SessionId,

        [Description("زمان ایجاد")]
        CreatedAt,

        [Description("آخرین فعالیت")]
        LastActivity,

        [Description("زمان انقضا")]
        ExpiresAt,

        #endregion

        #region VaultOptions

        [Description("آدرس")]
        Address,

        [Description("شناسه مخفی")]
        SecretId,

        [Description("نام کلید")]
        KeyName,

        [Description("مدت زمان اعتبار توکن (ثانیه)")]
        TokenTTLSeconds,

        #endregion

        #region WeakPassword

        [Description("متن هشدار رمز عبور ضعیف")]
        WeakPasswordText,

        [Description("وضعیت ارسال پیامک هشدار حجم لاگ")]
        IsAuditsLogWarningSmsSent,

        [Description("رفع مسدود بودن کاربر")]
        UnblockingUserTime

        #endregion

    }
}
