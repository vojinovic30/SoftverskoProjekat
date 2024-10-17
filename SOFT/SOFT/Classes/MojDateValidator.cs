using System;
using System.ComponentModel.DataAnnotations;

public class MojDateValidator :ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        value=(DateTime) value;

        if(DateTime.Now.CompareTo(value)<0)
        {
            return ValidationResult.Success;
        }
        else
        {
            return new ValidationResult("Datum mora da bude posle danjasnjeg!");
        }
    }
}