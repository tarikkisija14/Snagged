using FluentAssertions;
using Snagged.Application.Catalog.Adresses.Commands.AddAddress;
using Snagged.Application.Catalog.Cart.Commands;
using Snagged.Application.Catalog.Cart.Commands.AddCartItem;
using Snagged.Application.Catalog.Cart.Commands.Checkout;
using Snagged.Application.Catalog.Cart.Commands.ClearCart;
using Snagged.Application.Catalog.Cart.Commands.DeleteCartItem;
using Snagged.Application.Catalog.Cart.Commands.UpdateCartItem;
using Snagged.Application.Catalog.Categories.Commands.AddCategory;
using Snagged.Application.Catalog.Categories.Commands.UpdateCategory;
using Snagged.Application.Catalog.Cities.Commands;
using Snagged.Application.Catalog.Cities.Commands.AddCity;
using Snagged.Application.Catalog.Countries.Commands;
using Snagged.Application.Catalog.Countries.Commands.AddCountry;
using Snagged.Application.Catalog.ItemImages.Commands;
using Snagged.Application.Catalog.ItemImages.Commands.AddItemImage;
using Snagged.Application.Catalog.Items.Commands.AddItem;
using Snagged.Application.Catalog.Items.Commands.UpdateItem;
using Xunit;

