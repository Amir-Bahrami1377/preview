using System.Globalization;

namespace FormerUrban_Afta.DataAccess.Utilities
{
    public class MyFunctions
    {
        public FromUrbanDbContext _context { get; }

        public MyFunctions(FromUrbanDbContext context)
        {
            _context = context;
        }

        public string GetCodNosazi(long shop) =>
            _context.Parvandeh.Where(c => c.shop == shop).Select(s => s.codeN).SingleOrDefault() ?? string.Empty;

        public decimal GetRadif(int shop, long shod)
        {
            var tableName = GetStrNoeParvandeh(shop);
            return GetRadif(tableName, shop, shod);
        }

        public decimal GetRadif(string masterTable, decimal shop)
        {
            if (shop == 0)
                return 0;

            switch (masterTable.ToLower())
            {
                case "melk":
                    {
                        var dataMelk = _context.Melk.Where(p => p.shop == shop && p.Active == true).Select(s => s.radif)
                            .FirstOrDefault();
                        return Convert.ToDecimal(dataMelk);
                    }
                case "sakhteman":
                    {
                        var dataSakh = _context.Sakhteman.Where(p => p.shop == shop && p.Active == true)
                            .Select(s => s.radif)
                            .FirstOrDefault();
                        return Convert.ToDecimal(dataSakh);
                    }
                case "aparteman":
                    {
                        var dataApar = _context.Apartman.Where(p => p.shop == shop && p.Active == true).Select(s => s.radif)
                            .FirstOrDefault();
                        return Convert.ToDecimal(dataApar);
                    }
                default:
                    return 0;
            }
        }

        public string GetStrNoeParvandeh(double shop)
        {
            if (shop <= 0)
                return "";

            var data = _context.Parvandeh.Where(p => p.shop == shop)
                .Select(s => new { s.shomelk, s.sakhteman, s.apar, s.senfi }).FirstOrDefault();

            if (data == null)
                return "";

            int melk = Convert.ToInt32(data.shomelk);
            int sakh = (int)data.sakhteman;
            int apar = (int)data.apar;
            int senf = (int)data.senfi;

            if ((melk > 0) & (sakh == 0))
                return "melk";
            if ((sakh > 0) & (apar == 0) & (senf == 0))
                return "sakhteman";
            if ((apar > 0) & (senf == 0))
                return "aparteman";
            if (senf > 0)
                return "senfi";
            return "";
        }

        public decimal GetRadif(string masterTable, int shop, long shod)
        {
            decimal radif = 0;

            if (shop == 0)
                return 0;

            if (masterTable.ToLower() == "melk")
            {
                var ListMelk = _context.Melk.Where(p => p.shop == shop);

                if (shod > 0)
                    radif = ListMelk.Where(p => p.sh_Darkhast == shod)
                        .Select(s => s.radif).FirstOrDefault();
                else
                    radif = ListMelk.Where(p => p.Active == true)
                        .Select(s => s.radif).FirstOrDefault();
            }

            else if (masterTable.ToLower() == "sakhteman")
            {
                var ListSakhteman = _context.Sakhteman.Where(p => p.shop == shop);

                if (shod > 0)
                    radif = ListSakhteman.Where(p => p.sh_Darkhast == shod)
                        .Select(s => s.radif).FirstOrDefault();
                else
                    radif = ListSakhteman.Where(p => p.Active == true)
                        .Select(s => s.radif).FirstOrDefault();
            }

            else if (masterTable.ToLower() == "aparteman")
            {
                var ListApartemant = _context.Apartman.Where(p => p.shop == shop);

                if (shod > 0)
                    radif = ListApartemant.Where(p => p.sh_Darkhast == shod)
                        .Select(s => s.radif)
                        .FirstOrDefault();
                else
                    radif = ListApartemant.Where(p => p.Active == true)
                        .Select(s => s.radif).FirstOrDefault();
            }


            return radif;
        }
        public decimal MaxRadif(int decShop)
        {
            var radifTemp = new[]
            {
                _context.Melk.Where(m => m.shop == decShop).Max(m => (decimal?)m.radif) ?? 0,
                _context.Sakhteman.Where(m => m.shop == decShop).Max(m => (decimal?)m.radif) ?? 0,
                _context.Apartman.Where(m => m.shop == decShop).Max(m => (decimal?)m.radif) ?? 0,
                _context.Dv_karbari.Where(m => m.shop == decShop).Max(m => (decimal?)m.d_radif) ?? 0
            };

            return radifTemp.Max();
        }

