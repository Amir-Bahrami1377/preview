namespace FormerUrban_Afta.DataAccess.Services;
public class SakhtemanService : ISakhtemanService
{
    private readonly FromUrbanDbContext _context;
    private readonly IMapper _mapper;
    private readonly MyFunctions _myFunctions;
    private readonly IHistoryLogService _historyLogService;
    private readonly IAuditService _auditService;


    public SakhtemanService(FromUrbanDbContext context, IMapper mapper, MyFunctions myFunctions, IHistoryLogService historyLogService, IAuditService auditService)
    {
        _myFunctions = myFunctions;
        _context = context;
        _mapper = mapper;
        _historyLogService = historyLogService;
        _auditService = auditService;
    }

    #region Get
    public SakhtemanDto GetDataByShop(int Shop)
    {
        try
        {
            var data = _context.Sakhteman.FirstOrDefault(c => c.shop == Shop && c.Active == true);
            var mapped = _mapper.Map<SakhtemanDto>(data);
            mapped.IsValid = CipherService.IsEqual(data.ToString(), data.Hashed);
            if (!mapped.IsValid)
            {
                _historyLogService.PrepareForInsert($"رد صحت سنجی در دریافت اطلاعات ساختمان با شماره پرونده {Shop}",
                    EnumFormName.Sakhteman, EnumOperation.Validate, shop: Shop, shod: Convert.ToInt32(mapped.sh_Darkhast ?? 0));
            }

            _historyLogService.PrepareForInsert($"دریافت اطلاعات ساختمان با شماره پرونده {Shop}",
                EnumFormName.Sakhteman, EnumOperation.Get, shop: Shop, shod: Convert.ToInt32(mapped.sh_Darkhast ?? 0));

            return mapped;
        }
        catch (Exception e)
        {
            _historyLogService.PrepareForInsert($"خطا در دریافت اطلاعات ساختمان با شماره پرونده {Shop}",
                EnumFormName.Sakhteman, EnumOperation.Get, shop: Shop);
            throw;
        }
    }

    public SakhtemanDto GetDataByShop(int Shop, int Sh_Darkhast)
    {
        try
        {
            var data = _context.Sakhteman.FirstOrDefault(c => c.shop == Shop && c.sh_Darkhast == Sh_Darkhast);
            var mapped = _mapper.Map<SakhtemanDto>(data);
            mapped.IsValid = CipherService.IsEqual(data.ToString(), data.Hashed);
            if (!mapped.IsValid)
            {
                _historyLogService.PrepareForInsert($"رد صحت سنجی در دریافت اطلاعات ساختمان با شماره پرونده {Shop}",
                    EnumFormName.Sakhteman, EnumOperation.Validate, shop: Shop, shod: Sh_Darkhast);
            }

            _historyLogService.PrepareForInsert($"دریافت اطلاعات ساختمان با شماره پرونده {Shop}",
                EnumFormName.Sakhteman, EnumOperation.Get, shop: Shop, Sh_Darkhast);

            return mapped;
        }
        catch (Exception e)
        {
            _historyLogService.PrepareForInsert($"خطا در دریافت اطلاعات ساختمان با شماره پرونده {Shop} : {e.Message}",
                EnumFormName.Sakhteman, EnumOperation.Get, shop: Shop, shod: Sh_Darkhast);
            throw;
        }
    }

    public SakhtemanDto GetByIdAsNoTracking(long id)
    {
        var data = _context.Sakhteman.AsNoTracking().FirstOrDefault(x => x.Identity == id);
        var model = _mapper.Map<SakhtemanDto>(data);
        model.IsValid = CipherService.IsEqual(data.ToString(), data.Hashed);

        return model;
    }

    #endregion

    #region Update
    public bool Update(int shop, decimal radif, string marhale, int c_marhale, string tozihat)
    {
        try
        {
            var r = _context.Sakhteman.FirstOrDefault(c => c.shop == shop && c.radif == radif);

            r.marhaleh = marhale;
            r.c_marhaleh = c_marhale;
            _context.Entry(r).State = EntityState.Modified;
            _context.SaveChanges();
            _historyLogService.PrepareForInsert($" بروزرسانی اطلاعات ساختمان با شماره پرونده {shop}",
                EnumFormName.Sakhteman, EnumOperation.Update, shop: shop);
            return true;
        }
        catch (Exception e)
        {
            _historyLogService.PrepareForInsert($"خطا در بروزرسانی اطلاعات ساختمان با شماره پرونده {shop} : {e.Message}",
                EnumFormName.Sakhteman, EnumOperation.Update, shop: shop);
            return false;
        }
    }

    public bool UpdateActiveLastRow(int Shop)
    {
        var activeRow = _context.Sakhteman.Where(c => c.shop == Shop && c.Active == true).ToList();
        if (activeRow.Count() > 1)
        {
            var activeRowNew = activeRow.OrderByDescending(c => c.radif).ToList().Skip(1);
            foreach (var row in activeRowNew)
            {
                row.Active = false;
            }
            _context.SaveChanges();
        }
        return true;
    }

