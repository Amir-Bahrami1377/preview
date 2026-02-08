namespace FormerUrban_Afta.DataAccess.Services;
public class ParvandehService : IParvandehService
{
    private readonly MyFunctions _myFunctions;
    private readonly FromUrbanDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMelkService _melkService;
    private readonly ISakhtemanService _sakhtemanService;
    private readonly IApartmanService _apartmanService;
    private readonly IHistoryLogService _historyLogService;
    private readonly IAuditService _auditService;


    public ParvandehService(FromUrbanDbContext context, IMapper mapper, MyFunctions myFunctions, IMelkService melkService, ISakhtemanService sakhtemanService,
        IApartmanService apartmanService, IHistoryLogService historyLogService, IAuditService auditService)
    {
        _context = context;
        _mapper = mapper;
        _myFunctions = myFunctions;
        _melkService = melkService;
        _sakhtemanService = sakhtemanService;
        _apartmanService = apartmanService;
        _historyLogService = historyLogService;
        _auditService = auditService;
    }

    public static bool CheckHash(Parvandeh obj)
    {
        var hash = CipherService.IsEqual(obj.ToString(), obj.Hashed);
        return hash;
    }

    #region Get

    public ParvandehDto GetRow(string shop)
    {
        try
        {
            var shopDouble = double.Parse(shop);
            var parvandeh = _context.Parvandeh.Where(p => p.shop == shopDouble).AsNoTracking().FirstOrDefault();

            if (parvandeh == null)
                return null;

            var model = _mapper.Map<ParvandehDto>(parvandeh);
            model.IsValid = CheckHash(parvandeh);
            if (!model.IsValid)
            {

                _historyLogService.PrepareForInsert($"رد صحت سنجی در اطلاعات پرونده {shop}", EnumFormName.Parvandeh, EnumOperation.Validate,
                    shop: Convert.ToInt32(shop));
            }
            // Log activity

            _historyLogService.PrepareForInsert($"دریافت اطلاعات پرونده {shop}", EnumFormName.Parvandeh, EnumOperation.Get,
                shop: Convert.ToInt32(shop));
            return model;
        }
        catch (Exception e)
        {

            _historyLogService.PrepareForInsert($"خطا در دریافت اطلاعات پرونده : {e.Message} {shop}", EnumFormName.Parvandeh, EnumOperation.Get,
                shop: Convert.ToInt32(shop));
            throw;
        }

    }

    public ParvandehDto GetRowByCodeN(string CodeN)
    {
        try
        {
            var parvandeh = _context.Parvandeh.Where(p => p.codeN == CodeN).AsNoTracking().FirstOrDefault();

            if (parvandeh == null)
                return null;

            var model = _mapper.Map<ParvandehDto>(parvandeh);
            model.IsValid = CheckHash(parvandeh);

            if (!model.IsValid)
            {

                _historyLogService.PrepareForInsert($"رد صحت سنجی در اطلاعات پرونده {parvandeh.shop}", EnumFormName.Parvandeh, EnumOperation.Validate,
                    shop: Convert.ToInt32(parvandeh.shop));
            }
            // Log activity

            _historyLogService.PrepareForInsert($"دریافت اطلاعات پرونده {parvandeh.shop}", EnumFormName.Parvandeh, EnumOperation.Get,
                shop: Convert.ToInt32(parvandeh.shop));

            return model;
        }
        catch (Exception e)
        {

            _historyLogService.PrepareForInsert($"خطا در دریافت اطلاعات پرونده با کد نوسازی : {e.Message} {CodeN}", EnumFormName.Parvandeh, EnumOperation.Get);
            throw;
        }
    }

    public int GetParvandehType(string shop)
    {
        try
        {
            double shopDouble = double.Parse(shop);
            return (int)_context.Parvandeh.Where(p => p.shop == shopDouble).Select(p => p.idparent).SingleOrDefault();
        }
        catch (Exception e)
        {

            _historyLogService.PrepareForInsert($"خطا در دریافت اطلاعات پرونده  : {e.Message} {shop}", EnumFormName.Parvandeh,
                EnumOperation.Get, shop: Convert.ToInt32(shop));
            throw;
        }
    }

    public bool CheckExistSenfi(decimal mantagheh, decimal hozeh, decimal blok, decimal shomelk, decimal sakhteman, decimal apar)
    {
        try
        {
            return _context.Parvandeh.Any(c => c.mantaghe == mantagheh && c.hoze == hozeh && c.blok == blok
                                               && c.shomelk == shomelk && c.sakhteman == sakhteman && c.apar == apar && c.senfi > 0);
        }
        catch (Exception e)
        {

            _historyLogService.PrepareForInsert($"خطا در دریافت اطلاعات پرونده  : {e.Message}", EnumFormName.Parvandeh,
                EnumOperation.Get);
            throw;
        }
    }

