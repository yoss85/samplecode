namespace Dynamics365BC.Models;

/// <summary>
/// <see href="https://learn.microsoft.com/en-us/dynamics365/business-central/dev-itpro/api-reference/v2.0/resources/dynamics_dimension"/>
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class DynDimension
{
    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string? DisplayName { get; set; }
    public DateTime LastModifiedDateTime { get; set; }
    public IEnumerable<DynDimensionValue>? DimensionValues { get; set; }
}
