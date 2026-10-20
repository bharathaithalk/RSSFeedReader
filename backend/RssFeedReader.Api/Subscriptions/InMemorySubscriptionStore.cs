namespace RssFeedReader.Api.Subscriptions;

public sealed class InMemorySubscriptionStore
{
    private readonly List<FeedSubscription> _subscriptions = [];

    public IReadOnlyList<FeedSubscription> GetAll() => _subscriptions.AsReadOnly();

    public FeedSubscription Add(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            throw new ArgumentException("A subscription URL is required.", nameof(url));
        }

        var existing = _subscriptions.FirstOrDefault(item => string.Equals(item.Url, url, StringComparison.Ordinal));
        if (existing is not null)
        {
            return existing;
        }

        var subscription = new FeedSubscription { Url = url };
        _subscriptions.Add(subscription);
        return subscription;
    }

    public bool Contains(string? url)
    {
        return !string.IsNullOrWhiteSpace(url)
            && _subscriptions.Any(sub => string.Equals(sub.Url, url, StringComparison.Ordinal));
    }
}