    public void UpdateByModel(SakhtemanDto obj)
    {
        try
        {
            var model = _mapper.Map<Sakhteman>(obj);
            var oldModel = GetByIdAsNoTracking(model.Identity);

            _context.Update(model);
            _context.SaveChanges();
            _auditService.GetDifferences<SakhtemanDto>(oldModel, obj, oldModel.Identity.ToString(), EnumFormName.Sakhteman, EnumOperation.Update);

            _historyLogService.PrepareForInsert($" بروزرسانی اطلاعات ساختمان با شماره پرونده {obj.shop}با ردیف {obj.radif}",
                EnumFormName.Sakhteman, EnumOperation.Update, shop: obj.shop, shod: Convert.ToInt32(obj.sh_Darkhast ?? 0));
        }
        catch (Exception e)
        {
            _historyLogService.PrepareForInsert($"خطا در بروزرسانی اطلاعات ساختمان با شماره پرونده {obj.shop} : {e.Message}",
                EnumFormName.Sakhteman, EnumOperation.Update, shop: obj.shop, shod: Convert.ToInt32(obj.sh_Darkhast ?? 0));
            throw;
        }
    }

    public void Copy_Sakhteman(int Shop, long ShodG, long ShodJ)
    {
        var radifJ = _myFunctions.MaxRadif(Shop);
        if (radifJ == 0)
        {
            throw new Exception("ورود اطلاعات برای این پرونده انجام نشده است.");
        }
        radifJ += 1;
        decimal radifG = _myFunctions.GetRadif("sakhteman", Shop, ShodG);
        if (radifG == 0)
        {
            throw new Exception("اطلاعات درخواست قبلی وجود ندارد.");
        }
        var sakhteman = _context.Sakhteman.Where(m => m.shop == Shop && m.radif == radifG).AsNoTracking().SingleOrDefault();

        if (sakhteman != null)
        {
            Sakhteman s = sakhteman;
            s.Identity = 0;
            s.Active = false;
            s.radif = (int)radifJ;
            s.sh_Darkhast = ShodJ;
            _context.Sakhteman.Add(s);
        }

        _myFunctions.Copy_DvTable("Sakhteman", "Dv_karbari", Shop, radifJ, radifG);
        _myFunctions.Copy_DvTable("Sakhteman", "Dv_savabegh", Shop, radifJ, radifG);
    }

    public void CopyUpdate_Sakhteman(int shop, long shodG, long shodJ)
    {
        decimal decShop = Convert.ToDecimal(shop);
        var sakhteman = _context.Sakhteman.FirstOrDefault(m => m.shop == decShop && m.sh_Darkhast == shodG);
        if (sakhteman == null)
            return;

        decimal radifJ = _myFunctions.GetRadif("sakhteman", shop, shodJ);
        decimal radifG = _myFunctions.GetRadif("sakhteman", shop, shodG);
        var lstSakhteman = _context.Sakhteman.Where(m => m.shop == decShop && m.sh_Darkhast == shodJ).ToList();

        foreach (var m in lstSakhteman)
        {
            _context.Entry(m).CurrentValues.SetValues(sakhteman); // Copy properties from sakhteman
            m.radif = (int)radifJ;                                     // Set radif
            m.Active = false;                                     // Deactivate
            m.sh_Darkhast = shodJ;                                // Set sh_Darkhast (optional)
            _context.Entry(m).State = EntityState.Modified;       // Mark as modified
        }

        _myFunctions.Update_DvTable("Sakhteman", "Dv_karbari", Convert.ToDecimal(shop), radifJ, radifG);
        _myFunctions.Update_DvTable("Sakhteman", "Dv_savabegh", Convert.ToDecimal(shop), radifJ, radifG);
    }
    #endregion

    #region Insert
    public decimal Insert(int shop, string marhale, int c_marhale, string tozihat)
    {
        var r = new Sakhteman();
        r.radif = _context.Sakhteman.Where(c => c.shop == shop && c.Active == true).Select(c => c.radif).DefaultIfEmpty(0).Max() + 1;
        r.shop = shop;
        r.marhaleh = marhale;
        r.c_marhaleh = c_marhale;
        r.Active = true;
        _context.Sakhteman.Add(r);
        _context.SaveChanges();
        return r.radif;
    }
    public void InsetByModel(SakhtemanDto obj)
    {
        try
        {
            var model = _mapper.Map<Sakhteman>(obj);
            _context.Sakhteman.Add(model);
            _context.SaveChanges();
            _historyLogService.PrepareForInsert($" ثبت اطلاعات ساختمان با شماره پرونده {obj.shop}",
                EnumFormName.Sakhteman, EnumOperation.Post, shop: obj.shop, shod: Convert.ToInt32(obj.sh_Darkhast ?? 0));
        }
        catch (Exception e)
        {
            _historyLogService.PrepareForInsert($" خطا در ثبت اطلاعات ساختمان با شماره پرونده {obj.shop} : {e.Message}",
                EnumFormName.Sakhteman, EnumOperation.Post, shop: obj.shop, shod: Convert.ToInt32(obj.sh_Darkhast ?? 0));
            throw;
        }
    }
    #endregion

}