using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Snagged.Application.Abstractions;
using Snagged.Application.Common.Helper;
using Snagged.Infrastructure.Database;
using System.Text.Json;
using WebPush;

namespace Snagged.Infrastructure.Services
{
    public class WebPushService(
        IAppDbContext ctx,
        IOptions<VapidSettings> vapidOptions,
        ILogger<WebPushService> logger) : IWebPushService
    {
        private readonly VapidSettings _vapid = vapidOptions.Value;

        public async Task SendAsync(int userId, string title, string body, CancellationToken ct = default)
        {
            var subscriptions = await ctx.PushSubscriptions
                .Where(s => s.UserId == userId)
                .ToListAsync(ct);

           


            if (!subscriptions.Any())
                return;

            var payload = JsonSerializer.Serialize(new { title, body });
           


            var vapidDetails = new VapidDetails(
                _vapid.Subject,
                _vapid.PublicKey,
                _vapid.PrivateKey);

            
            using var client = new WebPushClient();

            var staleSubscriptions = new List<Domain.Entities.PushSubscription>();

            foreach (var sub in subscriptions)
            {
                try
                {
                    var pushSubscription = new WebPush.PushSubscription(sub.Endpoint, sub.P256DhKey, sub.AuthKey);
                    await client.SendNotificationAsync(pushSubscription, payload, vapidDetails);
                   

                }
                catch (WebPushException ex) when (
                    ex.StatusCode == System.Net.HttpStatusCode.Gone ||
                    ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    staleSubscriptions.Add(sub);
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex,
                        "Failed to send push notification to userId={UserId}, endpoint={Endpoint}",
                        userId, sub.Endpoint);
                }
            }

            if (staleSubscriptions.Any())
            {
                ctx.PushSubscriptions.RemoveRange(staleSubscriptions);
                await ctx.SaveChangesAsync(ct);
            }

           
        }
    }
}