    public bool CheckExistApartman(decimal mantagheh, decimal hozeh, decimal blok, decimal shomelk, decimal sakhteman)
    {
        try
        {
            return _context.Parvandeh.Any(c => c.mantaghe == mantagheh && c.hoze == hozeh && c.blok == blok
                                               && c.shomelk == shomelk && c.sakhteman == sakhteman && c.apar > 0);
        }
        catch (Exception e)
        {

            _historyLogService.PrepareForInsert($"خطا در دریافت اطلاعات پرونده  : {e.Message}", EnumFormName.Parvandeh,
                EnumOperation.Get);
            throw;
        }
    }

    public bool CheckSakhtemanHasSenf(decimal mantagheh, decimal hozeh, decimal blok, decimal shomelk, decimal sakhteman)
    {
        try
        {
            return _context.Parvandeh.Any(c => c.mantaghe == mantagheh && c.hoze == hozeh && c.blok == blok
                                               && c.shomelk == shomelk && c.sakhteman == sakhteman && c.apar == 0 && c.senfi > 0);
        }
        catch (Exception e)
        {

            _historyLogService.PrepareForInsert($"خطا در دریافت اطلاعات پرونده  : {e.Message}", EnumFormName.Parvandeh,
                EnumOperation.Get);
            throw;
        }
    }

    public ParvandehDto GetRow(long mantaghe, long hoze, long blok, long melk, long sakh, long apar, long senfi)
    {
        try
        {
            var parvandeh = _context.Parvandeh.SingleOrDefault(c => c.mantaghe == mantaghe && c.hoze == hoze && c.blok == blok &&
                                                                    c.shomelk == melk && c.sakhteman == sakh && c.apar == apar && c.senfi == senfi);
            var model = _mapper.Map<ParvandehDto>(parvandeh);
            model.IsValid = CheckHash(parvandeh);

            if (!model.IsValid)
            {

                _historyLogService.PrepareForInsert($"رد صحت سنجی در اطلاعات پرونده {parvandeh.shop}", EnumFormName.Parvandeh, EnumOperation.Validate,
                    shop: Convert.ToInt32(parvandeh.shop));
            }
            // Log activity

            _historyLogService.PrepareForInsert($"دریافت اطلاعات پرونده {parvandeh.shop}", EnumFormName.Parvandeh, EnumOperation.Get,
                shop: Convert.ToInt32(parvandeh.shop));

            return model;
        }
        catch (Exception e)
        {

            _historyLogService.PrepareForInsert($"خطا در دریافت اطلاعات پرونده  : {e.Message}", EnumFormName.Parvandeh,
                EnumOperation.Get);
            throw;
        }
    }

    public int CheckCountParvandeh(int mantagheh, int hozeh, int blok, int melk, int sakhteman, int aparteman, int senf)
    {
        try
        {
            return _context.Parvandeh.Count(x => x.mantaghe == mantagheh && x.hoze == hozeh && x.blok == blok &&
                                                 x.shomelk == melk && x.sakhteman == sakhteman && x.apar == aparteman &&
                                                 x.senfi == senf);
        }
        catch (Exception e)
        {

            _historyLogService.PrepareForInsert($"خطا در دریافت اطلاعات پرونده  : {e.Message}", EnumFormName.Parvandeh,
                EnumOperation.Get);
            throw;
        }
    }

    public int CheckCountParvandehByCodeTree(int mantagheh, int hozeh, int blok, int melk, double codeTree)
    {
        try
        {
            return _context.Parvandeh.Count(x => x.mantaghe == mantagheh && x.hoze == hozeh && x.blok == blok &&
                                                 x.shomelk == melk && codeTree == codeTree);
        }
        catch (Exception e)
        {

            _historyLogService.PrepareForInsert($"خطا در دریافت اطلاعات پرونده  : {e.Message}", EnumFormName.Parvandeh,
                EnumOperation.Get);
            throw;
        }
    }

    public int GetMaxShop()
    {
        try
        {
            return _context.Parvandeh.Any() ? (_context.Parvandeh.Max(x => (int?)x.shop) ?? 0) + 1 : 1;
        }
        catch (Exception e)
        {

            _historyLogService.PrepareForInsert($"خطا در دریافت اطلاعات شماره پرونده جدید  : {e.Message}", EnumFormName.Parvandeh,
                EnumOperation.Get);
            throw;
        }
    }

