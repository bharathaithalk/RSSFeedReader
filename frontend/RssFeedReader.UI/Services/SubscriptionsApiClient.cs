using System.Net.Http.Json;

namespace RssFeedReader.UI.Services;

public sealed class FeedSubscription
{
    public string Url { get; set; } = string.Empty;
}

public sealed class SubscriptionsApiClient
{
    private readonly HttpClient _httpClient;

    public SubscriptionsApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<FeedSubscription>> GetSubscriptionsAsync()
    {
        var subscriptions = await _httpClient.GetFromJsonAsync<List<FeedSubscription>>("/api/subscriptions");
        return subscriptions ?? new List<FeedSubscription>();
    }

    public async Task<FeedSubscription?> AddSubscriptionAsync(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return null;
        }

        var response = await _httpClient.PostAsJsonAsync("/api/subscriptions", new { url });
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<FeedSubscription>();
    }
}
