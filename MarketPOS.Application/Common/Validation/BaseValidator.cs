using FluentValidation;
using System.Linq.Expressions;

public abstract class BaseValidator<TDto> : AbstractValidator<TDto>
{
    protected void RuleForName(Expression<Func<TDto, string>> selector, int maxLength = 100)
    {
        RuleFor(selector)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(maxLength).WithMessage($"Name cannot exceed {maxLength} characters.");
    }
    protected void RuleForNameValid(Expression<Func<TDto, string>> selector, string fieldName)
    {
        RuleFor(selector)
            .NotEmpty().WithMessage("Name is required.").WithMessage($"{fieldName ?? "Name"} is required.");
    }
    protected void RuleForOptionalName(Expression<Func<TDto, string?>> selector, int maxLength = 100)
    {
        RuleFor(selector)
            .MaximumLength(maxLength).WithMessage($"Name cannot exceed {maxLength} characters.");
    }

    protected void RuleForId(Expression<Func<TDto, Guid>> selector, string? fieldName = null)
    {
        RuleFor(selector)
            .NotEmpty().WithMessage($"{fieldName ?? "Id"} is required.");
    }

    protected void RuleForOptionalId(Expression<Func<TDto, Guid?>> selector, string? fieldName = null)
    {
        RuleFor(selector)
            .NotNull().WithMessage($"{fieldName ?? "Id"} is required.")
            .NotEqual(Guid.Empty).WithMessage($"{fieldName ?? "Id"} must not be empty.");
    }

    protected void RuleForPrice(Expression<Func<TDto, decimal>> selector, string? fieldName = null)
    {
        RuleFor(selector)
            .GreaterThanOrEqualTo(0).WithMessage($"{fieldName ?? "Price"} must be >= 0.");
    }

    protected void RuleForNullablePrice(Expression<Func<TDto, decimal?>> selector, string? fieldName = null)
    {
        RuleFor(selector)
            .NotNull().WithMessage($"{fieldName ?? "Price"} is required.")
            .GreaterThanOrEqualTo(0).WithMessage($"{fieldName ?? "Price"} must be >= 0.");
    }

    protected void RuleForQuantity(Expression<Func<TDto, int>> selector, string? fieldName = null)
    {
        RuleFor(selector)
            .GreaterThanOrEqualTo(0).WithMessage($"{fieldName ?? "Quantity"} must be >= 0.");
    }

    protected void RuleForCategoryId(Expression<Func<TDto, Guid>> selector, string? fieldName = null)
    {
        RuleFor(selector)
            .NotEmpty().WithMessage($"{fieldName ?? "CategoryId"} is required.");
    }

    protected void RuleForExpirationDate(Expression<Func<TDto, DateTime?>> selector, string selctor)
    {
        RuleFor(selector)
            .NotNull().WithMessage("Expiration date is required.")
            .Must(date => date.HasValue && date.Value > DateTime.Now)
            .WithMessage($"Expiration date must be in the future. The Name Selector: {selctor}");
    }

    protected void RuleForPercentage(Expression<Func<TDto, decimal>> selector, string? fieldName = null)
    {
        RuleFor(selector)
            .InclusiveBetween(0, 100).WithMessage($"{fieldName ?? "Percentage"} must be between 0 and 100.");
    }

    protected void RuleForNullablePercentage(Expression<Func<TDto, decimal?>> selector, string? fieldName = null)
    {
        RuleFor(selector)
            .NotNull().WithMessage($"{fieldName ?? "Percentage"} is required.")
            .InclusiveBetween(0, 100).WithMessage($"{fieldName ?? "Percentage"} must be between 0 and 100.");
    }

    protected void RuleForUnitConversion(Expression<Func<TDto, int>> selector, string unitName)
    {
        RuleFor(selector)
            .GreaterThan(0).WithMessage($"{unitName} conversion rate must be greater than 0.");
    }

    protected void RuleForLessThan<TProperty>(
        Expression<Func<TDto, TProperty>> smallerSelector,
        Expression<Func<TDto, TProperty>> greaterSelector,
        string errorMessage
    ) where TProperty : IComparable<TProperty>
    {
        RuleFor(x => x).Must(x =>
        {
            var smallerValue = smallerSelector.Compile().Invoke(x);
            var greaterValue = greaterSelector.Compile().Invoke(x);
            return smallerValue.CompareTo(greaterValue) < 0;
        }).WithMessage(errorMessage);
    }

    protected void RuleForOneRequired<TProp1, TProp2>(
        Expression<Func<TDto, TProp1?>> selector1,
        Expression<Func<TDto, TProp2?>> selector2,
        string errorMessage
    )
        where TProp1 : struct
        where TProp2 : struct
    {
        RuleFor(x => x).Must(x =>
        {
            var val1 = selector1.Compile().Invoke(x);
            var val2 = selector2.Compile().Invoke(x);
            return val1.HasValue || val2.HasValue;
        }).WithMessage(errorMessage);
    }
}