        public int GetNoeParvandeh(long shop)
        {
            if (shop <= 0)
                return 0;

            var data = _context.Parvandeh.Where(p => p.shop == shop).Select(s => s.idparent).FirstOrDefault();
            if (data == null || data < 0 || data > 3)
                return 0;

            return (int)data + 1;
        }

        public double GetShoPMelk(int shop)
        {
            if (shop == 0)
                return 0;

            var data = _context.Parvandeh.AsNoTracking().FirstOrDefault(p => p.shop == shop);
            if (data == null)
                return 0;

            var data2 = _context.Parvandeh.AsNoTracking().Where(p => p.mantaghe == data.mantaghe && p.hoze == data.hoze && p.blok == data.blok &&
                                                                     p.shomelk == data.shomelk && ((p.sakhteman == null) || (p.sakhteman == 0)))
                .Select(s => s.shop).FirstOrDefault();

            return data2;
        }

        public decimal GetShoPMelk(double shop)
        {
            if (shop == 0)
                return 0;

            var data = _context.Parvandeh.AsNoTracking().FirstOrDefault(p => p.shop == shop);
            if (data == null)
                return 0;

            var data2 = _context.Parvandeh.AsNoTracking().Where(p => p.mantaghe == data.mantaghe && p.hoze == data.hoze && p.blok == data.blok &&
                                                                     p.shomelk == data.shomelk && ((p.sakhteman == null) || (p.sakhteman == 0)))
                .Select(s => s.shop).FirstOrDefault();

            return Convert.ToDecimal(data2);
        }

        public decimal GetShoPSakhteman(double shop)
        {
            if (shop == 0)
                return 0;
            var data = _context.Parvandeh.FirstOrDefault(p => p.shop == shop);
            if (data == null)
                return 0;
            int noeParvandeh = GetNoeParvandeh((long)shop);
            if (noeParvandeh == 1)
                return 0;
            if (noeParvandeh == 2)
                return Convert.ToDecimal(shop);

            var data2 = _context.Parvandeh.Where(p => p.mantaghe == data.mantaghe && p.hoze == data.hoze && p.blok == data.blok && p.shomelk == data.shomelk &&
                                                      p.sakhteman == data.sakhteman && (p.apar == 0 || p.apar == null) && (p.senfi == null || p.senfi == 0))
                .Select(s => s.shop).FirstOrDefault();
            return Convert.ToDecimal(data2);
        }

