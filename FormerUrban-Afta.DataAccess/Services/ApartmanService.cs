namespace FormerUrban_Afta.DataAccess.Services
{
    public class ApartmanService : IApartmanService
    {
        private readonly FromUrbanDbContext _context;
        private readonly IMapper _mapper;
        private readonly MyFunctions _myFunctions;
        private readonly IHistoryLogService _historyLogService;
        private readonly IEncryptionService _encryptionService;
        private readonly IAuditService _auditService;

        public ApartmanService(FromUrbanDbContext context, IMapper mapper, IHistoryLogService historyLogService, MyFunctions myFunctions, IEncryptionService encryptionService, IAuditService auditService)
        {
            _context = context;
            _mapper = mapper;
            _myFunctions = myFunctions;
            _historyLogService = historyLogService;
            _encryptionService = encryptionService;
            _auditService = auditService;
        }

        #region Encryption

        private async Task<ApartmanDto> DecryptInfo2(ApartmanDto obj)
        {
            if (!string.IsNullOrWhiteSpace(obj.address))
                obj.address = await _encryptionService.DecryptAsync(obj.address);
            if (!string.IsNullOrWhiteSpace(obj.tel))
                obj.tel = await _encryptionService.DecryptAsync(obj.tel);

            if (!string.IsNullOrWhiteSpace(obj.codeposti))
                obj.codeposti = await _encryptionService.DecryptAsync(obj.codeposti);

            return obj;
        }
        private async Task<Apartman> EncryptInfo2(Apartman obj)
        {
            if (obj.address != null)
                obj.address = await _encryptionService.EncryptAsync(obj.address);

            if (obj.tel != null)
                obj.tel = await _encryptionService.EncryptAsync(obj.tel);

            if (obj.codeposti != null)
                obj.codeposti = await _encryptionService.EncryptAsync(obj.codeposti);
            return obj;
        }

        public static bool CheckHash(Apartman obj)
        {
            var hash = CipherService.IsEqual(obj.ToString(), obj.Hashed);
            return hash;
        }

        #endregion

        public bool updateActiveLastRow(int Shop)
        {
            var activeRow = _context.Apartman.Where(c => c.shop == Shop && c.Active == true).ToList();
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

        #region Get

        public async Task<ApartmanDto> GetRowByShop(int shop)
        {
            _historyLogService.PrepareForInsert($"دریافت اطلاعات آپارتمان با شماره پرونده {shop}",
                EnumFormName.Apartman, EnumOperation.Get, shop: shop);

            var data = _context.Apartman.FirstOrDefault(c => c.shop == shop && c.Active == true);
            if (data == null)
            {
                _historyLogService.PrepareForInsert($"خطا در دریافت اطلاعات آپارتمان با شماره پرونده {shop}",
                    EnumFormName.Apartman, EnumOperation.Get, shop: shop);
                return null;
            }

            var mapped = _mapper.Map<ApartmanDto>(data);
            mapped.IsValid = CheckHash(data);
            mapped = await DecryptInfo2(mapped);
            if (!mapped.IsValid)
            {
                _historyLogService.PrepareForInsert($"رد صحت سنجی داده آپارتمان با شماره پرونده  {mapped.shop} و شماره درخواست {mapped.sh_Darkhast}",
                    EnumFormName.Apartman, EnumOperation.Validate, shop: mapped.shop, shod: Convert.ToInt32(mapped.sh_Darkhast));
            }

            _historyLogService.PrepareForInsert($"دریافت اطلاعات آپارتمان با شماره پرونده {shop} و شماره درخواست {mapped.sh_Darkhast}",
                EnumFormName.Apartman, EnumOperation.Get, shop: mapped.shop, shod: Convert.ToInt32(mapped.sh_Darkhast));
            return mapped;
        }

        public async Task<ApartmanDto> GetRowByShop(int shop, int shod)
        {
            var data = _context.Apartman.FirstOrDefault(c => c.shop == shop && c.sh_Darkhast == shod);
            if (data == null)
            {
                _historyLogService.PrepareForInsert($"خطا در دریافت اطلاعات آپارتمان با شماره درخواست {shod}",
                    EnumFormName.Apartman, EnumOperation.Get, shop: shop, shod: shod);
                return null;
            }

            var mapped = _mapper.Map<ApartmanDto>(data);
            mapped.IsValid = CheckHash(data);
            mapped = await DecryptInfo2(mapped);
            if (!mapped.IsValid)
            {
                _historyLogService.PrepareForInsert($"رد صحت سنجی داده آپارتمان با شماره پرونده  {mapped.Identity} و  شماره درخواست {mapped.sh_Darkhast}",
                    EnumFormName.Apartman, EnumOperation.Validate, shop: shop, shod: shod);
            }

            _historyLogService.PrepareForInsert($"دریافت اطلاعات آپارتمان با شماره درخواست {shod}",
                EnumFormName.Apartman, EnumOperation.Get, shop: shop, shod: shod);
            return mapped;
        }

        public async Task<ApartmanDto> GetByIdAsNoTracking(long id)
        {
            var data = await _context.Apartman.AsNoTracking().FirstOrDefaultAsync(x => x.Identity == id);
            var model = _mapper.Map<ApartmanDto>(data);
            model.IsValid = CheckHash(data);
            model = await DecryptInfo2(model);

            return model;
        }

        #endregion

        #region Insert

        public async Task InsertByModel(ApartmanDto obj)
        {
            var model = _mapper.Map<Apartman>(obj);
            model = await EncryptInfo2(model);
            _context.Apartman.Add(model);
            var res = await _context.SaveChangesAsync();

            if (res > 0)
                _historyLogService.PrepareForInsert($"ثبت اطلاعات آپارتمان با شماره پرونده {obj.shop}", EnumFormName.Apartman, EnumOperation.Post, shop: obj.shop, shod: Convert.ToInt32(obj.sh_Darkhast));
            else
                _historyLogService.PrepareForInsert($"خطا در ثبت اطلاعات آپارتمان با شماره پرونده {obj.shop}", EnumFormName.Apartman, EnumOperation.Post, shop: obj.shop, shod: Convert.ToInt32(obj.sh_Darkhast));
        }


        #endregion

        #region Update

        public async Task UpdateByModel(ApartmanDto apartman)
        {
            var model = _mapper.Map<Apartman>(apartman);
            model = await EncryptInfo2(model);
            var oldModel = await GetByIdAsNoTracking(model.Identity);

            _context.Apartman.Update(model);
            var res = await _context.SaveChangesAsync();
            if (res > 0)
            {
                _auditService.GetDifferences<ApartmanDto>(oldModel, apartman, oldModel.Identity.ToString(), EnumFormName.Apartman, EnumOperation.Update);

                _historyLogService.PrepareForInsert($"بروزرسانی آپارتمان با شماره پرونده {apartman.shop} و  شماره درخواست {apartman.sh_Darkhast}",
                    EnumFormName.Apartman, EnumOperation.Update, shop: apartman.shop, shod: Convert.ToInt32(apartman.sh_Darkhast));
            }
            _historyLogService.PrepareForInsert($"خطا در بروزرسانی آپارتمان با شماره پرونده {apartman.shop}  شماره درخواست {apartman.sh_Darkhast}", EnumFormName.Apartman,
                EnumOperation.Update, shop: apartman.shop, shod: Convert.ToInt32(apartman.sh_Darkhast));
        }

        public void Copy_Apar(int Shop, long ShodG, long ShodJ)
        {
            decimal radifJ = _myFunctions.MaxRadif(Shop);
            if (radifJ == 0)
            {
                _historyLogService.PrepareForInsert($"کپی اطلاعات آپارتمان با شماره پرونده {Shop} باخطای شماره ردیف مواجه شده است",
                    EnumFormName.Apartman, EnumOperation.Update, shop: Shop, shod: (int)ShodJ);

                throw new ApplicationException("ورود اطلاعات برای این پرونده انجام نشده است.");
            }

            radifJ++;
            decimal radifG = _myFunctions.GetRadif("aparteman", Shop, ShodG);

            if (radifG == 0)
            {
                _historyLogService.PrepareForInsert($"کپی اطلاعات آپارتمان با شماره پرونده {Shop} باخطای شماره ردیف مواجه شده است",
                    EnumFormName.Apartman, EnumOperation.Update, shop: Shop, shod: (int)ShodJ);

                throw new ApplicationException("اطلاعات درخواست قبلی وجود ندارد.");
            }

            var ap = _context.Apartman.Where(m => m.shop == Shop && m.radif == radifG).AsNoTracking().SingleOrDefault();
            if (ap != null)
            {
                Apartman a = ap;
                a.Identity = 0;
                a.Active = false;
                a.radif = (int)radifJ;
                a.sh_Darkhast = ShodJ;
                _context.Apartman.Add(a);
            }
            // Copy_DvTable("apartman","", mShop, RadifJ);
            // Copy_DvTable("apartman", "Dv_pelaksabti", mShop, RadifJ, RadifG);
            _myFunctions.Copy_DvTable("aparteman", "Dv_malekin", Shop, radifJ, radifG);
            _myFunctions.Copy_DvTable("aparteman", "Dv_savabegh", Shop, radifJ, radifG);
            _myFunctions.Copy_DvTable("aparteman", "Dv_karbari", Shop, radifJ, radifG);

            _historyLogService.PrepareForInsert($"کپی اطلاعات آپارتمان با شماره پرونده {Shop} و شماره ردیف {radifJ}",
                EnumFormName.Apartman, EnumOperation.Update, shop: Shop, shod: (int)ShodJ);

        }

        public void CopyUpdate_Apartman(int shop, long shodG, long shodJ)
        {
            var apartman = _context.Apartman.FirstOrDefault(m => m.shop == shop && m.sh_Darkhast == shodG);
            if (apartman == null)
            {
                _historyLogService.PrepareForInsert($" اطلاعات آپارتمان با شماره پرونده {shop} یافت نشد",
                    EnumFormName.Apartman, EnumOperation.Update, shop: shop, shod: (int)shodJ);
                return;
            }

            decimal radifJ = _myFunctions.GetRadif("aparteman", shop, shodJ);
            decimal radifG = _myFunctions.GetRadif("aparteman", shop, shodG);
            var lstApartman = _context.Apartman.Where(m => m.shop == shop && m.sh_Darkhast == shodJ).ToList();

            foreach (var m in lstApartman)
            {
                _context.Entry(m).CurrentValues.SetValues(apartman); // Copy properties from sakhteman
                m.radif = (int)radifJ;                                     // Set radif
                m.Active = false;                                     // Deactivate
                m.sh_Darkhast = shodJ;                                // Set sh_Darkhast (optional)
                _context.Entry(m).State = EntityState.Modified;       // Mark as modified
            }

            _myFunctions.Update_DvTable("aparteman", "Dv_malekin", Convert.ToDecimal(shop), radifJ, radifG);
            _myFunctions.Update_DvTable("aparteman", "Dv_savabegh", Convert.ToDecimal(shop), radifJ, radifG);
            _myFunctions.Update_DvTable("aparteman", "Dv_karbari", Convert.ToDecimal(shop), radifJ, radifG);
            _historyLogService.PrepareForInsert($"کپی اطلاعات آپارتمان با شماره پرونده {shop}",
                EnumFormName.Apartman, EnumOperation.Update, shop: shop, shod: (int)shodJ);
        }

        #endregion

    }
}
