namespace FormerUrban_Afta.DataAccess.Services;

public class ErjaService : IErjaService
{
    private readonly FromUrbanDbContext _context;
    private readonly MyFunctions _myFunctions;
    private readonly IPermissionService _permissionService;
    private readonly IHistoryLogService _historyLogService;
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;

    public ErjaService(FromUrbanDbContext context, MyFunctions myFunctions, IPermissionService permissionService,
        IHistoryLogService historyLogService, IAuthService authService, IMapper mapper)
    {
        _context = context;
        _myFunctions = myFunctions;
        _permissionService = permissionService;
        _historyLogService = historyLogService;
        _authService = authService;
        _mapper = mapper;
    }

    public static bool CheckHash(Erja obj)
    {
        var hash = CipherService.IsEqual(obj.ToString(), obj.Hashed);
        return hash;
    }

    #region Get

    public double GetDataWithShopNoeDarkhastVaziatErja(int shop, int c_noeDarkhast, int c_vaziatErja)
    {
        return _context.Erja.Where(e => e.shop == shop && e.c_nodarkhast == c_noeDarkhast && e.c_vaziatErja == c_vaziatErja).Select(s => s.sh_darkhast).FirstOrDefault() ?? 0;
    }

    public List<KartableDTO> GetKartable()
    {
        try
        {
            var userData = _authService.GetCurrentUser();

            var data = _context.Erja.Where(w => w.girandeh_c == userData.Id && w.flag == true && w.c_vaziatErja == 1).OrderByDescending(c => c.sh_darkhast).ToList();

            // Map to DTOs and validate
            var results = data.Select(item =>
            {
                var dto = _mapper.Map<KartableDTO>(item);
                dto.IsValid = CheckHash(item);
                return dto;
            }).ToList();

            // Log invalid records in a batch
            var invalidRecords = results.Where(dto => !dto.IsValid).Select(dto => $"رد صحت سنجی داده کارتابل با شماه درخواست {dto.sh_darkhast}").ToList();

            if (invalidRecords.Any())
            {
                foreach (var message in invalidRecords)
                {
                    _historyLogService.PrepareForInsert(message, EnumFormName.Erja, EnumOperation.Validate);
                }
            }

            _historyLogService.PrepareForInsert($"مشاهده اطلاعات کارتابل", EnumFormName.Erja, EnumOperation.Get);

            return results;
        }
        catch (Exception e)
        {
            _historyLogService.PrepareForInsert($"خطا در دریافت اطلاعات کارتابل : {e.Message} ", EnumFormName.Erja, EnumOperation.Get);
            throw;
        }
    }

    public async Task<List<KartableDTO>> GetBaygani()
    {
        try
        {
            var userData = await _authService.GetCurentUserAsync();
            var data = await _context.Erja.Where(c => c.girandeh_c == userData.Id && c.flag == true && c.c_vaziatErja == 2).ToListAsync();

            // Map to DTOs and validate
            var results = data.Select(item =>
            {
                var dto = _mapper.Map<KartableDTO>(item);
                dto.IsValid = CheckHash(item);
                return dto;
            }).OrderByDescending(c => c.sh_darkhast).ToList();

            // Log invalid records in a batch
            var invalidRecords = results.Where(dto => !dto.IsValid).Select(dto => $"رد صحت سنجی داده کارتابل بایگانی با شماه درخواست {dto.sh_darkhast}").ToList();

            if (invalidRecords.Any())
            {
                foreach (var message in invalidRecords)
                {
                    _historyLogService.PrepareForInsert(message, EnumFormName.Erja, EnumOperation.Validate);
                }
            }

            // Log activity
            _historyLogService.PrepareForInsert($"مشاهده اطلاعات بایگانی پرونده ها.", EnumFormName.Baygani, EnumOperation.Get);

            return results;
        }
        catch (Exception e)
        {
            _historyLogService.PrepareForInsert($"خطا در دریافت اطلاعات کارتابل : {e.Message} ", EnumFormName.Erja, EnumOperation.Get);
            throw;
        }
    }

    public List<Erja> GetActiveData(int shod)
    {
        return _context.Erja.Where(c => c.sh_darkhast == shod && c.flag == true).ToList();
    }

    #endregion

