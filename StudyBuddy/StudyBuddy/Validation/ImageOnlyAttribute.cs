using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace StudyBuddy.Validation
{
    public class ImageOnlyAttribute : ValidationAttribute
    {
        private readonly string[] _validExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var files = value as List<IFormFile>;
            if (files != null)
            {
                foreach (var file in files)
                {
                    if (file != null)
                    {
                        var extension = System.IO.Path.GetExtension(file.FileName).ToLower();
                        if (!_validExtensions.Contains(extension))
                        {
                            return new ValidationResult($"Invalid file type. Only {string.Join(", ", _validExtensions)} are allowed.");
                        }
                    }
                }
            }
            return ValidationResult.Success;
        }
    }
}
