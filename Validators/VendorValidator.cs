using AvidConnect.DataModel;
using FluentValidation;

namespace Dynamics365BC.Validators;

[ExcludeFromCodeCoverage]
public sealed class VendorValidator : AbstractValidator<Vendor>
{
    public VendorValidator()
    {
        RuleFor(vendor => vendor.ExternalVendorCode).MaximumLength(50);
        RuleFor(vendor => vendor.Address.Line1).MaximumLength(255);
        RuleFor(vendor => vendor.Address.Line2).MaximumLength(255);
        RuleFor(vendor => vendor.Address.City).MaximumLength(50);
        RuleFor(vendor => vendor.Address.State).MaximumLength(50);
        RuleFor(vendor => vendor.Address.PostalCode).MaximumLength(20);
        RuleFor(vendor => vendor.Telephone1).MaximumLength(20);
    }
}