    #region Update
    public bool UpdateData(List<Erja> erjas)
    {
        var currentUser = _authService.GetCurrentUser();
        _context.Erja.UpdateRange(erjas);
        var res = _context.SaveChanges();
        if (res > 0)
        {
            _historyLogService.PrepareForInsert($"ویرایش ارجاع توسط کاربر {currentUser.Name} {currentUser.Family}",
             EnumFormName.Erja, EnumOperation.Update, Convert.ToInt32(erjas[0].shop), erjas[0].sh_darkhast);
            return true;
        }
        _historyLogService.PrepareForInsert($"خطا در ویرایش ارجاع توسط کاربر {currentUser.Name} {currentUser.Family}",
             EnumFormName.Erja, EnumOperation.Update, Convert.ToInt32(erjas[0].shop), erjas[0].sh_darkhast);
        return false;
    }
    #endregion

    #region Insert
    public bool InsertForSabtDarkhast(DarkhastDTO darkhast)
    {
        var visitUserList = _permissionService.GetUsersByRole("Visit");
        var currentFarsiDate = _myFunctions.Get_Farsi_Date();
        var currentTime = _myFunctions.GetTime();

        var erjaList = visitUserList.Select(userInfo => new Erja
        {
            shop = darkhast.shop,
            sh_darkhast = darkhast.shodarkhast,
            tarikh_darkhast = _myFunctions.FormatDate(currentFarsiDate.ToString()),
            noedarkhast = darkhast.noedarkhast ?? "",
            c_nodarkhast = darkhast.c_noedarkhast,
            c_marhaleh = 1,
            marhaleh = "بازدید",
            name_mot = darkhast.moteghazi,
            tarikh_erja = _myFunctions.Get_Farsi_Date(),
            ijadkonandeh_c = userInfo.Id,
            ijadkonandeh = $"{userInfo.Name} {userInfo.Family}",
            ersalkonandeh_c = userInfo.Id,
            ersalkonandeh = $"{userInfo.Name} {userInfo.Family}",
            girandeh_c = userInfo.Id,
            girandeh = $"{userInfo.Name} {userInfo.Family}",
            saat_erja = currentTime,
            flag = true,
            c_vaziatErja = 1,
            vaziatErja = "در جریان",
            code_nosazi = darkhast.c_nosazi,

        }).ToList();

        _context.Erja.AddRange(erjaList);

        var res = _context.SaveChanges();
        if (res > 0)
        {
            _historyLogService.PrepareForInsert($" ثیت ارجاع برای ثیت درخواست {darkhast.shodarkhast}",
               EnumFormName.Erja, EnumOperation.Post, Convert.ToInt32(darkhast.shop), darkhast.shodarkhast);
            return true;
        }
        _historyLogService.PrepareForInsert($"خطای ثیت ارجاع برای ثیت درخواست {darkhast.shodarkhast}",
         EnumFormName.Erja, EnumOperation.Post, Convert.ToInt32(darkhast.shop), darkhast.shodarkhast);
        return false;
    }

    public bool InsertForNextMarhaleh(DarkhastDTO darkhast, EnumMarhalehTypeInfo nextMarhale)
    {
        var visitUserList = _permissionService.GetUsersByRole(nextMarhale.Name);
        var currentFarsiDate = _myFunctions.Get_Farsi_Date();
        var currentTime = _myFunctions.GetTime();

        var erjaList = visitUserList.Select(userInfo => new Erja
        {
            shop = darkhast.shop,
            sh_darkhast = darkhast.shodarkhast,
            tarikh_darkhast = _myFunctions.FormatDate(currentFarsiDate.ToString()),
            noedarkhast = darkhast.noedarkhast ?? "",
            c_nodarkhast = darkhast.c_noedarkhast,
            c_marhaleh = nextMarhale.Index,
            marhaleh = nextMarhale.DisplayName,
            name_mot = darkhast.moteghazi,
            tarikh_erja = _myFunctions.Get_Farsi_Date(),
            ijadkonandeh_c = userInfo.Id,
            ijadkonandeh = $"{userInfo.Name} {userInfo.Family}",
            ersalkonandeh_c = userInfo.Id,
            ersalkonandeh = $"{userInfo.Name} {userInfo.Family}",
            girandeh_c = userInfo.Id,
            girandeh = $"{userInfo.Name} {userInfo.Family}",
            saat_erja = currentTime,
            flag = true,
            c_vaziatErja = 1,
            vaziatErja = "در جریان",
            code_nosazi = darkhast.c_nosazi,
        }).ToList();

        if (nextMarhale.Index == 5)
        {
            erjaList.ForEach(c => { c.c_vaziatErja = 2; c.vaziatErja = "بایگانی"; });
            DoBaygani(darkhast);
        }

        _context.Erja.AddRange(erjaList);

        var res = _context.SaveChanges();
        if (res > 0)
        {
            _historyLogService.PrepareForInsert($"ثبت ارجاع درخواست {darkhast.shodarkhast} به مرحله {nextMarhale.DisplayName}",
                EnumFormName.Erja, EnumOperation.Post, Convert.ToInt32(darkhast.shop), darkhast.shodarkhast);
            return true;
        }
        _historyLogService.PrepareForInsert($"ثبت ارجاع درخواست {darkhast.shodarkhast} به مرحله {nextMarhale.DisplayName}",
            EnumFormName.Erja, EnumOperation.Post, Convert.ToInt32(darkhast.shop), darkhast.shodarkhast);
        return false;
    }
    #endregion

