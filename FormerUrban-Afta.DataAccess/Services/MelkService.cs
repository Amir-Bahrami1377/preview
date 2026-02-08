namespace FormerUrban_Afta.DataAccess.Services
{
    public class MelkService : IMelkService
    {
        private readonly IEncryptionService _encryptionService;
        private readonly FromUrbanDbContext _context;
        private readonly IMapper _mapper;
        private readonly MyFunctions _myFunctions;
        private readonly IHistoryLogService _historyLogService;
        private readonly IAuditService _auditService;

        public MelkService(FromUrbanDbContext context, IMapper mapper, MyFunctions myFunctions, IHistoryLogService historyLogService, IEncryptionService encryptionService, IAuditService auditService)
        {
            _context = context;
            _mapper = mapper;
            _myFunctions = myFunctions;
            _historyLogService = historyLogService;
            _encryptionService = encryptionService;
            _auditService = auditService;
        }

        #region Shit

        private async Task<MelkDto> DecryptInfo2(MelkDto entity)
        {
            if (entity?.address != null)
                entity.address = await _encryptionService.DecryptAsync(entity.address);

            if (entity?.codeposti != null)
                entity.codeposti = await _encryptionService.DecryptAsync(entity.codeposti);

            if (entity?.tel != null)
                entity.tel = await _encryptionService.DecryptAsync(entity.tel);

            return entity;
        }
        private async Task<Melk> EncryptInfo2(Melk obj)
        {
            if (obj.address != null)
                obj.address = await _encryptionService.EncryptAsync(obj.address);

            if (obj.codeposti != null)
                obj.codeposti = await _encryptionService.EncryptAsync(obj.codeposti);

            if (obj.tel != null)
                obj.tel = await _encryptionService.EncryptAsync(obj.tel);

            return obj;
        }

        public static bool CheckHash(Melk obj)
        {
            var hash = CipherService.IsEqual(obj.ToString(), obj.Hashed);
            return hash;
        }
        #endregion

        #region Get

        public async Task<MelkDto> GetData(int shop, int shod)
        {
            try
            {
                var model = _context.Melk.FirstOrDefault(c => c.shop == shop && c.sh_Darkhast == shod);
                if (model == null)
                {
                    _historyLogService.PrepareForInsert($"خطا در دریافت اطلاعات ملک پرونده {shop} یافت نشد", EnumFormName.Melk, EnumOperation.Get, shop: shop, shod: shod);
                    return new MelkDto();
                }

                var result = _mapper.Map<MelkDto>(model);
                result.IsValid = CheckHash(model);
                result = await DecryptInfo2(result);
                if (!result.IsValid)
                    _historyLogService.PrepareForInsert($"رد صحت سنجی داده جدول ملک با پرونده {result.shop}", EnumFormName.Melk, EnumOperation.Validate);

                _historyLogService.PrepareForInsert($"دریافت اطلاعات ملک پرونده {shop}", EnumFormName.Melk, EnumOperation.Get, shop: shop, shod: shod);

                return result;
            }
            catch (Exception e)
            {
                _historyLogService.PrepareForInsert($"خطا در دریافت اطلاعات ملک پرونده {shop} : {e.Message}", EnumFormName.Melk, EnumOperation.Get, shop: shop, shod: shod);

                throw;
            }
        }
        public async Task<MelkDto> GetDataByShop(int shop)
        {
            try
            {
                var model = _context.Melk.FirstOrDefault(c => c.shop == shop && c.Active == true);
                if (model == null)
                {
                    _historyLogService.PrepareForInsert($"خطا در دریافت اطلاعات ملک پرونده {shop} یافت نشد", EnumFormName.Melk, EnumOperation.Get, shop: shop);
                    return new MelkDto();
                }

                var result = _mapper.Map<MelkDto>(model);
                result.IsValid = CheckHash(model);
                result = await DecryptInfo2(result);
                if (!result.IsValid)
                    _historyLogService.PrepareForInsert($"رد صحت سنجی داده جدول ملک با پرونده {result.shop}", EnumFormName.Melk, EnumOperation.Validate);
                // Log activity
                _historyLogService.PrepareForInsert($"دریافت اطلاعات ملک پرونده {shop}", EnumFormName.Melk, EnumOperation.Get, shop: shop, shod: Convert.ToInt32(model.sh_Darkhast ?? 0));

                return result;
            }
            catch (Exception e)
            {
                _historyLogService.PrepareForInsert($"خطا در دریافت اطلاعات ملک پرونده {shop} : {e.Message}", EnumFormName.Melk, EnumOperation.Get,
                    shop: shop);

                throw;
            }
        }
        public decimal GetRadif(int shop, long shod) => _context.Melk.Where(m => m.shop == shop && m.sh_Darkhast == shod).Select(m => m.radif).FirstOrDefault();

        public decimal GetRadif(int shop) => _context.Melk.Where(m => m.shop == shop && m.Active == true).Select(m => m.radif).FirstOrDefault();

        public async Task<MelkDto> GetDataByRadif(int shop, int radif)
        {
            try
            {
                var model = _context.Melk.FirstOrDefault(c => c.shop == shop && c.radif == radif);
                if (model == null)
                {
                    _historyLogService.PrepareForInsert($"خطا در دریافت اطلاعات ملک پرونده {shop} یافت نشد", EnumFormName.Melk, EnumOperation.Get, shop: shop);
                    return new MelkDto();
                }

                var task = _mapper.Map<MelkDto>(model);
                task.IsValid = CheckHash(model);
                var result = await DecryptInfo2(task);

                _historyLogService.PrepareForInsert($"دریافت اطلاعات ملک پرونده {shop}", EnumFormName.Melk, EnumOperation.Get, shop: shop, shod: Convert.ToInt32(model.sh_Darkhast ?? 0));

                return result;
            }
            catch (Exception e)
            {
                _historyLogService.PrepareForInsert($"خطا در دریافت اطلاعات ملک پرونده {shop} : {e.Message}", EnumFormName.Melk, EnumOperation.Get,
                    shop: shop);

                throw;
            }
        }

        public async Task<MelkDto> GetByIdAsNoTracking(long id)
        {
            var data = _context.Melk.AsNoTracking().FirstOrDefault(x => x.Identity == id);
            var model = _mapper.Map<MelkDto>(data);
            model.IsValid = CheckHash(data);
            model = await DecryptInfo2(model);

            return model;
        }

        #endregion

        #region Inset
        public async Task InsetByModel(MelkDto obj)
        {
            try
            {
                var model = _mapper.Map<Melk>(obj);
                model = await EncryptInfo2(model);
                _context.Melk.Add(model);
                _context.SaveChanges();
                _historyLogService.PrepareForInsert($"ثبت اطلاعات ملک پرونده {obj.shop}", EnumFormName.Melk, EnumOperation.Post,
                    shop: obj.shop, shod: Convert.ToInt32(obj.sh_Darkhast ?? 0));
            }
            catch (Exception e)
            {
                _historyLogService.PrepareForInsert($"خطا در ثبت اطلاعات ملک پرونده {obj.shop}", EnumFormName.Melk, EnumOperation.Post,
                    shop: obj.shop, shod: Convert.ToInt32(obj.sh_Darkhast ?? 0));
                throw;
            }
        }
        #endregion

        #region Update
        public async Task Update(MelkDto obj)
        {
            try
            {
                var model = _mapper.Map<Melk>(obj);
                model = await EncryptInfo2(model);
                var oldModel = await GetByIdAsNoTracking(model.Identity);
                _context.Melk.Update(model);
                await _context.SaveChangesAsync();
                _auditService.GetDifferences<MelkDto>(oldModel, obj, oldModel.Identity.ToString(), EnumFormName.Melk, EnumOperation.Update);

                _historyLogService.PrepareForInsert($"بروزرسانی اطلاعات ملک پرونده {obj.shop}", EnumFormName.Melk, EnumOperation.Update,
                    shop: obj.shop, shod: Convert.ToInt32(obj.sh_Darkhast ?? 0));
            }
            catch (Exception e)
            {
                _historyLogService.PrepareForInsert($"خطا در بروزرسانی اطلاعات ملک پرونده {obj.shop}", EnumFormName.Melk, EnumOperation.Update,
                    shop: obj.shop, shod: Convert.ToInt32(obj.sh_Darkhast ?? 0));
                throw;
            }
        }
        #endregion

        public void Copy_Melk(int Shop, long ShodG, long ShodJ)
        {
            try
            {
                decimal radifJadid = _myFunctions.MaxRadif(Shop);

                if (radifJadid == 0)
                    throw new Exception("ورود اطلاعات برای این پرونده انجام نشده است.");

                decimal radifGhadim = _myFunctions.GetRadif("melk", Shop, ShodG);

                if (radifGhadim == 0)
                    throw new Exception("اطلاعات درخواست قبلی وجود ندارد.");

                radifJadid++;

                //ایجاد یک ردیف جدید در جدول نوع ملک با شماره ردیف جدید و شماره درخواست جدید
                var melk = _context.Melk.Where(m => m.shop == Shop && m.radif == radifGhadim).AsNoTracking().SingleOrDefault();
                if (melk != null)
                {
                    Melk m = melk;
                    m.Identity = 0;
                    m.Active = false;
                    m.radif = (int)radifJadid;
                    m.sh_Darkhast = ShodJ;
                    _context.Melk.Add(m);
                }

                if (CheckHash(melk))
                {
                    _historyLogService.PrepareForInsert($"رد صحت سنجی در کپی اطلاعات ملک پرونده {Shop}", EnumFormName.Melk, EnumOperation.Validate, shop: Shop, shod: Convert.ToInt32(ShodJ));
                    throw new Exception("رد صحت سنجی داده");
                }

                _myFunctions.Copy_DvTable("melk", "Dv_malekin", Shop, radifJadid, radifGhadim);
                _myFunctions.Copy_DvTable("melk", "Dv_savabegh", Shop, radifJadid, radifGhadim);
                _historyLogService.PrepareForInsert($"کپی اطلاعات ملک پرونده {Shop}", EnumFormName.Melk, EnumOperation.Post, shop: Shop, shod: Convert.ToInt32(ShodJ));
            }
            catch (Exception e)
            {
                _historyLogService.PrepareForInsert($"خطا در کپی اطلاعات ملک پرونده {Shop}", EnumFormName.Melk, EnumOperation.Post, shop: Shop, shod: Convert.ToInt32(ShodJ));
                throw;
            }
        }

        public void CopyUpdate_Melk(int shop, long shodG, long shodJ)
        {
            try
            {
                var modelItemMelk = _context.Melk.FirstOrDefault(m => m.shop == shop && m.sh_Darkhast == shodG);
                if (modelItemMelk == null)
                    return;
                if (CheckHash(modelItemMelk))
                {
                    _historyLogService.PrepareForInsert($"رد صحت سنجی در کپی اطلاعات ملک پرونده {modelItemMelk.shop}", EnumFormName.Melk, EnumOperation.Validate,
                        shop: modelItemMelk.shop, shod: Convert.ToInt32(modelItemMelk.sh_Darkhast ?? 0));
                    throw new Exception("رد صحت سنجی داده");
                }

                decimal radifJadid = _myFunctions.GetRadif("melk", shop, shodJ);
                decimal radifGhadim = _myFunctions.GetRadif("melk", shop, shodG);

                var listMelk = _context.Melk.Where(m => m.shop == shop && m.sh_Darkhast == shodJ).AsNoTracking().ToList();

                // Update each target record
                foreach (var m in listMelk)
                {
                    // Copy all properties from modelItemMelk to m
                    _context.Entry(m).CurrentValues.SetValues(modelItemMelk);

                    // Set specific properties
                    m.radif = (int)radifJadid;
                    m.Active = false;
                    m.sh_Darkhast = shodJ;

                    // Attach the entity and mark it as modified
                    _context.Entry(m).State = EntityState.Modified;
                }

                _myFunctions.Update_DvTable("melk", "Dv_malekin", Convert.ToDecimal(shop), radifJadid, radifGhadim);
                _myFunctions.Update_DvTable("melk", "Dv_savabegh", Convert.ToDecimal(shop), radifJadid, radifGhadim);
                _historyLogService.PrepareForInsert($"کپی اطلاعات ملک پرونده {shop}", EnumFormName.Melk, EnumOperation.Post, shop: shop, shod: Convert.ToInt32(shodJ));
            }
            catch (Exception e)
            {
                _historyLogService.PrepareForInsert($"خطا در کپی اطلاعات ملک پرونده {shop}", EnumFormName.Melk, EnumOperation.Post, shop: shop, shod: Convert.ToInt32(shodJ));
                throw;
            }
        }
    }
}