    public List<ParvandehDto> GetByCodeNAndCodeTree(string codeN, int codeTree)
    {
        try
        {
            var c = codeN.Split('-');
            var parvandeh = _context.Parvandeh.Where(x =>
                    x.mantaghe == Convert.ToInt32(c[0]) && x.hoze == Convert.ToInt32(c[1]) &&
                    x.blok == Convert.ToInt32(c[2]) && x.shomelk == Convert.ToInt32(c[3]) && x.code_tree == codeTree)
                .ToList();
            var result = parvandeh.Select(item =>
            {
                var dto = _mapper.Map<ParvandehDto>(item);
                dto.IsValid = CheckHash(item);
                return dto;
            }).ToList();
            var invalidRecords = result.Where(c => !c.IsValid).ToList();
            if (invalidRecords.Any())
            {
                foreach (var item in invalidRecords)
                {
                    _historyLogService.PrepareForInsert($"رد صحت سنجی در دریافت داده پرونده {item.shop}",
                        EnumFormName.Parvandeh, EnumOperation.Validate, shop: item.shop);
                }
            }

            // Log activity
            _historyLogService.PrepareForInsert($"دریافت اطلاعات پرونده با کد نوسازی {codeN} ", EnumFormName.Parvandeh, EnumOperation.Get,
                shop: Convert.ToInt32(parvandeh.FirstOrDefault()?.shop));
            return result;
        }
        catch (Exception e)
        {
            _historyLogService.PrepareForInsert($"خطا در دریافت اطلاعات  پرونده با کد نوسازی {codeN} : {e.Message}", EnumFormName.Parvandeh,
                EnumOperation.Get);
            throw;
        }
    }

    public ParvandehDto GetRowForTreeViewByCodeN(string CodeN)
    {
        try
        {
            var parvandeh = _context.Parvandeh.FirstOrDefault(w => w.codeN == CodeN);

            if (parvandeh == null)
                return null;

            var model = _mapper.Map<ParvandehDto>(parvandeh);
            if (parvandeh != null)
                model.IsValid = CheckHash(parvandeh);

            if (!model.IsValid)
                _historyLogService.PrepareForInsert($"رد صحت سنجی در دریافت داده پرونده {model.shop}", EnumFormName.Parvandeh, EnumOperation.Validate, shop: model.shop);

            _historyLogService.PrepareForInsert($"دریافت اطلاعات پرونده  {model.shop} ", EnumFormName.Parvandeh, EnumOperation.Get, shop: Convert.ToInt32(parvandeh.shop));

            return model;
        }
        catch (Exception e)
        {
            _historyLogService.PrepareForInsert($"خطا در دریافت اطلاعات  پرونده با کد نوسازی {CodeN} : {e.Message}", EnumFormName.Parvandeh,
                EnumOperation.Get);
            throw;
        }
    }

    public long GetShodMojud(int shop)
    {
        string tblName = _myFunctions.GetStrNoeParvandeh((double)shop).ToLower();
        long shod_mojod = 0;
        switch (tblName)
        {
            case "melk":
                var melk = _context.Melk.FirstOrDefault(m => m.shop == shop && m.Active == true);
                if (melk?.sh_Darkhast != null)
                    shod_mojod = (long)melk.sh_Darkhast;
                break;
            case "sakhteman":
                var sakh = _context.Sakhteman.FirstOrDefault(m => m.shop == shop && m.Active == true);
                if (sakh?.sh_Darkhast != null)
                    shod_mojod = (long)sakh.sh_Darkhast;
                break;
            case "apartman":
                var apar = _context.Apartman.FirstOrDefault(m => m.shop == shop && m.Active == true);
                if (apar?.sh_Darkhast != null)
                    shod_mojod = (long)apar.sh_Darkhast;

                break;
        }
        return shod_mojod;
    }

    public int GetAreaId(int shop) => _context.Parvandeh.Where(c => c.shop == shop).Select(c => c.AreaId).FirstOrDefault();

    public ParvandehDto GetByIdAsNoTracking(long id)
    {
        var data = _context.Parvandeh.AsNoTracking().FirstOrDefault(x => x.Identity == id);
        var model = _mapper.Map<ParvandehDto>(data);
        model.IsValid = CheckHash(data);

        return model;
    }
    #endregion

    #region Update

