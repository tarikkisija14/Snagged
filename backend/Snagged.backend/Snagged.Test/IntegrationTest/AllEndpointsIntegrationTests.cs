using Microsoft.VisualStudio.TestPlatform.TestHost;
using Snagged.Application.Catalog.Payment.Commands.CreatePayment;
using Snagged.Application.Catalog.Payment.Commands.CreateStripePayment;
using Snagged.Application.Catalog.Review.Commands.AddReview;
using Snagged.Application.Catalog.Review.Commands.UpdateReview;
using Snagged.Application.Catalog.Subcategories.Commands.AddSubcategory;
using Snagged.Application.Catalog.Subcategories.Commands.UpdateSubCategory;
using Snagged.Test.Infrastructure;
using System.Net.Http.Json;

public class AllEndpointsIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AllEndpointsIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task AllEndpoints_CRUD_Workflow()
    {
        // ----------------- SUBCATEGORY -----------------
        var createSub = new AddSubcategoryCommand { Name = "TestSub", CategoryId = 1 };
        var subResp = await _client.PostAsJsonAsync("/api/subcategories", createSub);
        subResp.EnsureSuccessStatusCode();
        var subId = await subResp.Content.ReadFromJsonAsync<int>();

        var getSub = await _client.GetAsync($"/api/subcategories/{subId}");
        getSub.EnsureSuccessStatusCode();

        var updateSub = new UpdateSubcategoryCommand { Id = subId, Name = "UpdatedSub", CategoryId = 1 };
        var updateResp = await _client.PutAsJsonAsync("/api/subcategories", updateSub);
        updateResp.EnsureSuccessStatusCode();

        var deleteResp = await _client.DeleteAsync($"/api/subcategories/{subId}");
        deleteResp.EnsureSuccessStatusCode();

        // ----------------- ORDER -----------------
        var orderId = 1; // Pretpostavimo da već postoji order u seed-u
        var getOrder = await _client.GetAsync($"/api/orders/{orderId}");
        getOrder.EnsureSuccessStatusCode();

        var getPagedOrders = await _client.GetAsync("/api/orders/paged?pageNumber=1&pageSize=10");
        getPagedOrders.EnsureSuccessStatusCode();

        // ----------------- PAYMENT -----------------
        var createPayment = new CreatePaymentCommand { OrderId = orderId, PaymentMethod = "Cash", Amount = 100 };
        var paymentResp = await _client.PostAsJsonAsync("/api/payments", createPayment);
        paymentResp.EnsureSuccessStatusCode();

        var createStripe = new CreateStripePaymentCommand { OrderId = orderId, Currency = "usd" };
        var stripeResp = await _client.PostAsJsonAsync("/api/payments/stripe", createStripe);
        stripeResp.EnsureSuccessStatusCode();

        // ----------------- REVIEW -----------------
        var addReview = new AddReviewCommand { ReviewerId = 1, ReviewedUserId = 2, Rating = 5, Comment = "Great!" };
        var reviewResp = await _client.PostAsJsonAsync("/api/reviews", addReview);
        reviewResp.EnsureSuccessStatusCode();
        var reviewId = await reviewResp.Content.ReadFromJsonAsync<int>();

        var updateReview = new UpdateReviewCommand { Id = reviewId, Rating = 4, Comment = "Good" };
        var updateReviewResp = await _client.PutAsJsonAsync("/api/reviews", updateReview);
        updateReviewResp.EnsureSuccessStatusCode();

        var deleteReviewResp = await _client.DeleteAsync($"/api/reviews/{reviewId}");
        deleteReviewResp.EnsureSuccessStatusCode();
    }
}
