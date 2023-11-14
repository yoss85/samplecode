using Dynamics365BC.Models;
using Dynamics365BC.Services;

namespace Dynamics365BC.Extensions;

[ExcludeFromCodeCoverage]
public static class Dynamics365Extensions
{
    public static async Task<Result<DynCompany?>> GetCompanyIdByNameAsync(this IDynamics365ApiClient apiClient, string companyName)
        => await apiClient.GetCompaniesAsync(true, companyName)
            .Bind(c => Result.Success(c?.FirstOrDefault()));
}
