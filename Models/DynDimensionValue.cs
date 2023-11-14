namespace Dynamics365BC.Models;

/// <summary>
/// <see href="https://learn.microsoft.com/en-us/dynamics365/business-central/dev-itpro/api-reference/v2.0/resources/dynamics_dimensionvalue"/>
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class DynDimensionValue
{
    /// <summary>
    /// The unique ID of the dimension value. Non-editable.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The code of the dimension value.
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// The unique ID of dimension.
    /// </summary>
    public Guid DimensionId { get; set; }

    /// <summary>
    /// Specifies the dimension value's name. This name will appear on all sales documents for the dimension value.
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// The last datetime the dimension value was modified. Read-Only.
    /// </summary>
    public DateTime LastModifiedDateTime { get; set; }
}