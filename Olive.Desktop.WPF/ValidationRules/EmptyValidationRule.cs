namespace Olive.Desktop.WPF.ValidationRules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Controls;
    using System.Globalization;

    class EmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            //if (!((string)value).Equals("a"))
            //{
            //    return new ValidationResult(true, null);
            //}
            //else
            //{
            //    return new ValidationResult(false,
            //        "вкарахте различно a");
            //}

            if (!String.IsNullOrEmpty((string)value))
            {
                return new ValidationResult(true, null);
            }
            else
            {
                return new ValidationResult(false,
                    "(От validation rules)Първо попълни празното поле");
            }

            #region Original
            //string pattern = @"\A\w+ \w+\Z";
            //if (Regex.IsMatch((string)value, pattern))
            //{
            //    return new ValidationResult(true, null);
            //}
            //else
            //{
            //    return new ValidationResult(false,
            //        "Full name should be in format '<FirstName> <LastName>'");
            //} 
            #endregion
        }
    }
}