namespace Snagged.Test.Validation
{
    public class ValidatorsIntegrationTests
    {
        [Fact]
        public void AllValidators_ShouldFail_OnInvalidData()
        {
            // ----- Address -----
            var addAddressValidator = new AddAddressCommandValidator();
            var invalidAddress = new AddAddressCommand { Street = "", UserId = 0 };
            var addressResult = addAddressValidator.Validate(invalidAddress);
            addressResult.IsValid.Should().BeFalse();
            addressResult.Errors.Should().Contain(e => e.PropertyName == "Street");
            addressResult.Errors.Should().Contain(e => e.PropertyName == "UserId");


            // ----- Cart Commands -----
            // AddCartItem
            var addCartItemValidator = new AddCartItemCommandValidator();
            var invalidAddCartItem = new AddCartItemCommand { UserId = 0, ItemId = 0, Quantity = 0 };
            var addCartItemResult = addCartItemValidator.Validate(invalidAddCartItem);
            addCartItemResult.IsValid.Should().BeFalse();
            addCartItemResult.Errors.Should().Contain(e => e.PropertyName == "UserId");
            addCartItemResult.Errors.Should().Contain(e => e.PropertyName == "ItemId");
            addCartItemResult.Errors.Should().Contain(e => e.PropertyName == "Quantity");

            // UpdateCartItem
            var updateCartItemValidator = new UpdateCartItemCommandValidator();
            var invalidUpdateCartItem = new UpdateCartitemCommand { CartItemId = 0, Quantity = 0 };
            var updateCartItemResult = updateCartItemValidator.Validate(invalidUpdateCartItem);
            updateCartItemResult.IsValid.Should().BeFalse();
            updateCartItemResult.Errors.Should().Contain(e => e.PropertyName == "CartItemId");
            updateCartItemResult.Errors.Should().Contain(e => e.PropertyName == "Quantity");

            // DeleteCartItem
            var deleteCartItemValidator = new DeleteCartItemCommandValidator();
            var invalidDeleteCartItem = new DeleteCartItemCommand { CartItemId = 0 };
            var deleteCartItemResult = deleteCartItemValidator.Validate(invalidDeleteCartItem);
            deleteCartItemResult.IsValid.Should().BeFalse();
            deleteCartItemResult.Errors.Should().Contain(e => e.PropertyName == "CartItemId");

            // ClearCart
            var clearCartValidator = new ClearCartCommandValidator();
            var invalidClearCart = new ClearCartCommand { UserId = 0 };
            var clearCartResult = clearCartValidator.Validate(invalidClearCart);
            clearCartResult.IsValid.Should().BeFalse();
            clearCartResult.Errors.Should().Contain(e => e.PropertyName == "UserId");

            // Checkout
            var checkoutValidator = new CheckoutCommandValidator();
            var invalidCheckout = new CheckoutCommand { UserId = 0 };
            var checkoutResult = checkoutValidator.Validate(invalidCheckout);
            checkoutResult.IsValid.Should().BeFalse();
            checkoutResult.Errors.Should().Contain(e => e.PropertyName == "UserId");


            // ----- Categories -----
            var addCategoryValidator = new AddCategoryCommandValidator();
            var invalidAddCat = new AddCategoryCommand { Name = "" };
            var addCatResult = addCategoryValidator.Validate(invalidAddCat);
            addCatResult.IsValid.Should().BeFalse();
            addCatResult.Errors.Should().Contain(e => e.PropertyName == "Name");

            var updateCategoryValidator = new UpdateCategoryCommandValidator();
            var invalidUpdateCat = new UpdateCategoryCommand { Id = 0, Name = "" };
            var updateCatResult = updateCategoryValidator.Validate(invalidUpdateCat);
            updateCatResult.IsValid.Should().BeFalse();
            updateCatResult.Errors.Should().Contain(e => e.PropertyName == "Id");
            updateCatResult.Errors.Should().Contain(e => e.PropertyName == "Name");

            // ----- Cities -----
            var addCityValidator = new AddCityCommandValidator();
            var invalidCity = new AddCityCommand { Name = "", CountryId = 0 };
            var cityResult = addCityValidator.Validate(invalidCity);
            cityResult.IsValid.Should().BeFalse();
            cityResult.Errors.Should().Contain(e => e.PropertyName == "Name");
            cityResult.Errors.Should().Contain(e => e.PropertyName == "CountryId");

            // ----- Countries -----
            var addCountryValidator = new AddCountryCommandValidator();
            var invalidCountry = new AddCountryCommand { Name = "" };
            var countryResult = addCountryValidator.Validate(invalidCountry);
            countryResult.IsValid.Should().BeFalse();
            countryResult.Errors.Should().Contain(e => e.PropertyName == "Name");

            // ----- Items -----
            var addItemValidator = new AddItemCommandValidator();
            var invalidAddItem = new AddItemCommand
            {
                Title = "",
                Description = "",
                Price = -1,
                Condition = null!,
                CategoryId = 0,
                UserId = 0
            };
            var addItemResult = addItemValidator.Validate(invalidAddItem);
            addItemResult.IsValid.Should().BeFalse();
            addItemResult.Errors.Should().Contain(e => e.PropertyName == "Title");
            addItemResult.Errors.Should().Contain(e => e.PropertyName == "Description");
            addItemResult.Errors.Should().Contain(e => e.PropertyName == "Price");
            addItemResult.Errors.Should().Contain(e => e.PropertyName == "Condition");
            addItemResult.Errors.Should().Contain(e => e.PropertyName == "CategoryId");
            addItemResult.Errors.Should().Contain(e => e.PropertyName == "UserId");

            var updateItemValidator = new UpdateItemCommandValidator();
            var invalidUpdateItem = new UpdateItemCommand
            {
                Id = 0,
                Title = "",
                Description = "",
                Price = -5,
                Condition = "",
                CategoryId = 0,
                IsSold = false 
            };
            var updateItemResult = updateItemValidator.Validate(invalidUpdateItem);
            updateItemResult.IsValid.Should().BeFalse();
            updateItemResult.Errors.Should().Contain(e => e.PropertyName == "Id");
            updateItemResult.Errors.Should().Contain(e => e.PropertyName == "Title");
            updateItemResult.Errors.Should().Contain(e => e.PropertyName == "Description");
            updateItemResult.Errors.Should().Contain(e => e.PropertyName == "Price");
            updateItemResult.Errors.Should().Contain(e => e.PropertyName == "Condition");
            updateItemResult.Errors.Should().Contain(e => e.PropertyName == "CategoryId");

            // ----- ItemImages -----
            var addImageValidator = new AddItemImageCommandValidator();
            var invalidImage = new AddItemImageCommand { ItemId = 0, ImageUrl = "" };
            var imageResult = addImageValidator.Validate(invalidImage);
            imageResult.IsValid.Should().BeFalse();
            imageResult.Errors.Should().Contain(e => e.PropertyName == "ItemId");
            imageResult.Errors.Should().Contain(e => e.PropertyName == "ImageUrl");
        }
    }
}
