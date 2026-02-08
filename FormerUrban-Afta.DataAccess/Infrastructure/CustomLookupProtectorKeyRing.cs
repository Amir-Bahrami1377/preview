using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormerUrban_Afta.DataAccess.Infrastructure;
public class CustomLookupProtectorKeyRing : ILookupProtectorKeyRing
{
    public string this[string keyId]
    {
        get
        {
            return GetAllKeyIds().Where(x => x == keyId).FirstOrDefault();
        }
    }

    public string CurrentKeyId
    {
        get
        {
            byte[] key = { 200, 15, 147, 5, 155, 78, 118, 57, 180, 179, 60, 150, 188, 18, 165, 134 };
            var currentKey = Convert.ToBase64String(key);
            return currentKey;
        }
    }

    public IEnumerable<string> GetAllKeyIds()
    {
        var list = new List<string>();
        // This is 24 bytes length
        byte[] key = { 200, 15, 147, 5, 155, 78, 118, 57, 180, 179, 60, 150, 188, 18, 165, 134 };
        byte[] key2 = { 242, 207, 146, 81, 121, 231, 168, 93, 89, 130, 4, 68, 18, 185, 98, 154 };
        byte[] key3 = { 101, 104, 174, 233, 88, 29, 20, 16, 21, 216, 249, 45, 148, 18, 102, 150 };

        list.Add(Convert.ToBase64String(key));
        list.Add(Convert.ToBase64String(key2));
        list.Add(Convert.ToBase64String(key3));


        return list;
    }
}