    public bool DoBaygani(Darkhast darkhast)
    {
        try
        {
            var tableName = _myFunctions.GetStrNoeParvandeh(darkhast.shop);
            if (string.IsNullOrEmpty(tableName))
                return false; // Invalid table name

            switch (tableName)
            {
                case "melk":
                    MelkBaygani(darkhast.shop, darkhast.shodarkhast);
                    break;

                case "sakhteman":
                    SakhtemanBaygani(darkhast.shop, darkhast.shodarkhast);
                    break;

                case "aparteman":
                    ApartemanBaygani(darkhast.shop, darkhast.shodarkhast);
                    break;
            }

            var model = new Dv_savabegh
            {
                shop = darkhast.shop,
                sh_darkhast = darkhast.shodarkhast,
                mtable_name = tableName,
                noe = darkhast.noedarkhast,
                c_noe = darkhast.c_noedarkhast,
                d_radif = (int)_myFunctions.GetRadif(darkhast.shop, darkhast.shodarkhast)
            };
            _context.Dv_savabegh.Add(model);
            _context.SaveChanges();

            _historyLogService.PrepareForInsert($" ثبت اطلاعات ارجاع به بایگانی درخواست {darkhast.shodarkhast}", EnumFormName.Erja, EnumOperation.Post);

            return true;
        }
        catch (Exception e)
        {
            _historyLogService.PrepareForInsert($"خطا در ثبت اطلاعات ارجاع به بایگانی درخواست {darkhast.shodarkhast} : {e.Message} ",
                EnumFormName.Erja, EnumOperation.Post, Convert.ToInt32(darkhast.shop), darkhast.shodarkhast);
            throw;
        }
    }

    private void MelkBaygani(int shop, int shod)
    {
        var melk = _context.Melk.FirstOrDefault(x => x.shop == shop && x.Active == true);
        if (melk != null) melk.Active = false;

        var dMelk = _context.Melk.FirstOrDefault(x => x.shop == shop && x.sh_Darkhast == shod);
        if (dMelk != null) dMelk.Active = true;
    }

    private void SakhtemanBaygani(int shop, int shod)
    {
        var shopMelk = _myFunctions.GetShoPMelk(shop);
        MelkBaygani((int)shopMelk, shod);

        var sakhteman = _context.Sakhteman.FirstOrDefault(x => x.shop == shop && x.Active == true);
        if (sakhteman != null) sakhteman.Active = false;

        var dSakhteman = _context.Sakhteman.FirstOrDefault(x => x.shop == shop && x.sh_Darkhast == shod);
        if (dSakhteman != null) dSakhteman.Active = true;
    }

    private void ApartemanBaygani(int shop, int shod)
    {
        var shopMelk = _myFunctions.GetShoPMelk(shop);
        MelkBaygani((int)shopMelk, shod);

        var shopSakhteman = _myFunctions.GetShoPSakhteman(shop);
        SakhtemanBaygani((int)shopSakhteman, shod);

        var aparteman = _context.Apartman.FirstOrDefault(x => x.shop == shop && x.Active == true);
        if (aparteman != null) aparteman.Active = false;

        var dAparteman = _context.Apartman.FirstOrDefault(x => x.shop == shop && x.sh_Darkhast == shod);
        if (dAparteman != null) dAparteman.Active = true;
    }
}
