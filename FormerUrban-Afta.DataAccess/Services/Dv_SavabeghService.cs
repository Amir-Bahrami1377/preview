namespace FormerUrban_Afta.DataAccess.Services;
class Dv_SavabeghService : IDv_SavabeghService
{
    private readonly FromUrbanDbContext _context;
    private readonly MyFunctions _myFunctions;
    private readonly IMapper _mapper;
    private readonly IHistoryLogService _historyLogService;

    public Dv_SavabeghService(FromUrbanDbContext context, MyFunctions myFunctions, IMapper mapper, IHistoryLogService historyLogService)
    {
        _context = context;
        _myFunctions = myFunctions;
        _mapper = mapper;
        _historyLogService = historyLogService;
    }

    public static bool CheckHash(Dv_savabegh obj)
    {
        var hash = CipherService.IsEqual(obj.ToString(), obj.Hashed);
        return hash;
    }

    #region Get
    public List<Dv_savabeghDTO> GetData(int shop, int shod)
    {
        try
        {
            var radif = _myFunctions.GetRadif(shop, shod);
            var tableName = _myFunctions.GetStrNoeParvandeh(shop);
            var listData = _context.Dv_savabegh.Where(c => c.shop == shop && c.d_radif == radif && c.mtable_name == tableName).ToList();
            if (!listData.Any() && tableName == "sakhteman")
            {
                var PropertyId = _myFunctions.GetShoPMelk(shop);
                listData = _context.Dv_savabegh.Where(c => c.shop == PropertyId && c.d_radif == radif && c.mtable_name == "melk").ToList();
            }


            // Map to DTOs and validate
            var results = listData.Select(item =>
            {
                var dto = _mapper.Map<Dv_savabeghDTO>(item);
                dto.IsValid = CheckHash(item);
                return dto;
            }).OrderByDescending(x => x.CreateDateTime).ToList();

            // Log invalid records in a batch
            var invalidRecords = results.Where(dto => !dto.IsValid).Select(_ => $"رد صحت سنجی داده سوابق یا شماره پرونده {shop} و ردیف {radif}").ToList();

            if (invalidRecords.Any())
            {
                foreach (var message in invalidRecords)
                {
                    _historyLogService.PrepareForInsert(message, EnumFormName.Dv_savabegh, EnumOperation.Validate, shop: shop, shod: shod);
                }
            }

            _historyLogService.PrepareForInsert($"نمایش سوابق پرونده {shop} و ردیف {radif}", EnumFormName.Dv_savabegh, EnumOperation.Get, shop: shop, shod: shod);

            return results;
        }
        catch (Exception e)
        {
            _historyLogService.PrepareForInsert($"خطا در نمایش سوابق پرونده {shop} : {e.Message}", EnumFormName.Dv_savabegh, EnumOperation.Get, shop: shop, shod: Convert.ToInt32(shod));
            throw;
        }

    }
    #endregion

    #region Insert
    public bool InsertByModel(Dv_savabeghDTO savabeghDTO, string mtablename)
    {
        try
        {
            var savabegh = _mapper.Map<Dv_savabegh>(savabeghDTO);
            savabegh.CreateDateTime = DateTime.UtcNow.AddHours(3.5);
            savabegh.d_radif = (int)_myFunctions.GetRadif(mtablename, Convert.ToDecimal(savabegh.shop));
            _context.Dv_savabegh.Add(savabegh);
            var res = _context.SaveChanges();
            if (res > 0)
            {
                _historyLogService.PrepareForInsert($"ثبت سوابق پرونده {savabegh.shop}", EnumFormName.Dv_savabegh, EnumOperation.Post);
                return true;
            }
            _historyLogService.PrepareForInsert($"خطا در ثبت سوابق پرونده {savabegh.shop}", EnumFormName.Dv_savabegh, EnumOperation.Post);
            return false;
        }
        catch (Exception e)
        {
            _historyLogService.PrepareForInsert($"خطا در ثبت سوابق پرونده {savabeghDTO.shop} : {e.Message}", EnumFormName.Dv_savabegh, EnumOperation.Post);
            throw;
        }

    }
    #endregion

    #region Update
    public void Update(Dv_savabeghDTO savabegh)
    {
        try
        {
            var mapdata = _mapper.Map<Dv_savabegh>(savabegh);
            _context.Dv_savabegh.Update(mapdata);
            _context.SaveChanges();
            _historyLogService.PrepareForInsert($"ویرایش سوابق پرونده {mapdata.shop} ", EnumFormName.Dv_savabegh, EnumOperation.Update);
        }
        catch (Exception e)
        {
            _historyLogService.PrepareForInsert($"خطا در ویرایش سوابق پرونده {savabegh.shop} : {e.Message}", EnumFormName.Dv_savabegh, EnumOperation.Update);
            throw;
        }

    }
    #endregion

    #region Delete
    public bool DeleteByModel(Dv_savabeghDTO savabegh)
    {
        try
        {
            var mapData = _mapper.Map<Dv_savabegh>(savabegh);
            var d = _context.Dv_savabegh.SingleOrDefault(c => c.shop == mapData.shop && c.d_radif == mapData.d_radif && c.mtable_name == mapData.mtable_name);

            if (d != null)
                _context.Dv_savabegh.Remove(d);

            _context.SaveChanges();

            _historyLogService.PrepareForInsert($"حذف سوابق پرونده {savabegh.shop} ", EnumFormName.Dv_savabegh, EnumOperation.Delete);

            return true;
        }
        catch (Exception e)
        {
            _historyLogService.PrepareForInsert($"خطا در حذف سوابق پرونده {savabegh.shop} : {e.Message}", EnumFormName.Dv_savabegh, EnumOperation.Delete);
            throw;
        }

    }
    #endregion
}
