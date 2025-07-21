using System.ComponentModel.DataAnnotations;

namespace CloudBlue.Domain.NewFolder;

public class DateValidatorAttribute(int years = 0, int months = 0, int days = 0) : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success;
        }

        if (years == 0 && months == 0 && days == 0)
        {
            return new ValidationResult("Years, month, or days should be greater than zero.");
        }

        if (value is not DateOnly date)
        {
            return new ValidationResult("invalid Date.");
        }

        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        date = date.AddYears(years)
            .AddMonths(months)
            .AddDays(days);

        if (date <= today)
        {
            return ValidationResult.Success;
        }

        return new ValidationResult(
            $"Date must be at least {years} years, {months} months, and {days} days  older than now.");
    }
}