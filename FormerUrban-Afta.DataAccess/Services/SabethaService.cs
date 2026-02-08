using FormerUrban_Afta.DataAccess.DTOs.Sabetha;
using System.Reflection;

namespace FormerUrban_Afta.DataAccess.Services;
public class SabethaService : ISabethaService
{

    private Type FindEnumType(string enumName)
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .FirstOrDefault(t => t.IsEnum && t.Name == enumName);
    }

    public List<SabethaDto> GetRows(string enumName)
    {
        var enumType = FindEnumType(enumName);
        if (enumType == null)
            throw new ArgumentException($"Enum type '{enumName}' was not found in loaded assemblies.");

        var dataList = Enum.GetValues(enumType)
            .Cast<object>()
            .Select(e => new SabethaDto
            {
                Name = e.ToString(),
                Index = Convert.ToInt32(e),
                DisplayName = enumType
                    .GetMember(e.ToString())[0]
                    .GetCustomAttribute<DisplayAttribute>()?.Name ?? e.ToString()
            })
            .ToList();

        return dataList;
    }


}

