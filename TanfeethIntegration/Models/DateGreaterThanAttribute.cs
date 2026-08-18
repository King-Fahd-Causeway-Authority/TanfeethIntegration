namespace TanfeethIntegration.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;

    public class DateGreaterThanAttribute : ValidationAttribute
    {
        private readonly string _comparisonPropertyName;

        public DateGreaterThanAttribute(string comparisonPropertyName, string errorMessage)
            : base(errorMessage)
        {
            _comparisonPropertyName = comparisonPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Get the property to compare with
            var comparisonProperty = validationContext.ObjectType.GetProperty(_comparisonPropertyName);

            // If the property is not found, return an error
            if (comparisonProperty == null)
            {
                return new ValidationResult($"Unknown property: {_comparisonPropertyName}");
            }

            // Get the value of the comparison property
            var comparisonValue = comparisonProperty.GetValue(validationContext.ObjectInstance);

            // Both values need to be of a type that can be compared
            if (value is DateTime currentValue && comparisonValue is DateTime comparisonDateTime)
            {
                // If the current value is less than the comparison value, return an error
                if (currentValue <= comparisonDateTime)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            // If value is not set or comparison is valid, return Success
            return ValidationResult.Success;
        }
    }
}
