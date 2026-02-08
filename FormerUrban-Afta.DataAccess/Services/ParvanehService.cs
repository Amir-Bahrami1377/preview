using FormerUrban_Afta.DataAccess.DTOs.Marahel;

namespace FormerUrban_Afta.DataAccess.Services;

public class ParvanehService : IParvanehService
{
    private readonly FromUrbanDbContext _context;
    private readonly IMapper _mapper;
    private readonly IDarkhastService _darkhastService;
    private readonly MyFunctions _myFunctions;
    private readonly IMelkService _melkService;
    private readonly IHistoryLogService _historyLogService;
    private readonly IAuditService _auditService;


    public ParvanehService(FromUrbanDbContext context, IMapper mapper, IDarkhastService darkhastService, MyFunctions myFunctions,
        IMelkService melkService, IHistoryLogService historyLogService, IAuditService auditService)
    {
        _context = context;
        _mapper = mapper;
        _darkhastService = darkhastService;
        _myFunctions = myFunctions;
        _melkService = melkService;
        _historyLogService = historyLogService;
        _auditService = auditService;
    }

    public async Task<bool> Exist(int shod) => await _context.Parvaneh.AnyAsync(c => c.sh_darkhast == shod);

    public async Task<ParvanehDto> GetData(int shod)
    {
        try
        {
            var data = await _context.Parvaneh.Where(c => c.sh_darkhast == shod).FirstOrDefaultAsync();
            var mapped = _mapper.Map<ParvanehDto>(data);
            mapped.IsValid = CipherService.IsEqual(data.ToString(), data.Hashed);
            if (!mapped.IsValid)
                _historyLogService.PrepareForInsert($"رد صحت سنجی داده پروانه به شماره پرونده {data.shop}",
                    EnumFormName.Parvaneh, EnumOperation.Validate, shop: data.shop, shod: shod);

            _historyLogService.PrepareForInsert($"دریافت اطلاعات پروانه با شماره پرونده {data.shop}",
                EnumFormName.Parvaneh, EnumOperation.Get, shop: data.shop, shod: shod);
            return mapped;
        }
        catch (Exception e)
        {
            _historyLogService.PrepareForInsert($"خطا در دریافت اطلاعات پروانه شماره درخواست {shod}", EnumFormName.Parvaneh, EnumOperation.Get, shod: shod);
            throw;
        }
    }

    public async Task<ParvanehDto> GetByIdAsNoTracking(long id)
    {
        var data = await _context.Parvaneh.AsNoTracking().FirstOrDefaultAsync(x => x.Identity == id);
        var model = _mapper.Map<ParvanehDto>(data);
        model.IsValid = CipherService.IsEqual(data.ToString(), data.Hashed);

        return model;
    }

    public async Task<ParvanehDto> AddFirstTime(int shod)
    {
        try
        {
            var oDarkhast = await _darkhastService.GetDataByShod(shod);
            string tblName = _myFunctions.GetStrNoeParvandeh(oDarkhast.shop);
            var shopMelk = _myFunctions.GetShoPMelk(oDarkhast.shop);
            var radif = _myFunctions.GetRadif(tblName, oDarkhast.shop, shod);
            var oMelk = await _melkService.GetDataByRadif((int)shopMelk, (int)radif);
            var model = new Parvaneh
            {
                sh_darkhast = shod,
                shop = oDarkhast.shop,
                tarikh_parvaneh = DateTime.UtcNow.AddHours(3.5),
                sho_parvaneh = shod,
                masahat_m_esh_zamin = oMelk.masahat_s,
                masahat_m_s_tarakom = oMelk.masahat_m,
            };
            await _context.Parvaneh.AddAsync(model);
            await _context.SaveChangesAsync();
            var res = await _context.Parvaneh.Where(c => c.sh_darkhast == shod).FirstOrDefaultAsync();

            _historyLogService.PrepareForInsert($"ثبت اطلاعات پروانه با شماره درخواست {shod}",
                EnumFormName.Parvaneh, EnumOperation.Post, shop: oDarkhast.shop, shod: shod);

            var mapped = _mapper.Map<ParvanehDto>(res);
            mapped.IsValid = true;
            return mapped;
        }
        catch (Exception e)
        {
            _historyLogService.PrepareForInsert($"خطا در ثبت اطلاعات پروانه با شماره درخواست {shod} : {e.Message}",
                EnumFormName.Parvaneh, EnumOperation.Post, shod: shod);
            throw;
        }
    }

    public async Task<bool> UpdateModel(ParvanehDto parvanehDto)
    {
        try
        {
            var model = _mapper.Map<Parvaneh>(parvanehDto);
            var oldModel = await GetByIdAsNoTracking(model.Identity);

            _context.Parvaneh.Update(model);
            var res = await _context.SaveChangesAsync();
            _auditService.GetDifferences<ParvanehDto>(oldModel, parvanehDto, oldModel.Identity.ToString(), EnumFormName.Parvaneh, EnumOperation.Update);

            _historyLogService.PrepareForInsert($"بروزرسانی اطلاعات پروانه درخواست {parvanehDto.sh_darkhast}",
                EnumFormName.Parvaneh, EnumOperation.Update, shop: parvanehDto.shop, shod: parvanehDto.sh_darkhast);
            return res > 0;
        }
        catch (Exception e)
        {
            _historyLogService.PrepareForInsert($"خطا در بروزرسانی اطلاعات پروانه درخواست {parvanehDto.sh_darkhast}",
                EnumFormName.Parvaneh, EnumOperation.Update, shop: parvanehDto.shop, shod: parvanehDto.sh_darkhast);
            return false;
        }
    }
}
