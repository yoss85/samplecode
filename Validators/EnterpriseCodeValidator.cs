using AvidConnect.DataModel;
using FluentValidation;

namespace Dynamics365BC.Validators;

[ExcludeFromCodeCoverage]
public sealed class EnterpriseCodeValidator : AbstractValidator<EnterpriseCode>
{
    public EnterpriseCodeValidator()
    {
        RuleFor(enterpriseCode => enterpriseCode.Description).MaximumLength(50);
    }
}
