using MediatR;

namespace Snagged.Application.Catalog.PushSubscriptions.Commands.Unsubscribe
{
    public class UnsubscribePushCommand : IRequest
    {
        public string Endpoint { get; set; } = string.Empty;
    }
}