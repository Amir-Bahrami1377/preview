using FormerUrban_Afta.DataAccess.DTOs.Marahel;

namespace FormerUrban_Afta.DataAccess.Services;
public class ControlMapService
{
    private readonly FromUrbanDbContext _context;
    private readonly IMapper _mapper;
    private readonly MyFunctions _myFunctions;
    private readonly IMelkService _melkService;
    private readonly IDarkhastService _darkhastService;
    private readonly IHistoryLogService _historyLogService;
    private readonly IAuditService _auditService;


    public ControlMapService(FromUrbanDbContext context, IMapper mapper, MyFunctions myFunctions, IMelkService melkService, IDarkhastService darkhastService,
        IHistoryLogService historyLogService, IAuditService auditService)
    {
        _mapper = mapper;
        _context = context;
        _myFunctions = myFunctions;
        _melkService = melkService;
        _darkhastService = darkhastService;
        _historyLogService = historyLogService;
        _auditService = auditService;
    }

    #region Security
    public static bool CheckHash(ControlMap obj)
    {
        var hash = CipherService.IsEqual(obj.ToString(), obj.Hashed);
        return hash;
    }
    #endregion
    public bool Exists(int shod) =>
        _context.ControlMap.Any(c => c.sh_Darkhast == shod);

    public ControlMapDto GetData(int shod, int shop)
    {
        var data = _context.ControlMap.FirstOrDefault(c => c.shop == shop && c.sh_Darkhast == shod);
        if (data == null)
        {
            _historyLogService.PrepareForInsert($"خطا در دریافت اطلاعات کنترل نقشه با شماره پرونده {shop} و  شماره درخواست {shod} پرونده یافت نشد",
                EnumFormName.ControlMap, EnumOperation.Get, shop: shop, shod: shod);
            return null;
        }
        var mapped = _mapper.Map<ControlMapDto>(data);
        mapped.IsValid = CheckHash(data);
        if (!mapped.IsValid)
        {
            _historyLogService.PrepareForInsert($"خطا در دریافت اطلاعات کنترل نقشه با شماره پرونده {shop} و شماره درخواست {mapped.sh_Darkhast} صحت سنجی داده رد شد",
                EnumFormName.ControlMap, EnumOperation.Validate, shop: shop);
        }

        _historyLogService.PrepareForInsert($" دریافت اطلاعات کنترل نقشه با شماره پرونده{shop} و شماره درخواست {mapped.sh_Darkhast}",
            EnumFormName.ControlMap, EnumOperation.Get, shop: shop, shod: shod);

        return mapped;
    }

    public ControlMapDto GetByIdAsNoTracking(long id)
    {
        var data = _context.ControlMap.AsNoTracking().FirstOrDefault(x => x.Identity == id);
        var model = _mapper.Map<ControlMapDto>(data);
        model.IsValid = CheckHash(data);

        return model;
    }

    public async Task<ControlMapDto> CreateNew(int shod, int shop)
    {
        string tblName = _myFunctions.GetStrNoeParvandeh(shop);
        var shopMelk = _myFunctions.GetShoPMelk(shop);
        var radif = _myFunctions.GetRadif(tblName, shop, shod);
        var oMelk = await _melkService.GetDataByRadif((int)shopMelk, (int)radif);
        Darkhast oDarkhast = await _darkhastService.GetDataByShod(shod);

        var controlMap = new ControlMap
        {
            shop = oDarkhast.shop,
            sh_Darkhast = oDarkhast.shodarkhast,
            masahat_s = oMelk.masahat_s,
            masahat_m = oMelk.masahat_m,
            masahat_e = oMelk.masahat_e,
            masahat_b = oMelk.masahat_b,
            NoeNama = "",
            NoeSaze = "",
            noesaghf = "",
        };

        _context.ControlMap.Add(controlMap);
        var res = _context.SaveChanges();
        if (res == 0)
        {
            _historyLogService.PrepareForInsert($"خطا در ثبت اطلاعات در کنترل نقشه با شماره پرونده {shop} و شماره درخواست {shod} ذخیره اطلاعات در دیتابیس انجام نشد",
                EnumFormName.ControlMap, EnumOperation.Post, shop: shop, shod: shod);
            return null;
        }

        _historyLogService.PrepareForInsert($" ثبت اطلاعات در کنترل نقشه با شماره پرونده {shop}و شماره درخواست {shod}", EnumFormName.ControlMap, EnumOperation.Post, shop: shop, shod: shod);

        var obj = await _context.ControlMap.Where(c => c.sh_Darkhast == controlMap.sh_Darkhast).FirstOrDefaultAsync();
        var mapped = _mapper.Map<ControlMapDto>(obj);
        mapped.IsValid = true;
        return mapped;
    }

    public bool UpdateByModel(ControlMapDto controlMapDto)
    {
        var model = _mapper.Map<ControlMap>(controlMapDto);
        var oldModel = GetByIdAsNoTracking(model.Identity);
        _context.ControlMap.Update(model);
        var res = _context.SaveChanges();
        if (res == 0)
        {
            _historyLogService.PrepareForInsert($"خطا در ویرایش اطلاعات در کنترل نقشه با شماره پرونده {model.shop} و شماره درخواست {model.sh_Darkhast} ذخیره اطلاعات در دیتابیس انجام نشد",
                EnumFormName.ControlMap, EnumOperation.Update, shop: controlMapDto.shop, shod: controlMapDto.sh_Darkhast);
            return false;
        }

        _auditService.GetDifferences<ControlMapDto>(oldModel, controlMapDto, oldModel.Identity.ToString(), EnumFormName.ControlMap, EnumOperation.Update);

        _historyLogService.PrepareForInsert($" ویرایش اطلاعات در کنترل نقشه با شماره پرونده {model.shop} و شماره درخواست {model.sh_Darkhast}",
            EnumFormName.ControlMap, EnumOperation.Update, shop: controlMapDto.shop, shod: controlMapDto.sh_Darkhast);
        return true;
    }
}
