namespace Dynamics365BC.Models;


/// <summary>
/// <see href="https://learn.microsoft.com/en-us/dynamics365/business-central/dev-itpro/api-reference/v2.0/resources/dynamics_account"/>
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class DynAccount
{
    /// <summary>
    ///	The unique ID of the account. Non-editable.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Specifies the number of the account.
    /// </summary>
    public string? Number { get; set; }

    /// <summary>
    /// Specifies the account's name. This name will appear on all sales documents for the account.
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// Specifies the category of the account.
    /// It can be " ", "Assets", "Liabilities", "Equity", "Income", "Cost of Goods Sold" or "Expense".
    /// <see cref="GLAccountCategory"/>
    /// </summary>
    public string? Category { get; set; }

    /// <summary>
    /// Specifies the subcategory of the account category of the G/L account
    /// </summary>
    public string? SubCategory { get; set; }

    /// <summary>
    /// Specifies that entries cannot be posted to the account. True indicates account is blocked and posting is not allowed.
    /// </summary>
    public bool Blocked { get; set; }

    /// <summary>
    /// The type of the account that the account is related to.
    /// It can be "Posting", "Heading", "Total", "Begin Total" or "End Total".
    /// <see cref="GlAccountType"/>
    /// </summary>
    public string? AccountType { get; set; }

    /// <summary>
    /// Specifies whether direct posting is enabled.
    /// </summary>
    public bool DirectPosting { get; set; }

    /// <summary>
    /// Specifies the net change in the account balance during the time period in the Date Filter field.
    /// </summary>
    public decimal NetChange { get; set; }

    /// <summary>
    /// The last datetime the account was modified. Read-Only.
    /// </summary>
    public DateTime LastModifiedDateTime { get; set; }
}

public static class GlAccountType
{
    public const string Posting = "Posting";
    public const string Heading = "Heading";
    public const string Total = "Total";
    public const string BeginTotal = "Begin Total";
    public const string EndTotal = "End Total";
}

public static class GLAccountCategory
{
    public const string Empty = " ";
    public const string Assets = "Assets";
    public const string Liabilities = "Liabilities";
    public const string Equity = "Equity";
    public const string Income = "Income";
    public const string CostOfGoodsSold = "Cost of Goods Sold";
    public const string Expense = "Expense";
}