        public void Copy_DvTable(string masterTable, string dvTable, int dvShop, decimal RadifJ, decimal RadifG)
        {
            switch (dvTable)
            {
                /*------------------------------------------------
                 * 
                 * جداول جزییات ملک
                 * 
                 * -----------------------------------------------*/
                case "Dv_malekin":
                    {
                        var listCurrentMalekin = _context.Dv_malekin.Where(w => w.shop == dvShop && w.mtable_name.ToLower().Equals(masterTable.ToLower()));
                        if (RadifG > 0)
                            listCurrentMalekin = listCurrentMalekin.Where(w => w.d_radif == RadifG).AsNoTracking();

                        foreach (var item in listCurrentMalekin)
                        {
                            Dv_malekin dm = item;
                            dm.Identity = 0;
                            dm.d_radif = (int)RadifJ;
                            _context.Dv_malekin.Add(dm);
                        }
                    }
                    break;
                /*------------------------------------------------
                 * 
                 * جداول جزییات ساختمان
                 * 
                 * -----------------------------------------------*/
                case "Dv_karbari":
                    {
                        var listCurrentKarbari = _context.Dv_karbari.Where(w => w.shop == dvShop && w.mtable_name.ToLower().Equals(masterTable.ToLower()));
                        if (RadifG > 0)
                            listCurrentKarbari = listCurrentKarbari.Where(w => w.d_radif == RadifG).AsNoTracking();

                        foreach (var item in listCurrentKarbari)
                        {
                            Dv_karbari dk = item;
                            dk.Identity = 0;
                            dk.d_radif = Convert.ToInt32(RadifJ);
                            _context.Dv_karbari.Add(dk);
                        }
                    }
                    break;
                case "Dv_savabegh":
                    {
                        var listCurrentSavabegh = _context.Dv_savabegh.Where(w => w.shop == dvShop && w.mtable_name.ToLower().Equals(masterTable.ToLower()));
                        if (RadifG > 0)
                            listCurrentSavabegh = listCurrentSavabegh.Where(w => w.d_radif == RadifG).AsNoTracking();
                        int id = 1;
                        foreach (var item in listCurrentSavabegh)
                        {
                            Dv_savabegh ds = item;
                            ds.Identity = 0;
                            ds.d_radif = Convert.ToInt32(RadifJ);
                            _context.Dv_savabegh.Add(ds);
                            id++;
                        }
                    }
                    break;
            }
        }

        public void Update_DvTable(string masterTable, string dvTable, decimal dvShop, decimal RadifJ, decimal RadifG)
        {
            decimal shop = dvShop;
            decimal radifJadid = RadifJ;

            switch (dvTable)
            {
                case "Dv_malekin"://-------------------ردبف قدیم نداره لیست اول
                    {
                        var listCurrentMalekin = _context.Dv_malekin.Where(w => w.shop == shop && w.mtable_name.ToLower().Equals(masterTable.ToLower()));

                        foreach (var item in listCurrentMalekin)
                        {
                            var ListNewMalekin = _context.Dv_malekin.Where(w => w.shop == shop && w.d_radif == radifJadid &&
                                w.mtable_name.ToLower().Equals(masterTable.ToLower()) && w.id == item.id).ToList();

                            foreach (var j in ListNewMalekin)
                            {
                                // Copy properties from item to j
                                _context.Entry(j).CurrentValues.SetValues(item);
                                // Update specific property
                                j.d_radif = (int)radifJadid;
                                // Mark as modified for database update
                                _context.Entry(j).State = EntityState.Modified;
                            }

                        }
                    }
                    break;
                case "Dv_karbari"://-------------------ردبف قدیم نداره لیست اول
                    {
                        var listCurrentKarbari = _context.Dv_karbari.Where(w => w.shop == shop && w.mtable_name.ToLower().Equals(masterTable.ToLower()));

                        foreach (var item in listCurrentKarbari)
                        {
                            var lisNewKarbari = _context.Dv_karbari.Where(w => w.shop == shop && w.d_radif == radifJadid &&
                                                                    w.mtable_name.ToLower().Equals(masterTable.ToLower()) && w.id == item.id).ToList();

                            foreach (var j in lisNewKarbari)
                            {
                                // Copy properties from item to j
                                _context.Entry(j).CurrentValues.SetValues(item);
                                // Update specific property
                                j.d_radif = (int)radifJadid;
                                // Mark as modified for database update
                                _context.Entry(j).State = EntityState.Modified;
                            }
                        }
                    }
                    break;
            }
        }


