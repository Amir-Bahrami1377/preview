using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FormerUrban_Afta.DataAccess.Services
{
    public static class CipherService
    {
        private const string Pepper = "w!z%C*F-JaNdRgUkXp2s5v8y/A?D(G+K";

        public static string Hash(string input, HashAlgorithmType algorithmType = HashAlgorithmType.SHA256)
        {
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var pepperBytes = Encoding.UTF8.GetBytes(Pepper);
            var saltLength = GetHashSizeInBytes(algorithmType);
            var saltBytes = RandomNumberGenerator.GetBytes(saltLength);

            using var hmac = CreateHmac(algorithmType, pepperBytes.Concat(saltBytes).ToArray());
            var hashBytes = hmac.ComputeHash(inputBytes);
            var mixed = MixChunks(hashBytes, saltBytes);
            CryptographicOperations.ZeroMemory(saltBytes);
            CryptographicOperations.ZeroMemory(pepperBytes);
            return Convert.ToHexString(mixed);
        }

        public static bool IsEqual(string input, string mixedHex,
            HashAlgorithmType algorithmType = HashAlgorithmType.SHA256)
        {
            if (string.IsNullOrWhiteSpace(input) || string.IsNullOrWhiteSpace(mixedHex))
                return false;

            byte[] mixedBytes;
            try
            {
                mixedBytes = Convert.FromHexString(mixedHex);
            }
            catch
            {
                return false;
            }

            var (saltBytes, originalHashBytes) = UnmixChunks(mixedBytes, algorithmType);

            var inputBytes = Encoding.UTF8.GetBytes(input);
            var pepperBytes = Encoding.UTF8.GetBytes(Pepper);

            using var hmac = CreateHmac(algorithmType, pepperBytes.Concat(saltBytes).ToArray());
            var recomputedHash = hmac.ComputeHash(inputBytes);

            CryptographicOperations.ZeroMemory(saltBytes);
            CryptographicOperations.ZeroMemory(pepperBytes);
            CryptographicOperations.ZeroMemory(inputBytes);

            return CryptographicOperations.FixedTimeEquals(recomputedHash, originalHashBytes);
        }

        private static int GetHashSizeInBytes(HashAlgorithmType algorithmType) => algorithmType switch
        {
            HashAlgorithmType.SHA256 => 32,
            HashAlgorithmType.SHA384 => 48,
            HashAlgorithmType.SHA512 => 64,
            _ => throw new ArgumentOutOfRangeException(nameof(algorithmType))
        };

        private static HMAC CreateHmac(HashAlgorithmType type, byte[] key) => type switch
        {
            HashAlgorithmType.SHA256 => new HMACSHA256(key),
            HashAlgorithmType.SHA384 => new HMACSHA384(key),
            HashAlgorithmType.SHA512 => new HMACSHA512(key),
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };

        private static byte[] MixChunks(byte[] hash, byte[] salt)
        {
            const int chunkSize = 8;
            var saltChunks = salt.Chunk(chunkSize).ToArray();
            var hashChunks = hash.Chunk(chunkSize).ToArray();

            var mixed = new List<byte>();
            mixed.AddRange(saltChunks[3]);
            mixed.AddRange(hashChunks[0]);
            mixed.AddRange(saltChunks[1]);
            mixed.AddRange(hashChunks[3]);
            mixed.AddRange(saltChunks[0]);
            mixed.AddRange(saltChunks[2]);
            mixed.AddRange(hashChunks[1]);
            mixed.AddRange(hashChunks[2]);

            return mixed.ToArray();
        }

        private static (byte[] salt, byte[] hash) UnmixChunks(byte[] mixedBytes, HashAlgorithmType algorithmType = HashAlgorithmType.SHA256)
        {
            const int chunkSize = 8;
            int chunkCount = GetHashSizeInBytes(algorithmType) / chunkSize;

            var S3 = mixedBytes[0..chunkSize];
            var H0 = mixedBytes[chunkSize..(2 * chunkSize)];
            var S1 = mixedBytes[(2 * chunkSize)..(3 * chunkSize)];
            var H3 = mixedBytes[(3 * chunkSize)..(4 * chunkSize)];
            var S0 = mixedBytes[(4 * chunkSize)..(5 * chunkSize)];
            var S2 = mixedBytes[(5 * chunkSize)..(6 * chunkSize)];
            var H1 = mixedBytes[(6 * chunkSize)..(7 * chunkSize)];
            var H2 = mixedBytes[(7 * chunkSize)..(8 * chunkSize)];

            var salt = S0.Concat(S1).Concat(S2).Concat(S3).ToArray();
            var hash = H0.Concat(H1).Concat(H2).Concat(H3).ToArray();

            return (salt, hash);
        }
    }

    public enum HashAlgorithmType
    {
        SHA256 = 0,
        SHA384 = 1,
        SHA512 = 2,
    }
}
