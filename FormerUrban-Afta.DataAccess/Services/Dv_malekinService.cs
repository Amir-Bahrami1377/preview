namespace FormerUrban_Afta.DataAccess.Services
{
    public class Dv_malekinService : IDv_malekinService
    {
        private readonly FromUrbanDbContext _context;
        private readonly MyFunctions _myFunctions;
        private readonly IMapper _mapper;
        private readonly IHistoryLogService _historyLogService;
        private readonly IEncryptionService _encryptionService;
        private readonly IAuditService _auditService;


        public Dv_malekinService(FromUrbanDbContext context, MyFunctions myFunctions, IMapper mapper, IHistoryLogService historyLogService, IEncryptionService encryptionService, IAuditService auditService)
        {
            _context = context;
            _myFunctions = myFunctions;
            _mapper = mapper;
            _historyLogService = historyLogService;
            _encryptionService = encryptionService;
            _auditService = auditService;
        }

        #region Shit
        private async Task<Dv_malekinDTO> DecryptInfo2(Dv_malekinDTO obj)
        {
            if (obj.mob != null)
                obj.mob = await _encryptionService.DecryptAsync(obj.mob);

            if (obj.kodemeli != null)
                obj.kodemeli = await _encryptionService.DecryptAsync(obj.kodemeli);

            if (obj.address != null)
                obj.address = await _encryptionService.DecryptAsync(obj.address);

            if (obj.tel != null)
                obj.tel = await _encryptionService.DecryptAsync(obj.tel);

            if (obj.sh_sh != null)
                obj.sh_sh = await _encryptionService.DecryptAsync(obj.sh_sh);

            if (obj.name != null)
                obj.name = await _encryptionService.DecryptAsync(obj.name);

            if (obj.family != null)
                obj.family = await _encryptionService.DecryptAsync(obj.family);

            if (obj.father != null)
                obj.father = await _encryptionService.DecryptAsync(obj.father);

            return obj;
        }
        private async Task<Dv_malekin> EncryptInfo2(Dv_malekin obj)
        {
            if (obj.address != null)
                obj.address = await _encryptionService.EncryptAsync(obj.address);

            if (obj.kodemeli != null)
                obj.kodemeli = await _encryptionService.EncryptAsync(obj.kodemeli);

            if (obj.tel != null)
                obj.tel = await _encryptionService.EncryptAsync(obj.tel);

            if (obj.mob != null)
                obj.mob = await _encryptionService.EncryptAsync(obj.mob);

            if (obj.sh_sh != null)
                obj.sh_sh = await _encryptionService.EncryptAsync(obj.sh_sh);

            if (obj.name != null)
                obj.name = await _encryptionService.EncryptAsync(obj.name);

            if (obj.family != null)
                obj.family = await _encryptionService.EncryptAsync(obj.family);

            if (obj.father != null)
                obj.father = await _encryptionService.EncryptAsync(obj.father);
            return obj;
        }
        public static bool CheckHash(Dv_malekin obj)
        {
            var hash = CipherService.IsEqual(obj.ToString(), obj.Hashed);
            return hash;
        }
        #endregion

        #region Get
        public async Task<List<Dv_malekinDTO>> GetMalekinForParvande(int shop, long shod)
        {
            var noeParvande = _myFunctions.GetNoeParvandeh(shop);
            decimal radif = 0;
            if (noeParvande == 2)
            {
                shop = (int)_myFunctions.GetShoPMelk(shop);
                radif = _myFunctions.GetRadif("melk", shop, shod);
            }
            else
                radif = _myFunctions.GetRadif(shop, shod);

            var malekin = await GetMalekin(shop, radif);
            return malekin;
        }

        public async Task<List<Dv_malekinDTO>> GetMalekin(int shop, decimal radif)
        {
            // Normalize table name
            string tableName = _myFunctions.GetStrNoeParvandeh(shop) switch
            {
                "sakhteman" => "melk",
                var name => name
            };

            try
            {
                // Fetch and project data efficiently
                var malekinData = _context.Dv_malekin.Where(d => d.shop == shop && d.d_radif == radif && d.mtable_name == tableName).ToList();

                // Map to DTOs and validate
                var tasks = malekinData.Select(async item =>
                {
                    var dto = _mapper.Map<Dv_malekinDTO>(item);
                    dto.IsValid = CheckHash(item);
                    return await DecryptInfo2(dto);
                });

                var results = (await Task.WhenAll(tasks)).OrderByDescending(x => x.Identity).ToList();

                // Log invalid records in a batch
                var invalidRecords = results.Where(dto => !dto.IsValid)
                    .Select(dto => $"رد صحت سنجی داده مالکین یا شماره پرونده {shop} و ردیف {radif} و آیدی {dto.id}").ToList();

                if (invalidRecords.Any())
                {
                    foreach (var message in invalidRecords)
                    {
                        _historyLogService.PrepareForInsert(message, EnumFormName.Dv_malekin, EnumOperation.Validate, shop: shop);
                    }
                }

                // Log activity
                _historyLogService.PrepareForInsert($"دریافت اطلاعات مالکین پرونده {shop} و ردیف {radif}", EnumFormName.Dv_malekin, EnumOperation.Get, shop: shop);

                return results;
            }
            catch (Exception ex)
            {
                // Log exception (use your preferred logging framework)
                _historyLogService.PrepareForInsert($"خطا در دریافت اطلاعات مالکین پرونده {shop}: {ex.Message}", EnumFormName.Dv_malekin, EnumOperation.Get, shop: shop);
                throw; // Rethrow or handle as needed
            }
        }

        public async Task<Dv_malekinDTO> GetById(long id)
        {
            var data = _context.Dv_malekin.FirstOrDefault(x => x.Identity == id);
            if (data == null)
            {
                _historyLogService.PrepareForInsert("داده مالک یافت نشد", EnumFormName.Dv_malekin, EnumOperation.Get);
                return null;
            }

            var model = _mapper.Map<Dv_malekinDTO>(data);
            model.IsValid = CheckHash(data);
            if (!model.IsValid)
                _historyLogService.PrepareForInsert($"رد صحت سنجی داده مالک با شماره پرونده {model.shop} و آیدی {id} و ردیف {model.d_radif}", EnumFormName.Dv_malekin, EnumOperation.Validate, shop: data.shop);

            _historyLogService.PrepareForInsert($"دریافت اطلاعات مالک با آیدی {id}", EnumFormName.Dv_malekin, EnumOperation.Get, shop: data.shop);

            return await DecryptInfo2(model);
        }

        public async Task<Dv_malekinDTO> GetByIdAsNoTracking(long id)
        {
            var data = await _context.Dv_malekin.AsNoTracking().FirstOrDefaultAsync(x => x.Identity == id);
            if (data == null)
            {
                _historyLogService.PrepareForInsert("داده مالک یافت نشد", EnumFormName.Dv_malekin, EnumOperation.Get);
                return null;
            }

            var model = _mapper.Map<Dv_malekinDTO>(data);
            model.IsValid = CheckHash(data);

            return await DecryptInfo2(model);
        }

        #endregion

        #region Update

        public async Task<bool> Update(Dv_malekinDTO malekin)
        {
            var data = _mapper.Map<Dv_malekin>(malekin);
            data = await EncryptInfo2(data);
            var oldModel = await GetByIdAsNoTracking(data.Identity);

            _context.Dv_malekin.Update(data);
            var res = await _context.SaveChangesAsync();
            if (res > 0)
            {
                _auditService.GetDifferences<Dv_malekinDTO>(oldModel, malekin, oldModel.Identity.ToString(), EnumFormName.Dv_malekin, EnumOperation.Update);

                _historyLogService.PrepareForInsert($"ویرایش اطلاعات مالکین پرونده {malekin.shop} با آیدی {malekin.id} و ردیف {malekin.d_radif}",
                    EnumFormName.Dv_malekin, EnumOperation.Update, shop: malekin.shop);
                return true;
            }

            _historyLogService.PrepareForInsert($"خطا در ویرایش اطلاعات مالکین پرونده {malekin.shop} با آیدی {malekin.id} و ردیف {malekin.d_radif}",
                EnumFormName.Dv_malekin, EnumOperation.Update, shop: malekin.shop);
            return false;
        }
        #endregion

        #region Insert
        public async Task<bool> InsertByModel(Dv_malekinDTO malek, string mtablename)
        {
            var mapData = _mapper.Map<Dv_malekin>(malek);
            mapData.mtable_name = mtablename;
            if (malek.d_radif <= 0)
                mapData.d_radif = (int)_myFunctions.GetRadif(mtablename, malek.shop);

            mapData.id = _context.Dv_malekin.Where(c => c.shop == mapData.shop && c.d_radif == mapData.d_radif && c.mtable_name == mtablename)
                    .Select(c => c.id).ToList().DefaultIfEmpty(0).Max() + 1;

            mapData = await EncryptInfo2(mapData);
            await _context.Dv_malekin.AddAsync(mapData);
            var res = await _context.SaveChangesAsync();
            if (res > 0)
            {
                _historyLogService.PrepareForInsert($"ثبت اطلاعات مالکین پرونده {mapData.shop} با آیدی {mapData.id} و ردیف {mapData.d_radif}", EnumFormName.Dv_malekin, EnumOperation.Post, shop: malek.shop);
                return true;
            }
            _historyLogService.PrepareForInsert($"خطا در ثبت اطلاعات مالکین پرونده {mapData.shop} ", EnumFormName.Dv_malekin, EnumOperation.Post, shop: malek.shop);
            return false;
        }
        #endregion

        #region Delete
        public bool DeleteMalek(long identity, int shop)
        {
            var malekin = _context.Dv_malekin.FirstOrDefault(c => c.Identity == identity);
            if (malekin == null)
            {
                _historyLogService.PrepareForInsert($"خطا در حذف مالک پرونده {shop} مالک یافت نشد", EnumFormName.Dv_malekin, EnumOperation.Delete, shop: shop);
                return false;
            }
            _context.Dv_malekin.Remove(malekin);
            _context.SaveChanges();
            _historyLogService.PrepareForInsert($"حذف  مالک با پرونده {shop} و آیدی {malekin.id} و ردیف {malekin.d_radif}", EnumFormName.Dv_malekin, EnumOperation.Delete, shop: shop);
            return true;
        }
        #endregion
    }
}
