using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormerUrban_Afta.DataAccess.Infrastructure;
public class CustomPersonalDataProtector : IPersonalDataProtector
{
    private readonly IDataProtector _protector;

    public CustomPersonalDataProtector(IDataProtectionProvider provider)
    {
        _protector = provider.CreateProtector("PersonalDataProtector");
    }

    public string Protect(string data)
    {
        return _protector.Protect(data);
    }

    public string Unprotect(string data)
    {
        return _protector.Unprotect(data);
    }
}

