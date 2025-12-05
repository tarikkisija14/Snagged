using FluentValidation.TestHelper;
using Snagged.Application.Catalog.Subcategories.Commands.AddSubcategory;
using Snagged.Application.Catalog.Subcategories.Commands.UpdateSubCategory;
using Snagged.Application.Catalog.Review.Commands.AddReview;
using Xunit;

namespace Snagged.Test
{
    public class ValidatorTests
    {
        [Fact]
        public void AddSubcategoryCommandValidator_Should_HaveErrors_WhenInvalid()
        {
            var validator = new AddSubcategoryCommandValidator();
            var command = new AddSubcategoryCommand
            {
                Name = "", // empty invalid
                CategoryId = 0 // invalid
            };

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Name);
            result.ShouldHaveValidationErrorFor(x => x.CategoryId);
        }

        [Fact]
        public void UpdateSubcategoryCommandValidator_Should_HaveErrors_WhenInvalid()
        {
            var validator = new UpdateSubcategoryCommandValidator();
            var command = new UpdateSubcategoryCommand
            {
                Id = 0, // invalid
                Name = "", // invalid
                CategoryId = 0 // invalid
            };

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Id);
            result.ShouldHaveValidationErrorFor(x => x.Name);
            result.ShouldHaveValidationErrorFor(x => x.CategoryId);
        }

        [Fact]
        public void AddReviewCommandValidator_Should_HaveErrors_WhenInvalid()
        {
            var validator = new AddReviewCommandValidator();
            var command = new AddReviewCommand
            {
                ReviewerId = 0,
                ReviewedUserId = 0,
                Rating = 6, // invalid, should be 1-5
                Comment = "" // invalid
            };

            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.ReviewerId);
            result.ShouldHaveValidationErrorFor(x => x.ReviewedUserId);
            result.ShouldHaveValidationErrorFor(x => x.Rating);
            result.ShouldHaveValidationErrorFor(x => x.Comment);
        }

        [Fact]
        public void AddReviewCommandValidator_Should_Pass_WhenValid()
        {
            var validator = new AddReviewCommandValidator();
            var command = new AddReviewCommand
            {
                ReviewerId = 1,
                ReviewedUserId = 2,
                Rating = 5,
                Comment = "Great job!"
            };

            var result = validator.TestValidate(command);

            result.ShouldNotHaveValidationErrorFor(x => x.ReviewerId);
            result.ShouldNotHaveValidationErrorFor(x => x.ReviewedUserId);
            result.ShouldNotHaveValidationErrorFor(x => x.Rating);
            result.ShouldNotHaveValidationErrorFor(x => x.Comment);
        }
    }
}
