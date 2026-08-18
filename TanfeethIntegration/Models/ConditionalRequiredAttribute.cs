namespace TanfeethIntegration.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ConditionalRequiredAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;
        private readonly object[] _desiredValues;

        public ConditionalRequiredAttribute(string comparisonProperty, object[] desiredValues, string errorMessage)
            : base(errorMessage)
        {
            _comparisonProperty = comparisonProperty;
            _desiredValues = desiredValues;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PropertyInfo otherProperty = validationContext.ObjectType.GetProperty(_comparisonProperty);
            if (otherProperty == null)
            {
                return new ValidationResult($"Unknown property: {_comparisonProperty}");
            }

            var otherPropertyValue = otherProperty.GetValue(validationContext.ObjectInstance, null);

            if (_desiredValues.Contains(otherPropertyValue) && (value == null || (value is string && string.IsNullOrWhiteSpace((string)value))))
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
