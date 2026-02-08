namespace FormerUrban_Afta.DataAccess.Services;

public class Dv_KarbariService : IDv_KarbariService
{
    private readonly FromUrbanDbContext _context;
    private readonly IMapper _mapper;
    private readonly MyFunctions _myFunctions;
    private readonly IAuditService _auditService;
    private readonly IHistoryLogService _historyLogService;

    public Dv_KarbariService(FromUrbanDbContext context, IMapper mapper, MyFunctions myFunctions, IHistoryLogService historyLogService, IAuditService auditService)
    {
        _context = context;
        _mapper = mapper;
        _myFunctions = myFunctions;
        _historyLogService = historyLogService;
        _auditService = auditService;
    }

    public static bool CheckHash(Dv_karbari obj)
    {
        var hash = CipherService.IsEqual(obj.ToString(), obj.Hashed);
        return hash;
    }

    #region Get
    public List<Dv_karbariDTO> GetDataByRadif(int shop, decimal radif)
    {
        try
        {
            string tableName = _myFunctions.GetStrNoeParvandeh(shop) switch
            {
                //"sakhteman" => "melk",
                "sakhteman" => "sakhteman",
                var name => name
            };

            var karbariData = _context.Dv_karbari.Where(d => d.shop == shop && d.d_radif == radif && d.mtable_name == tableName).ToList();

            // Map to DTOs and validate
            var results = karbariData.Select(item =>
            {
                var dto = _mapper.Map<Dv_karbariDTO>(item);
                dto.IsValid = CheckHash(item);
                return dto;
            }).OrderByDescending(x => x.CreateDateTime).ToList();

            // Log invalid records in a batch
            var invalidRecords = results.Where(dto => !dto.IsValid)
                .Select(dto => $"رد صحت سنجی داده کاربری با شماره پرونده {shop} و ردیف {radif} و آیدی ردیف {dto.id}").ToList();

            if (invalidRecords.Any())
            {
                foreach (var message in invalidRecords)
                {
                    _historyLogService.PrepareForInsert(message, EnumFormName.Dv_karbari, EnumOperation.Validate, shop: shop);
                }
            }

            // Log activity
            _historyLogService.PrepareForInsert($"دریافت اطلاعات کاربری پرونده {shop} و ردیف {radif}", EnumFormName.Dv_karbari, EnumOperation.Get, shop: shop);

            return results;
        }
        catch (Exception ex)
        {
            _historyLogService.PrepareForInsert(
                $"خطا در دریافت اطلاعات کاربری پرونده {shop} و ردیف {radif}: {ex.Message}", EnumFormName.Dv_karbari, EnumOperation.Get, shop: shop);
            throw; // Rethrow or handle as needed
        }
    }

    public Dv_karbariDTO GetById(long id)
    {
        var data = _context.Dv_karbari.FirstOrDefault(x => x.Identity == id);
        var model = _mapper.Map<Dv_karbariDTO>(data);
        model.IsValid = CheckHash(data);
        if (!model.IsValid)
            _historyLogService.PrepareForInsert($"رد صحت سنجی داده کاربری با شماره پرونده {model.shop} و آیدی {id} و ردیف {model.d_radif}", EnumFormName.Dv_karbari, EnumOperation.Validate, shop: model.shop);

        _historyLogService.PrepareForInsert($"دریافت اطلاعات کاربری با شماره پرونده {model.shop} و آیدی {id} و ردیف {model.d_radif}", EnumFormName.Dv_karbari, EnumOperation.Get, shop: model.shop);
        return model;
    }

    public Dv_karbariDTO GetByIdAsNoTracking(long id)
    {
        var data = _context.Dv_karbari.AsNoTracking().FirstOrDefault(x => x.Identity == id);
        var model = _mapper.Map<Dv_karbariDTO>(data);
        model.IsValid = CheckHash(data);

        return model;
    }
    #endregion

