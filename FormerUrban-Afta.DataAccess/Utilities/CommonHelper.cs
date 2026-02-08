using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace FormerUrban_Afta.DataAccess.Utilities
{
    public static class CommonHelper
    {
        private static bool? _isDevEnvironment;
        public const int MaximumSessions = 20;
        public const int MaximumKhatemeSessionAfterMinute = 600;

        /// <summary>
        /// Normalizes a list of strings by replacing arrow patterns with the Arabic comma.
        /// </summary>
        public static List<string> Normalize(this List<string> list) =>
            list.Select(item => item.Replace(" -> ", "، ").Replace("-> ", "، ")).ToList();

        //public static bool IsDevEnvironment
        //{
        //    get
        //    {
        //        if (!_isDevEnvironment.HasValue)
        //        {
        //            _isDevEnvironment = IsDevEnvironmentInternal();
        //        }

        //        return _isDevEnvironment.Value;
        //    }
        //}

        //private static bool IsDevEnvironmentInternal()
        //{
        //    if (!HostingEnvironment.IsHosted)
        //        return true;

        //    if (HostingEnvironment.IsDevelopmentEnvironment)
        //        return true;

        //    if (System.Diagnostics.Debugger.IsAttached)
        //        return true;

        //    if (Environment.UserInteractive)
        //        return true;

        //    return false;
        //}

        /// <summary>
        /// Retrieves the last 8 characters of the BIOS serial number on Windows systems.
        /// </summary>
        public static string GetBaseBoardSerialNumber()
        {
#if WINDOWS
            var serialData = new StringBuilder();
            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_BIOS");
                foreach (var bios in searcher.Get())
                {
                    if (bios["SerialNumber"] != null)
                    {
                        serialData.Append(bios["SerialNumber"].ToString());
                    }
                }
            }
            catch
            {
                // Optionally handle exceptions or log as needed.
            }
            
            var result = serialData.ToString().Trim().ToUpper();
            return result.Length >= 8 ? result[^8..] : result;
#else
            return "UNSUPPORTED_OS";
#endif
        }

        /// <summary>
        /// Generates a random digit code of a specified length.
        /// </summary>
        public static string GenerateRandomDigitCode(int length)
        {
            var sb = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                // Generates a random digit between 0 and 9.
                int digit = RandomNumberGenerator.GetInt32(0, 10);
                sb.Append(digit);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Generates a unique long identifier from a string using SHA1 hash.
        /// </summary>
        public static long GetUniqueIdFromString(this string input, bool caseSensitive = false)
        {
            if (!caseSensitive)
            {
                input = input.ToLowerInvariant();
            }

            using SHA1 sha1 = SHA1.Create();
            byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Converts the first 7 bytes of the hash to a long value.
            return Convert.ToInt64(BitConverter.ToString(hash, 0, 7).Replace("-", ""), 16);
        }

        /// <summary>
        /// Generates a random integer between the specified min and max values.
        /// </summary>
        public static int GenerateRandomInteger(int min = 0, int max = int.MaxValue) =>
            RandomNumberGenerator.GetInt32(min, max);

        /// <summary>
        /// Checks if a connection string exists in the provided configuration.
        /// </summary>
        public static bool HasConnectionString(string connectionStringName, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString(connectionStringName);
            return !string.IsNullOrEmpty(connectionString);
        }
    }
}
