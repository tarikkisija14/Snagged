using MediatR;

namespace Snagged.Application.Catalog.PushSubscriptions.Commands.Subscribe
{
    public class SubscribePushCommand : IRequest
    {
        public string Endpoint { get; set; } = string.Empty;
        public string P256DhKey { get; set; } = string.Empty;
        public string AuthKey { get; set; } = string.Empty;
    }
}