    #region Update
    public bool UpdateKarbari(Dv_karbariDTO dv_KarbariDTO)
    {
        var model = _mapper.Map<Dv_karbari>(dv_KarbariDTO);
        var old = GetByIdAsNoTracking(model.Identity);
        _context.Dv_karbari.Update(model);

        var res = _context.SaveChanges();
        if (res > 0)
        {
            _auditService.GetDifferences<Dv_karbariDTO>(old, dv_KarbariDTO, old.Identity.ToString(), EnumFormName.Dv_karbari, EnumOperation.Update);
            _historyLogService.PrepareForInsert($"ویرایش اطلاعات کاربری پرونده {model.shop} با آیدی  {model.id} و ردیف {model.d_radif}", EnumFormName.Dv_karbari, EnumOperation.Update, shop: model.shop);
            return true;
        }

        _historyLogService.PrepareForInsert($"خطا در ویرایش اطلاعات کاربری پرونده {model.shop} با آیدی  {model.id} و ردیف {model.d_radif}", EnumFormName.Dv_karbari, EnumOperation.Update, shop: dv_KarbariDTO.shop);
        return false;
    }
    #endregion

    #region Insert
    public bool InsertByModel(Dv_karbariDTO karbari, string mtableName)
    {
        var Model = _mapper.Map<Dv_karbari>(karbari);
        if (karbari.d_radif <= 0)
            karbari.d_radif = (int)_myFunctions.GetRadif(mtableName, karbari.shop);
        karbari.mtable_name = mtableName;
        karbari.id = _context.Dv_karbari.Where(c => c.shop == karbari.shop && c.d_radif == karbari.d_radif && c.mtable_name == mtableName)
                                .Select(c => c.id).ToList().DefaultIfEmpty(0).Max() + 1;

        _context.Dv_karbari.Add(karbari);
        var res = _context.SaveChanges();
        if (res > 0)
        {
            _historyLogService.PrepareForInsert(
                $"افزودن اطلاعات کاربری پرونده {Model.shop} با آیدی  {Model.id} و ردیف {Model.d_radif}", EnumFormName.Dv_karbari, EnumOperation.Post, shop: karbari.shop);
            return true;
        }
        _historyLogService.PrepareForInsert(
            $"خطا در افزودن اطلاعات کاربری پرونده {Model.shop} با آیدی  {Model.id} و ردیف {Model.d_radif}", EnumFormName.Dv_karbari, EnumOperation.Post, shop: karbari.shop);
        return false;
    }
    #endregion

    #region Delete
    public bool DeleteByModel(Dv_karbariDTO karbari)
    {
        var d = _context.Dv_karbari.SingleOrDefault(c => c.shop == karbari.shop && c.d_radif == karbari.d_radif && c.id == karbari.id && c.mtable_name == karbari.mtable_name);
        if (d != null)
            _context.Dv_karbari.Remove(d);

        var res = _context.SaveChanges();
        if (res > 0)
        {
            _historyLogService.PrepareForInsert($"حذف اطلاعات کاربری پرونده {karbari.shop} با آیدی  {karbari.id}  و ردیف  {karbari.d_radif}",
                EnumFormName.Dv_karbari, EnumOperation.Delete);
            return true;
        }
        _historyLogService.PrepareForInsert($"خطا در حذف اطلاعات کاربری پرونده {karbari.shop} با آیدی {karbari.id}  و ردیف {karbari.d_radif}",
            EnumFormName.Dv_karbari, EnumOperation.Delete);
        return false;
    }

    public bool DeleteById(long identity, int shop)
    {
        var karbari = _context.Dv_karbari.FirstOrDefault(c => c.Identity == identity);
        if (karbari == null)
        {
            _historyLogService.PrepareForInsert($"خطا در حذف کاربری پرونده {shop}", EnumFormName.Dv_karbari, EnumOperation.Delete, shop: shop);
            return false;
        }
        _context.Dv_karbari.Remove(karbari);
        var res = _context.SaveChanges();
        _historyLogService.PrepareForInsert(
            $"حذف  کاربری با پرونده {shop} با آیدی  {karbari.id} و ردیف {karbari.d_radif}", EnumFormName.Dv_karbari, EnumOperation.Delete, shop: shop);
        return res > 0;
    }
    #endregion
}
