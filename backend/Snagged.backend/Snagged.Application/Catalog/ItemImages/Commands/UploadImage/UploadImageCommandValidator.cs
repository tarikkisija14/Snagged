using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.ItemImages.Commands.UploadImage
{
    public class UploadImageCommandValidator : AbstractValidator<UploadImageCommand>
    {
        private readonly string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };

        public UploadImageCommandValidator()
        {
            RuleFor(x => x.ItemId)
                .GreaterThan(0)
                .WithMessage("ItemId must be greater than 0.");

            RuleFor(x => x.FilesBytes)
                .NotEmpty()
                .WithMessage("At least one file must be uploaded.");

            RuleFor(x => x.FileNames)
                .NotEmpty()
                .WithMessage("File names cannot be empty.");

            RuleFor(x => x.FileNames)
                .Must((command, fileNames) => fileNames.Count == command.FilesBytes.Count)
                .WithMessage("FilesBytes and FileNames count must match.");

            RuleForEach(x => x.FileNames)
                .Must(fn => allowedExtensions.Contains(Path.GetExtension(fn).ToLower()))
                .WithMessage("Only image files (.jpg, .jpeg, .png, .webp) are allowed.");

            RuleForEach(x => x.FilesBytes)
                .Must(f => f != null && f.Length > 0)
                .WithMessage("Files cannot be empty.");
        }
    }
}
