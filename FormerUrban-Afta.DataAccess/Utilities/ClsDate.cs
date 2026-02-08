using System.Globalization;

namespace FormerUrban_Afta.DataAccess.Utilities
{
    public static class ClsDate
    {
        public static string MiladiToShamsi(DateTime date)
        {
            StringBuilder sb = new();
            PersianCalendar pc = new();
            sb.Append(pc.GetYear(date).ToString("0000"));
            sb.Append("/");
            sb.Append(pc.GetMonth(date).ToString("00"));
            sb.Append("/");
            sb.Append(pc.GetDayOfMonth(date).ToString("00"));
            return sb.ToString();
        }

        public static int MiladiToShamsiInt(DateTime date)
        {
            StringBuilder sb = new();
            PersianCalendar pc = new();
            sb.Append(pc.GetYear(date).ToString("0000"));
            sb.Append(pc.GetMonth(date).ToString("00"));
            sb.Append(pc.GetDayOfMonth(date).ToString("00"));
            return Convert.ToInt32(sb.ToString());
        }

        public static int MiladiToShamsiInt(DateTime? date)
        {
            if (date == null) return 0;
            var pc = new PersianCalendar();
            var sb = new StringBuilder();
            sb.Append(pc.GetYear((DateTime)date).ToString("0000"));
            sb.Append(pc.GetMonth((DateTime)date).ToString("00"));
            sb.Append(pc.GetDayOfMonth((DateTime)date).ToString("00"));
            return Convert.ToInt32(sb.ToString());
        }

        public static DateTime ShamsiIntToMiladi(int shamsi)
        {
            try
            {
                var st = shamsi.ToString();
                if (st.Length == 8)
                {
                    string dat, sal, mah, roz, ret;
                    dat = st;
                    sal = dat.Substring(0, 4);
                    mah = dat.Substring(4, 2);
                    roz = dat.Substring(6, 2);
                    PersianCalendar pc = new();
                    ret = pc.ToDateTime(Convert.ToInt32(sal), Convert.ToInt32(mah), Convert.ToInt32(roz), 0, 0, 0, 0).ToString();
                    return Convert.ToDateTime(ret);
                }
                else
                    return DateTime.UtcNow.AddHours(3.5);
            }

            catch
            {
                return DateTime.UtcNow.AddHours(3.5);
            }
        }

        public static int GetYearFromShamsiInt(this int? shamsi)
        {
            try
            {
                var st = shamsi.ToString();
                if (st.Length == 8)
                {
                    string dat, sal;
                    dat = st;
                    sal = dat.Substring(0, 4);
                    return Convert.ToInt32(sal);
                }
                else
                    return 0;
            }
            catch
            {
                return 0;
            }
        }

        public static DateTime ShamsiIntToMiladi(int shamsi, int time)
        {
            try
            {
                var st = shamsi.ToString();
                if (st.Length == 8)
                {
                    string dat, sal, mah, roz, ret;
                    dat = st;
                    sal = dat.Substring(0, 4);
                    mah = dat.Substring(5, 2);
                    roz = dat.Substring(8, 2);
                    //****************************************************** Time
                    var strTime = time.ToString();
                    int h = 0, m = 0;
                    if (strTime.Length <= 2)
                        h = int.Parse(strTime);
                    else if (strTime.Length == 3)
                    {
                        h = int.Parse(strTime.Substring(0, 1));
                        m = int.Parse(strTime.Substring(2, 2));
                    }
                    else if (strTime.Length == 4)
                    {
                        h = int.Parse(strTime.Substring(0, 2));
                        m = int.Parse(strTime.Substring(3, 2));
                    }
                    //**********************************************************************
                    PersianCalendar pc = new();
                    ret = pc.ToDateTime(Convert.ToInt32(sal), Convert.ToInt32(mah), Convert.ToInt32(roz), h, m, 0, 0).ToString();
                    return Convert.ToDateTime(ret);
                }
                else
                    return DateTime.UtcNow.AddHours(3.5);
            }
            catch
            {
                return DateTime.UtcNow.AddHours(3.5);
            }
        }

        public static DateTime ShamsiToMiladi(string date)
        {
            string dat, sal, mah, roz, ret;
            dat = date;
            sal = dat.Substring(0, 4);
            mah = dat.Substring(5, 2);
            roz = dat.Substring(8, 2);
            PersianCalendar pc = new();
            ret = pc.ToDateTime(Convert.ToInt32(sal), Convert.ToInt32(mah), Convert.ToInt32(roz), 0, 0, 0, 0).ToString();
            return Convert.ToDateTime(ret);
        }


    }
}