        //public static decimal GET_Radif(string mtableName, decimal decShop)
        //{
        //    if (decShop == 0)
        //        return 0;
        //    using var db = new FromUrbanDbContext(CustomDbContext.CreateDbContext());
        //    if (mtableName.ToLower() == "melk")
        //    {
        //        var radifMelk = db.Melk.Where(p => p.shop == decShop && p.Active == true).Select(s => s.radif)
        //            .FirstOrDefault();
        //        return Convert.ToDecimal(radifMelk);
        //    }
        //    else if (mtableName.ToLower() == "sakhteman")
        //    {
        //        //var dataSakh = db.sakhteman.Where(p => p.shop == shop && p.sh_Darkhast == shod).Select(s => s.radif).FirstOrDefault();
        //        var radifSakh = db.Sakhteman.Where(p => p.shop == decShop && p.Active == true).Select(s => s.radif)
        //            .FirstOrDefault();
        //        return Convert.ToDecimal(radifSakh);
        //    }
        //    else if (mtableName.ToLower() == "aparteman")
        //    {
        //        //var dataApar = db.apartman.Where(p => p.shop == shop && p.sh_Darkhast == shod).Select(s => s.radif).FirstOrDefault();
        //        var radifApar = db.Apartman.Where(p => p.shop == decShop && p.Active == true).Select(s => s.radif)
        //            .FirstOrDefault();
        //        return Convert.ToDecimal(radifApar);
        //    }
        //    return 0;
        //}
        //public static decimal GetShoPSakhteman(double shop)
        //{
        //    if (shop == 0)
        //        return 0;
        //    using var db = new FromUrbanDbContext(CustomDbContext.CreateDbContext());
        //    var data = db.Parvandeh.Where(p => p.shop == shop).FirstOrDefault();
        //    if (data == null)
        //        return 0;
        //    int noeParvandeh = GetNoeParvandeh((long)shop);
        //    if (noeParvandeh == 1)
        //        return 0;
        //    if (noeParvandeh == 2)
        //        return Convert.ToDecimal(shop);

        //    //    var data2 = db.Parvandeh.Where(p => p.mantaghe == data.mantaghe && p.hoze == data.hoze &&
        //    //                                        p.blok == data.blok && p.shomelk == data.shomelk &&
        //    //                                        p.sakhteman == data.sakhteman && (p.apar == 0 || p.apar == null) &&
        //    //                                        (p.senfi == null || p.senfi == 0))
        //    //        .Select(s => s.shop).FirstOrDefault();
        //    //    return Convert.ToDecimal(data2);
        //    //}
        //    //public static int GetNoeSenfi(double shop)
        //    //{
        //    //    if (shop <= 0)
        //    //        return 0;

        //    //    int noeParvandeh = GetNoeParvandeh((long)shop);

        //    //    if (noeParvandeh != 4)
        //    //        return 0;

        //    //    using var db = new FromUrbanDbContext(CustomDbContext.CreateDbContext());
        //    //    var lstParvandeh = db.Parvandeh.Where(p => p.shop == shop)
        //    //        .Select(s => new { s.shop, s.apar, s.sakhteman, s.senfi }).ToList();

        //    //    if (lstParvandeh.Count() == 0)
        //    //        return 0;

        //    //    if ((lstParvandeh.First().sakhteman != 0) && (lstParvandeh.First().apar == 0))
        //    //        return 1;

        //    //    if (lstParvandeh.First().apar != 0)
        //    //        return 2;

        //    //    return 0;
        //    //}
        //    //public static decimal GetShoPApartman(double shop)
        //    //{
        //    //    if (shop == 0)
        //    //        return 0;
        //    //    using var db = new FromUrbanDbContext(CustomDbContext.CreateDbContext());
        //    //    var data = db.Parvandeh.Where(p => p.shop == shop).FirstOrDefault();
        //    //    if (data == null)
        //    //        return 0;
        //    //    int noeParvandeh = GetNoeParvandeh((long)shop);
        //    //    if (noeParvandeh == 1)
        //    //        return 0;
        //    //    if (noeParvandeh == 2)
        //    //        return 0;
        //    //    if (noeParvandeh == 3)
        //    //        return Convert.ToDecimal(shop);

