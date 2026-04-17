namespace Snagged.Application.Abstractions
{
    public interface IWebPushService
    {
        Task SendAsync(int userId, string title, string body, CancellationToken ct = default);
    }
}