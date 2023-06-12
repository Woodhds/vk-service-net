using Microsoft.EntityFrameworkCore;

namespace VkService.Data.Extensions;

public static class DbFunctionsExtensions
{
    [DbFunction]
    public static bool Match(string search, string column)
        => throw new NotImplementedException();
}