        //    //    var data2 = db.Parvandeh.Where(p => p.mantaghe == data.mantaghe && p.hoze == data.hoze &&
        //    //                                        p.blok == data.blok && p.shomelk == data.shomelk &&
        //    //                                        p.sakhteman == data.sakhteman && p.apar == data.apar &&
        //    //                                        ((p.senfi == null) || (p.senfi == 0))).Select(s => s.shop).FirstOrDefault();
        //    //    return Convert.ToDecimal(data2);
        //    //}

        public string GetTime() => DateTime.UtcNow.AddHours(3.5).ToString("HH:mm");

        public int Get_Farsi_Date()
        {
            PersianCalendar pc = new PersianCalendar();
            string y = pc.GetYear(DateTime.UtcNow.AddHours(3.5)).ToString();
            string m = pc.GetMonth(DateTime.UtcNow.AddHours(3.5)).ToString();
            string d = pc.GetDayOfMonth(DateTime.UtcNow.AddHours(3.5)).ToString();
            if (m.Length < 2)
                m = "0" + m;

            if (d.Length < 2)
                d = "0" + d;

            string date = y + m + d;
            return Convert.ToInt32(date);
        }

        public string FormatDate(string inputDate)
        {
            if (inputDate.Length == 8 && long.TryParse(inputDate, out _))
                return $"{inputDate.Substring(0, 4)}/{inputDate.Substring(4, 2)}/{inputDate.Substring(6, 2)}";

            return "";
        }

        public DateTime ConvertPersianToGregorian(string persianDateTime)
        {
            var persianCalendar = new PersianCalendar();

            // جدا کردن تاریخ و زمان از ورودی
            var parts = persianDateTime.Split(' ');
            var dateParts = parts[0].Split('/');
            var timeParts = parts[1].Split(':');

            int year = int.Parse(dateParts[2]);
            int month = int.Parse(dateParts[0]);
            int day = int.Parse(dateParts[1]);

            int hour = int.Parse(timeParts[0]);
            int minute = int.Parse(timeParts[1]);
            int second = int.Parse(timeParts[2]);

            // تبدیل تاریخ شمسی به میلادی
            DateTime gregorianDate = persianCalendar.ToDateTime(year, month, day, hour, minute, second, 0);

            return gregorianDate;
        }

        public DateTime ConvertPersianToGregorian2(string persianDateTime)
        {
            var persianCalendar = new PersianCalendar();

            // جدا کردن تاریخ و زمان از ورودی
            var parts = persianDateTime.Split(' ');
            var dateParts = parts[0].Split('/');

            int year = int.Parse(ConvertPersianNumbersToEnglish(dateParts[0]));
            int month = int.Parse(ConvertPersianNumbersToEnglish(dateParts[1]));
            int day = int.Parse(ConvertPersianNumbersToEnglish(dateParts[2]));

            int hour = 0;
            int minute = 0;
            int second = 0;

            if (parts.Length > 1)
            {
                var timeParts = parts[1].Split(':');
                hour = int.Parse(timeParts[0]);
                minute = int.Parse(timeParts[1]);
                second = int.Parse(timeParts[2]);
            }

            // تبدیل تاریخ شمسی به میلادی
            DateTime gregorianDate = persianCalendar.ToDateTime(year, month, day, hour, minute, second, 0);

            return gregorianDate;
        }

        //public DateTime ConvertGregorianToPersian(string persianDateTime)
        //{
        //    var persianCalendar = new PersianCalendar();

        //    // جدا کردن تاریخ و زمان از ورودی
        //    var parts = persianDateTime.Split(' ');
        //    var dateParts = parts[0].Split('/');
        //    var timeParts = parts[1].Split(':');

        //    int year = int.Parse(dateParts[2]);
        //    int month = int.Parse(dateParts[1]);
        //    int day = int.Parse(dateParts[0]);

