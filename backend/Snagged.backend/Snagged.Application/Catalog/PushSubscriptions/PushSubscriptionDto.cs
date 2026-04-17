namespace Snagged.Application.Catalog.PushSubscriptions
{
    public class PushSubscriptionDto
    {
        public string Endpoint { get; set; } = string.Empty;
        public string P256DhKey { get; set; } = string.Empty;
        public string AuthKey { get; set; } = string.Empty;
    }
}