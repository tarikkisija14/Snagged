using MediatR;

namespace Snagged.Application.Catalog.PushSubscriptions.Queries.GetPushSubscriptionStatus
{
    public class GetPushSubscriptionStatusQuery : IRequest<bool>
    {
        public string Endpoint { get; set; } = string.Empty;
    }
}