    public void UpdateByModel(ParvandehDto obj)
    {
        try
        {
            var model = _mapper.Map<Parvandeh>(obj);
            var oldModel = GetByIdAsNoTracking(model.Identity);
            _context.Update(model);
            _context.SaveChanges();
            _auditService.GetDifferences<ParvandehDto>(oldModel, obj, oldModel.Identity.ToString(), EnumFormName.Parvandeh, EnumOperation.Update);

            _historyLogService.PrepareForInsert($"بروزرسانی اطلاعات  پرونده {obj.shop}", EnumFormName.Parvandeh, EnumOperation.Update, shop: obj.shop);
        }
        catch (Exception e)
        {
            _historyLogService.PrepareForInsert($"خطا در بروزرسانی اطلاعات  پرونده  {obj.shop} : {e.Message}", EnumFormName.Parvandeh,
                EnumOperation.Update, shop: obj.shop);
            throw;
        }
    }

    public bool copyForSabtDarkhast(int Shop, long ShodG, long ShodJ)
    {
        try
        {
            var modelItemParvandeh = GetRow(Shop.ToString());

            if (modelItemParvandeh == null)
                return false;

            decimal shopToDecimal = Convert.ToDecimal(Shop);
            int noeParvandeh = _myFunctions.GetNoeParvandeh((long)Shop);

            switch (noeParvandeh)
            {
                case 1:
                    {
                        var modelItemMelk = _context.Melk.Where(m => m.shop == shopToDecimal && m.sh_Darkhast == ShodJ).Select(s => s.shop).FirstOrDefault();

                        if (modelItemMelk <= 0)
                            _melkService.Copy_Melk(Shop, ShodG, ShodJ);
                        else
                            _melkService.CopyUpdate_Melk(Shop, ShodG, ShodJ);
                    }
                    break;
                case 2:
                    {
                        var modelItemSakhteman = _context.Sakhteman
                            .Where(m => m.shop == shopToDecimal && m.sh_Darkhast == ShodJ)
                            .Select(s => s.shop).FirstOrDefault();

                        if (modelItemSakhteman <= 0)
                        {
                            _sakhtemanService.Copy_Sakhteman(Shop, ShodG, ShodJ);
                            _melkService.Copy_Melk((int)_myFunctions.GetShoPMelk(Shop), ShodG, ShodJ);
                        }
                        else
                        {
                            _sakhtemanService.CopyUpdate_Sakhteman(Shop, ShodG, ShodJ);
                            _melkService.CopyUpdate_Melk((int)_myFunctions.GetShoPMelk(Shop), ShodG, ShodJ);
                        }
                    }
                    break;
                case 3:
                    {
                        var modelItemApartman = _context.Apartman
                            .Where(m => m.shop == shopToDecimal && m.sh_Darkhast == ShodJ)
                            .Select(s => s.shop).FirstOrDefault();

                        if (modelItemApartman <= 0)
                        {
                            _apartmanService.Copy_Apar(Shop, ShodG, ShodJ);
                            _sakhtemanService.Copy_Sakhteman((int)_myFunctions.GetShoPSakhteman(Shop),
                                ShodG, ShodJ);
                            _melkService.Copy_Melk((int)_myFunctions.GetShoPMelk(Shop), ShodG, ShodJ);
                        }
                        else
                        {
                            _apartmanService.CopyUpdate_Apartman(Shop, ShodG, ShodJ);
                            _sakhtemanService.CopyUpdate_Sakhteman((int)_myFunctions.GetShoPSakhteman(Shop), ShodG, ShodJ);
                            _melkService.CopyUpdate_Melk((int)_myFunctions.GetShoPMelk(Shop), ShodG, ShodJ);
                        }
                    }
                    break;
            }

            _historyLogService.PrepareForInsert($"کپی اطلاعات  پرونده {Shop}", EnumFormName.Parvandeh,
                EnumOperation.Post, shop: Shop);

            return true;

        }
        catch (Exception ex)
        {
            _historyLogService.PrepareForInsert($"خطا در کپی اطلاعات  پرونده  {Shop} : {ex.Message}", EnumFormName.Parvandeh,
                EnumOperation.Post, shop: Shop);
            return false;
        }
    }

    #endregion

    #region Add

    public void InsertByModel(ParvandehDto obj)
    {
        try
        {
            var model = _mapper.Map<Parvandeh>(obj);
            _context.Add(model);
            _context.SaveChanges();
            _historyLogService.PrepareForInsert($"ثبت اطلاعات پرونده  {obj.shop} ", EnumFormName.Parvandeh, EnumOperation.Post,
                shop: Convert.ToInt32(obj.shop));
        }
        catch (Exception e)
        {
            _historyLogService.PrepareForInsert($"خطا در ثبت اطلاعات پرونده  {obj.shop} ", EnumFormName.Parvandeh, EnumOperation.Post,
                shop: Convert.ToInt32(obj.shop));
            throw;
        }
    }

    #endregion

    #region Delete

    #endregion
}