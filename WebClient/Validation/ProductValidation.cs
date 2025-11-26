using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace WebClient.Validation
{
    // ===== BASE ATTRIBUTE =====
    public abstract class BaseValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;
            var strValue = value.ToString() ?? "";
            return Validate(strValue) ? ValidationResult.Success : new ValidationResult(ErrorMessage);
        }

        protected abstract bool Validate(string value);
    }

    // ===== 1. FORBIDDEN WORDS =====
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ForbiddenWordsAttribute : BaseValidationAttribute
    {
        private readonly string[] _forbiddenWords;

        public ForbiddenWordsAttribute(params string[] forbiddenWords)
        {
            _forbiddenWords = forbiddenWords.Length > 0 ? forbiddenWords : new[] { "Online", "Game" };
            ErrorMessage = $"Field cannot contain any of the following words: {string.Join(", ", _forbiddenWords)}.";
        }

        protected override bool Validate(string value)
        {
            return !_forbiddenWords.Any(word => value.Contains(word, StringComparison.OrdinalIgnoreCase));
        }
    }
    // 👉 Example usage:
    // [ForbiddenWords("Game", "Online")]

    // ===== 2. NO CHARACTERS (Generic) =====
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class NoCharactersAttribute : BaseValidationAttribute
    {
        private readonly char[] _forbiddenCharsLower;

        public NoCharactersAttribute(string forbiddenChars)
        {
            if (string.IsNullOrWhiteSpace(forbiddenChars))
                throw new ArgumentException("You must specify at least one forbidden character.", nameof(forbiddenChars));

            _forbiddenCharsLower = forbiddenChars.ToLowerInvariant().ToCharArray();
            ErrorMessage = $"Field cannot contain any of the following characters: {string.Join(", ", _forbiddenCharsLower)}.";
        }

        protected override bool Validate(string value)
        {
            return !value.ToLowerInvariant().Any(ch => _forbiddenCharsLower.Contains(ch));
        }
    }
    // 👉 Example usage:
    // [NoCharacters("kK")] → cấm ký tự 'k' hoặc 'K'

    // ===== 3. NO SPECIAL CHAR =====
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class NoSpecialCharAttribute : BaseValidationAttribute
    {
        public NoSpecialCharAttribute()
        {
            ErrorMessage = "Field cannot contain special characters.";
        }

        protected override bool Validate(string value)
        {
            return value.All(ch => char.IsLetterOrDigit(ch) || char.IsWhiteSpace(ch));
        }
    }
    // 👉 Example usage:
    // [NoSpecialChar]

    // ===== 4. NO DIGIT =====
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class NoDigitAttribute : BaseValidationAttribute
    {
        public NoDigitAttribute()
        {
            ErrorMessage = "Field cannot contain numbers.";
        }

        protected override bool Validate(string value)
        {
            return !value.Any(char.IsDigit);
        }
    }
    // 👉 Example usage:
    // [NoDigit]

    // ===== 5. CAPITALIZED FIRST LETTER =====
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class CapitalizedFirstLetterAttribute : BaseValidationAttribute
    {
        public CapitalizedFirstLetterAttribute()
        {
            ErrorMessage = "The first letter must be uppercase.";
        }

        protected override bool Validate(string value)
        {
            return string.IsNullOrEmpty(value) || char.IsUpper(value[0]);
        }
    }
    // 👉 Example usage:
    // [CapitalizedFirstLetter]

    // ===== 6. STRONG PASSWORD =====
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class StrongPasswordAttribute : BaseValidationAttribute
    {
        public StrongPasswordAttribute()
        {
            ErrorMessage = "Password must contain at least one uppercase, one lowercase, one digit, and one special character.";
        }

        protected override bool Validate(string value)
        {
            bool hasUpper = value.Any(char.IsUpper);
            bool hasLower = value.Any(char.IsLower);
            bool hasDigit = value.Any(char.IsDigit);
            bool hasSpecial = value.Any(ch => !char.IsLetterOrDigit(ch));
            return hasUpper && hasLower && hasDigit && hasSpecial;
        }
    }
    // 👉 Example usage:
    // [StrongPassword]

    // ===== 7. NO LEADING/TRAILING SPACE =====
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class NoLeadingTrailingSpaceAttribute : BaseValidationAttribute
    {
        public NoLeadingTrailingSpaceAttribute()
        {
            ErrorMessage = "Field cannot start or end with whitespace.";
        }

        protected override bool Validate(string value)
        {
            return value == value.Trim();
        }
    }
    // 👉 Example usage:
    // [NoLeadingTrailingSpace]

    // ===== 8. EMAIL DOMAIN =====
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class EmailDomainAttribute : BaseValidationAttribute
    {
        private readonly string _domain;

        public EmailDomainAttribute(string domain)
        {
            _domain = domain;
            ErrorMessage = $"Email must end with @{domain}.";
        }

        protected override bool Validate(string value)
        {
            return value.EndsWith("@" + _domain, StringComparison.OrdinalIgnoreCase);
        }
    }
    // 👉 Example usage:
    // [EmailDomain("fpt.edu.vn")]

    // ===== 9. PAST DATE =====
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class PastDateAttribute : ValidationAttribute
    {
        public PastDateAttribute()
        {
            ErrorMessage = "Date must be in the past.";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not DateTime date) return ValidationResult.Success;
            return date <= DateTime.Today
                ? ValidationResult.Success
                : new ValidationResult(ErrorMessage);
        }
    }
    // 👉 Example usage:
    // [PastDate]

    // ===== 10. NOWORDS =====
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class NoWordsAttribute : ValidationAttribute
    {
        private readonly string[] _forbiddenWords;
        private readonly bool _matchWholeWord;

        public NoWordsAttribute(bool matchWholeWord, params string[] forbiddenWords)
        {
            if (forbiddenWords == null || forbiddenWords.Length == 0)
                throw new ArgumentException("You must specify at least one forbidden word.", nameof(forbiddenWords));

            _forbiddenWords = forbiddenWords;
            _matchWholeWord = matchWholeWord;
            ErrorMessage = $"Field cannot contain any of the following words: {string.Join(", ", forbiddenWords)}.";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;
            string strValue = value.ToString() ?? "";

            foreach (var word in _forbiddenWords)
            {
                if (_matchWholeWord)
                {
                    var pattern = $@"\b{Regex.Escape(word)}\b";
                    if (Regex.IsMatch(strValue, pattern, RegexOptions.IgnoreCase))
                        return new ValidationResult(ErrorMessage);
                }
                else
                {
                    if (strValue.Contains(word, StringComparison.OrdinalIgnoreCase))
                        return new ValidationResult(ErrorMessage);
                }
            }
            return ValidationResult.Success;
        }
    }
    // 👉 Example usage:
    // [NoWords(false, "spam", "fake", "ads")]
    // [NoWords(true, "admin", "root")]

    // ===== 11. PHONE NUMBER FORMAT =====
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class PhoneNumberFormatAttribute : BaseValidationAttribute
    {
        private readonly string _pattern;

        /// <summary>
        /// Validate format số điện thoại (mặc định: Việt Nam, hoặc định dạng quốc tế).
        /// </summary>
        /// <param name="country">"VN" (default) hoặc "INTL" cho định dạng quốc tế</param>
        public PhoneNumberFormatAttribute(string country = "VN")
        {
            if (country.Equals("INTL", StringComparison.OrdinalIgnoreCase))
            {
                // Cho phép +, mã vùng, khoảng trắng, dấu gạch
                _pattern = @"^\+?\d{1,4}?[\s-]?\(?\d+\)?([\s-]?\d+){5,}$";
                ErrorMessage = "Invalid international phone number format.";
            }
            else
            {
                // Format Việt Nam: bắt đầu 0, theo sau là 9-10 chữ số
                _pattern = @"^(0|\+84)[0-9]{9}$";
                ErrorMessage = "Invalid Vietnamese phone number format.";
            }
        }

        protected override bool Validate(string value)
        {
            return Regex.IsMatch(value, _pattern);
        }
    }
    // 👉 Example usage:
    // [PhoneNumberFormat] → Số VN: 0901234567 hoặc +84901234567
    // [PhoneNumberFormat("INTL")] → Số quốc tế: +1 800-123-4567

    // ===== 12. STOCK QUANTITY NON-NEGATIVE =====
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class NonNegativeStockAttribute : ValidationAttribute
    {
        public NonNegativeStockAttribute()
        {
            ErrorMessage = "Stock quantity must be greater than or equal to 0.";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            if (decimal.TryParse(value.ToString(), out var number))
            {
                return number >= 0
                    ? ValidationResult.Success
                    : new ValidationResult(ErrorMessage);
            }

            return new ValidationResult("Invalid number format.");
        }
    }
    // 👉 Example usage:
    //[NonNegativeStock]
}