        //    int hour = int.Parse(timeParts[0]);
        //    int minute = int.Parse(timeParts[1]);
        //    int second = int.Parse(timeParts[2]);

        //    // تبدیل تاریخ شمسی به میلادی
        //    DateTime gregorianDate = persianCalendar.ToDateTime(year, month, day, hour, minute, second, 0);

        //    return gregorianDate;
        //}

        public DateTime ConvertGregorianToPersian(string persianDateTime)
        {
            var persianCalendar = new PersianCalendar();

            var parts = persianDateTime.Split(' ');
            var dateParts = parts[0].Split('/');
            var timeParts = parts[1].Split(':');

            int year = int.Parse(dateParts[2]);
            int month = int.Parse(dateParts[1]);
            int day = int.Parse(dateParts[0]);

            int hour = int.Parse(timeParts[0]);
            int minute = int.Parse(timeParts[1]);
            int second = int.Parse(timeParts[2]);

            DateTime gregorianDate = persianCalendar.ToDateTime(year, month, day, hour, minute, second, 0);

            return gregorianDate;
        }

        public string ConvertPersianNumbersToEnglish(string input)
        {
            var persianDigits = new[] { '۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹' };
            for (int i = 0; i < persianDigits.Length; i++)
            {
                input = input.Replace(persianDigits[i], (char)('0' + i));
            }

            var arabicDigits = new[] { '٠', '١', '٢', '٣', '٤', '٥', '٦', '٧', '٨', '٩' };
            for (int i = 0; i < arabicDigits.Length; i++)
            {
                input = input.Replace(arabicDigits[i], (char)('0' + i));
            }

            return input;
        }

        /// <summary>
        /// فراخوانی یک URL بدون باز شدن TAB  
        /// </summary>
        /// <param name="url"></param>
        /// <returns>پاسخ برگردانده شده از url</returns>
        //public static string CallUrl(string url)
        //{
        //    try
        //    {
        //        if (WebRequest.Create(url) is not HttpWebRequest request) return string.Empty;

        //        ServicePointManager.ServerCertificateValidationCallback += ValidationCallback;
        //        var response = request.GetResponse();
        //        var reader = new StreamReader(response.GetResponseStream());
        //        return reader.ReadToEnd(); // it takes the response from your url. now you can use as your need  

        //    }
        //    catch (Exception e)
        //    {
        //        return string.Empty;
        //    }
        //}

        //private static bool ValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        //{
        //    // Since you want to be more strict than the default, reject it if anything went wrong.
        //    if (sslPolicyErrors != SslPolicyErrors.None)
        //        return false;

        //    if (chain.ChainStatus.Any(status => status.Status != X509ChainStatusFlags.NoError))
        //        return false;

        //    // If the chain didn't suppress any type of error, and revocation
        //    // was checked, then it's okay.
        //    if (chain.ChainPolicy.VerificationFlags == X509VerificationFlags.AllFlags && chain.ChainPolicy.RevocationMode == X509RevocationMode.Online)
        //        return true;

        //    var newChain = new X509Chain();
        //    // change any other ChainPolicy options you want.
        //    var chainElements = chain.ChainElements;

        //    // Skip the leaf cert and stop short of the root cert.
        //    for (var i = 1; i < chainElements.Count - 1; i++)
        //        newChain.ChainPolicy.ExtraStore.Add(chainElements[i].Certificate);

        //    // Use chainElements[0].Certificate since it's the right cert already
        //    // in X509Certificate2 form, preventing a cast or the sometimes-dangerous
        //    // X509Certificate2(X509Certificate) constructor.
        //    // If the chain build successfully it matches all our policy requests,
        //    // if it fails, it either failed to build (which is unlikely, since we already had one)
        //    // or it failed policy (like it's revoked).
        //    return newChain.Build(chainElements[0].Certificate);
        //}
    }
}
